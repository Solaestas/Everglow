using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Weapon : ModProjectile
{
	public const int MaxChargeTime = 720;
	private const int MaxTargetCount = 3;
	private const float ProjectileRandomRotation = 0.2f;

	public int ChargeTimer { get; set; }

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

	public override void SetStaticDefaults()
	{
		Main.projFrames[Type] = 4;
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

		ChargeTimer = 0;
	}

	public override void AI()
	{
		if (Main.time % Main.projFrames[Type] == 0)
		{
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
		}
		SyncOwnerMouseWorld();
		KillHoldout();
		ManageHoldout();
		HoldoutAI();
		UpdateRotationSuspension();
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

	public float Suspension_Rotation;
	public float Suspension_Omega;

	private void HoldoutAI()
	{
		if (ChargeTimer++ < MaxChargeTime)
		{
			if (ChargeTimer == MaxChargeTime - 10)
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Owner.Center, Vector2.zeroVector, ModContent.ProjectileType<EyeOfAnabiosis_Matrix>(), 0, 0, Projectile.owner);
			}
			return;
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
				Vector2 projPosition = Owner.Center + new Vector2(Owner.direction * Projectile.width, 0) * Projectile.scale;
				if (Owner.direction == -1)
				{
					projPosition.X += Projectile.width * 0.66f * Projectile.scale;
				}

				Vector2 projVelocity = Vector2.Normalize(OwnerMouseWorld - projPosition) * HeldItem.shootSpeed;

				for (int i = 0; i < MaxTargetCount; i++)
				{
					Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), Owner.Center + new Vector2(0, -24), projVelocity.RotatedBy(Main.rand.NextFloat(-ProjectileRandomRotation, ProjectileRandomRotation)), ModContent.ProjectileType<EyeOfAnabiosis_Projectile>(), HeldItem.damage, HeldItem.knockBack, Projectile.owner);
				}

				SoundEngine.PlaySound(SoundID.DD2_BetsysWrathShot, Projectile.Center);
				Owner.ItemCheck_ApplyManaRegenDelay(HeldItem);
				Owner.itemTime = HeldItem.useTime;
			}
		}

		Owner.direction = (Main.MouseWorld - Owner.MountedCenter).X < 0 ? -1 : 1;
	}

	private void UpdateRotationSuspension()
	{
		if (MathF.Abs(Suspension_Rotation) < 1.5f)
		{
			Suspension_Omega += 0.01f * (MathF.Sin((float)Main.timeForVisualEffects * 0.05f) + 0.75f) * Main.windSpeedCurrent + Owner.velocity.X * 0.006f;
			Suspension_Omega -= Suspension_Rotation * 0.04f;
			Suspension_Omega *= 0.99f;
			Suspension_Rotation += Suspension_Omega;
		}
		else
		{
			Suspension_Omega -= Suspension_Rotation * 0.04f;
			Suspension_Omega *= 0.99f;
			Suspension_Rotation += Suspension_Omega;
		}
	}

	public override void PostDraw(Color lightColor)
	{
		float chargeProgress = (float)ChargeTimer / MaxChargeTime > 1f ? 1f : (float)ChargeTimer / MaxChargeTime;
		if (chargeProgress == 1)
		{
			SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect shineEffect = ModAsset.EyeOfAnabiosis_Shine.Value;
			shineEffect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects);
			shineEffect.Parameters["uNoise"].SetValue(Commons.ModAsset.NoiseWave.Value);
			shineEffect.Parameters["uChargeProgress"].SetValue(chargeProgress);
			shineEffect.CurrentTechnique.Passes["MagicCircle_Pixel"].Apply();

			var magicCirTexture = Commons.ModAsset.Point.Value;
			var magicCirPosition = Owner.gravDir == 1 ? Owner.Bottom : Owner.Top;
			magicCirPosition = magicCirPosition - Main.screenPosition + new Vector2(0, 2 * Owner.gravDir);
			var magicCirScale = new Vector2(0.18f, 0.12f);
			Main.spriteBatch.Draw(magicCirTexture, magicCirPosition, null, Color.White, Owner.gravDir == 1 ? 0 : MathF.PI, new Vector2(magicCirTexture.Width / 2, magicCirTexture.Height), magicCirScale, SpriteEffects.None, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float chargeProgress = (float)ChargeTimer / MaxChargeTime > 1f ? 1f : (float)ChargeTimer / MaxChargeTime;
		lightColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));

		var body_texture = ModAsset.EyeOfAnabiosis_Body.Value;
		var body_position_offset = new Vector2(Owner.direction * (body_texture.Width / 2 - 12), Owner.gravDir * (-body_texture.Height / 2 + 16)) * Projectile.scale;
		var body_position = Projectile.Center - Main.screenPosition + body_position_offset;
		var body_rotation = Owner.direction == 1 ? 0f : MathF.PI;
		var body_effects = (Owner.direction == 1 && Owner.gravDir == 1) || (Owner.gravDir == -1 && Owner.direction == -1) ? SpriteEffects.None : SpriteEffects.FlipVertically;
		var body_origin = body_texture.Size() / 2;

		Main.spriteBatch.Draw(body_texture, body_position, null, lightColor, body_rotation, body_origin, Projectile.scale, body_effects, 0);

		var head_texture = ModAsset.EyeOfAnabiosis_Head.Value;
		var head_position_offset = new Vector2(Owner.direction * 23, Owner.gravDir * -10) * Projectile.scale;
		var head_position = body_position + head_position_offset;
		var head_rotation = Owner.gravDir == 1 ? 0 : MathF.PI;
		{
			head_rotation += Suspension_Rotation; // 0.1f * (MathF.Sin((float)Main.timeForVisualEffects * 0.05f) + 0.75f) * Main.windSpeedCurrent + Owner.velocity.X * 0.1f;
		}
		var head_effects = (Owner.direction == 1 && Owner.gravDir == 1) || (Owner.gravDir == -1 && Owner.direction == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		var head_rope_texture = ModAsset.EyeOfAnabiosis_Rope.Value;
		var head_rope_origin = new Vector2(head_rope_texture.Width / 2, 0);
		var head_ball_texture = ModAsset.EyeOfAnabiosis_Head_Ball.Value;
		var head_ball_origin = head_rope_origin + new Vector2((head_ball_texture.Width - head_rope_texture.Width) / 2, -head_rope_texture.Height + 2);
		var head_ballbg_texture = ModAsset.EyeOfAnabiosis_Head_BallBG.Value;
		var head_ballbg_origin = head_ball_origin + new Vector2((head_ballbg_texture.Width - head_ball_texture.Width) / 2, (head_ballbg_texture.Height - head_ball_texture.Height) / 2);

		var head_container_texture = ModAsset.EyeOfAnabiosis_Head_Container.Value;
		var head_container_position_offset = (-head_ballbg_origin + new Vector2(head_ball_texture.Width + (head_container_texture.Width - head_ball_texture.Width) / 2) / 2).RotatedBy(head_rotation) * Projectile.scale;
		var head_container_position = head_position + head_container_position_offset;
		var head_container_origin = new Vector2(head_container_texture.Width / 2, 0);
		var head_container_rotation = 0.3f * head_rotation;

		var fireProgress = 0.2f + 0.8f * chargeProgress;
		var head_fire_texture = ModAsset.EyeOfAnabiosis_Projectile.Value;
		var head_fire_origin = new Vector2(head_fire_texture.Width / 8, head_fire_texture.Height * (1.5f - 1 / fireProgress) / 2);
		var frame = head_fire_texture.Frame(horizontalFrames: Main.projFrames[Type], frameX: Projectile.frame);

		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		// Draw charge progress UI
		if (Main.myPlayer == Projectile.owner && chargeProgress < 1)
		{
			var progressTexture = Commons.ModAsset.White.Value;
			var progressPosition = Owner.Center - Main.screenPosition + Owner.gravDir * new Vector2(0, 36);

			Color frameColor = new Color(0.05f, 0.05f, 0.08f, 0.4f);
			Vector2 frameScale = new Vector2(2f, 0.6f) * 0.05f;

			Color lineColor = Color.SkyBlue;
			Vector2 lineScale = new Vector2(1.6f * chargeProgress, 0.3f) * 0.05f;
			Vector2 linePositionOffset = new Vector2(-1.6f * (1 - chargeProgress) * progressTexture.Width * 0.025f, 0);

			Main.spriteBatch.Draw(progressTexture, progressPosition, null, frameColor, 0, progressTexture.Size() / 2, frameScale, SpriteEffects.None, 0);

			Main.spriteBatch.Draw(progressTexture, progressPosition + linePositionOffset, null, lineColor, 0, progressTexture.Size() / 2, lineScale, SpriteEffects.None, 0);
		}

		Main.spriteBatch.Draw(head_ball_texture, head_position, null, lightColor, head_rotation, head_ball_origin, Projectile.scale, head_effects, 0);
		Main.spriteBatch.Draw(head_rope_texture, head_position, null, lightColor, head_rotation, head_rope_origin, Projectile.scale, head_effects, 0);

		Effect shineEffect = ModAsset.EyeOfAnabiosis_Shine.Value;
		shineEffect.Parameters["uImageSize"].SetValue(head_texture.Size());
		shineEffect.Parameters["uChargeProgress"].SetValue(chargeProgress);
		shineEffect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects);
		shineEffect.CurrentTechnique.Passes["Ball_Pixel"].Apply();
		Main.spriteBatch.Draw(head_ballbg_texture, head_position, null, lightColor, head_rotation, head_ballbg_origin, Projectile.scale, head_effects, 0);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		Main.spriteBatch.Draw(head_fire_texture, head_container_position, frame, lightColor, head_container_rotation, head_fire_origin, Projectile.scale * fireProgress, head_effects, 0);
		Main.spriteBatch.Draw(head_container_texture, head_container_position, null, lightColor, head_container_rotation, head_container_origin, Projectile.scale, head_effects, 0);

		var lightPosition = head_container_position + Main.screenPosition - head_ball_texture.Size() * Projectile.scale / 4;
		Lighting.AddLight(lightPosition, new Vector3(51, 235, 202) * 0.005f * chargeProgress);
		return false;
	}
}