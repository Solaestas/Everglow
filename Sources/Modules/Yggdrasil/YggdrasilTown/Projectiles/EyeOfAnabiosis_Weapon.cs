using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Weapon : ModProjectile
{
	private const int MaxChargeTime = 720;
	private const int MaxTargetCount = 3;
	private const int SearchDistance = 500;
	private const float ProjectileRandomRotation = 0.2f;

	private int ChargeTimer { get; set; } = 0;

	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	private Item HeldItem => Main.mouseItem.IsAir ? Owner.HeldItem : Main.mouseItem;

	private Vector2 OwnerMouseWorld
	{
		get => new Vector2(Projectile.ai[0], Projectile.ai[1]);
		set
		{
			Projectile.ai[0] = value.X;
			Projectile.ai[1] = value.Y;
		}
	}

	public override void SetDefaults()
	{
		Projectile.width = 72;
		Projectile.height = 66;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
	}

	public override void AI()
	{
		SyncOwnerMouseWorld();
		KillHoldout();
		ManageHoldout();
		HoldoutAI();
	}

	private void SyncOwnerMouseWorld()
	{
		if (Projectile.owner != Main.myPlayer)
		{
			return;
		}

		if (Main.MouseWorld == OwnerMouseWorld)
		{
			return;
		}

		OwnerMouseWorld = Main.MouseWorld;
		Projectile.netUpdate = true;
	}

	private void KillHoldout()
	{
		bool canUseHoldout =
			Owner == null
			|| !Owner.active
			|| Owner.dead
			|| Owner.CCed
			|| Owner.noItems;
		if (canUseHoldout)
		{
			Projectile.Kill();
			return;
		}

		if (HeldItem.type == ModContent.ItemType<EyeOfAnabiosis>())
		{
			Projectile.timeLeft = 2;
		}
		else
		{
			Projectile.Kill();
			return;
		}
	}

	private void ManageHoldout()
	{
		Owner.heldProj = Projectile.whoAmI;
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI + Owner.direction * MathF.PI * 2 / 3);

		Projectile.Center = Owner.MountedCenter;
		Projectile.velocity *= 0;
	}

	private void HoldoutAI()
	{
		if (ChargeTimer++ < MaxChargeTime)
		{
			return;
		}
		else
		{
			if (Main.time % 3 == 0)
			{
				var offset = new Vector2(MathF.Cos((float)Main.time * 3f) * Owner.width / 2, MathF.Sin((float)Main.time * 2f) * Owner.width / 2);
				Dust.NewDust(Owner.Center + offset, 1, 1, DustID.Shadowflame, newColor: new Color(51, 202, 235), Scale: Main.rand.NextFloat(0.8f, 1.1f));
			}
		}

		if (!Owner.controlUseItem)
		{
			return;
		}

		if (Owner.itemTime == 0)
		{
			bool manaCostPaid = Owner.CheckMana(HeldItem, pay: true);
			if (manaCostPaid)
			{
				Vector2 projPosition = Projectile.Center + new Vector2(Owner.direction * Projectile.width, 0);
				if (Owner.direction == -1)
				{
					projPosition.X += Projectile.width * 2 / 3;
				}
				Vector2 projVelocity = Vector2.Normalize(OwnerMouseWorld - projPosition) * HeldItem.shootSpeed;

				List<NPC> targets = SearchTargets();
				foreach (NPC target in targets)
				{
					Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), projPosition, projVelocity.RotatedBy(Main.rand.NextFloat(-ProjectileRandomRotation, ProjectileRandomRotation)), ModContent.ProjectileType<EyeOfAnabiosis_Projectile>(), HeldItem.damage, HeldItem.knockBack, Projectile.owner, target.whoAmI);
				}
				for (int i = 0; i < MaxTargetCount - targets.Count; i++)
				{
					Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), projPosition, projVelocity.RotatedBy(Main.rand.NextFloat(-ProjectileRandomRotation, ProjectileRandomRotation)), ModContent.ProjectileType<EyeOfAnabiosis_Projectile>(), HeldItem.damage, HeldItem.knockBack, Projectile.owner, -1);
				}

				SoundEngine.PlaySound(SoundID.DD2_BetsysWrathShot, Projectile.Center);
				Owner.ItemCheck_ApplyManaRegenDelay(HeldItem);
				Owner.itemTime = HeldItem.useTime;
			}
		}

		Owner.direction = (Main.MouseWorld - Owner.MountedCenter).X < 0 ? -1 : 1;
	}

	private List<NPC> SearchTargets()
	{
		List<NPC> targets = [];

		foreach (NPC npc in Main.ActiveNPCs)
		{
			if (!npc.friendly
				&& !npc.dontTakeDamage
				&& npc.CanBeChasedBy()
				&& Vector2.Distance(Owner.Center, npc.Center) <= SearchDistance)
			{
				targets.Add(npc);
			}
		}

		return targets.OrderBy(x => Vector2.Distance(Owner.Center, x.Center)).Take(MaxTargetCount).ToList();
	}

	public override void PostDraw(Color lightColor)
	{
		var magicCircleTexture = ModAsset.EyeOfAnabiosis_MagicCircle.Value;
		var magicCirclePosition = Owner.Bottom - Main.screenPosition;
		//Main.spriteBatch.Draw(magicCircleTexture, magicCirclePosition, null, Color.White, 0, magicCircleTexture.Size() / 2, 1, SpriteEffects.None, 0);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float chargeProgress = (float)ChargeTimer / MaxChargeTime > 1f ? 1f : (float)ChargeTimer / MaxChargeTime * 0.7f;
		var drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));

		var body_texture = ModAsset.EyeOfAnabiosis_Body.Value;
		var body_position_offset = new Vector2(Owner.direction * (body_texture.Width / 2 - 12), Owner.gravDir * (-body_texture.Height / 2 + 16)) * Projectile.scale;
		var body_position = Projectile.Center - Main.screenPosition + body_position_offset;
		var body_rotation = Owner.direction == 1 ? 0f : MathF.PI;
		var body_effects = (Owner.direction == 1 && Owner.gravDir == 1) || (Owner.gravDir == -1 && Owner.direction == -1) ? SpriteEffects.None : SpriteEffects.FlipVertically;
		var body_origin = body_texture.Size() / 2;

		Main.spriteBatch.Draw(body_texture, body_position, null, drawColor, body_rotation, body_origin, Projectile.scale, body_effects, 0);

		var head_texture = ModAsset.EyeOfAnabiosis_Head.Value;
		var head_position_offset = new Vector2(Owner.direction * 23, Owner.gravDir * -10) * Projectile.scale;
		var head_position = body_position + head_position_offset;
		var head_rotation = Owner.gravDir == 1 ? 0 : MathF.PI;
		head_rotation += 0.2f * MathF.Sin((float)Main.timeForVisualEffects * 0.05f);
		var head_effects = (Owner.direction == 1 && Owner.gravDir == 1) || (Owner.gravDir == -1 && Owner.direction == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		var head_rope_texture = ModAsset.EyeOfAnabiosis_Rope.Value;
		var head_rope_origin = new Vector2(head_rope_texture.Width / 2, 0);
		var head_ball_texture = ModAsset.EyeOfAnabiosis_Head_Ball.Value;
		var head_ball_origin = head_rope_origin + new Vector2(0, -12);
		var head_ballbg_texture = ModAsset.EyeOfAnabiosis_Head_BallBG.Value;
		var head_ballbg_origin = head_ball_origin + new Vector2((head_ballbg_texture.Width - head_ball_texture.Width) / 2, (head_ballbg_texture.Height - head_ball_texture.Height) / 2);
		var head_container_texture = ModAsset.EyeOfAnabiosis_Head_Container.Value;
		var head_container_origin = head_ball_origin + new Vector2((head_container_texture.Width - head_ball_texture.Width) / 2, -10);

		#region Effects
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Effect shineEffect = ModAsset.EyeOfAnabiosis_Shine.Value;
		shineEffect.Parameters["uImageSize"].SetValue(head_texture.Size());
		shineEffect.Parameters["uChargeProgress"].SetValue(chargeProgress);
		shineEffect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects);
		shineEffect.CurrentTechnique.Passes["Pixel"].Apply();
		Main.spriteBatch.Draw(head_ballbg_texture, head_position, null, drawColor, head_rotation, head_ballbg_origin, Projectile.scale, head_effects, 0);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
		#endregion

		Main.spriteBatch.Draw(head_container_texture, head_position, null, drawColor, head_rotation, head_container_origin, Projectile.scale, head_effects, 0);
		Main.spriteBatch.Draw(head_ball_texture, head_position, null, drawColor, head_rotation, head_ball_origin, Projectile.scale, head_effects, 0);
		Main.spriteBatch.Draw(head_rope_texture, head_position, null, drawColor, head_rotation, head_rope_origin, Projectile.scale, head_effects, 0);

		return false;
	}
}