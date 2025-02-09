using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class IstafelsSunfireGrasp_FireBall : ModProjectile
{
	public override string Texture => Commons.ModAsset.Point_Mod;

	public const int ProjectileVelocity = 8;
	public const int ProjectileVelocityYMax = 16;
	public const int BuffDuration = 960;
	public const int ContactDamage = 25;
	public const int TimeLeftMax = 600;
	private const float InitialScale = 0.2f;

	public bool Actived { get; set; } = false;

	public float BuildUpProgress { get; set; } = 0f;

	public override void SetDefaults()
	{
		Projectile.width = 40;
		Projectile.height = 40;

		Projectile.DamageType = DamageClass.Default;

		Projectile.friendly = false;
		Projectile.hostile = false;

		Projectile.timeLeft = TimeLeftMax;

		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;

		Projectile.localNPCHitCooldown = 10;
		Projectile.usesLocalNPCImmunity = true;
	}

	public override bool? CanCutTiles() => true;

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
		ShootScoria();
		return true;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		// Let the projectile deals true damage, minus base damage 1
		modifiers.FinalDamage.Flat += ContactDamage - 1;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<Charred>(), BuffDuration);
		ShootScoria();
	}

	private void ShootScoria()
	{
		// Shoot scoria on kill
		var scoriaProjCount = Main.rand.Next(5, 7);
		for (int i = 0; i < scoriaProjCount; i++)
		{
			var velocity = Projectile.velocity * 0.6f + Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 4f;
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<IstafelsSunfireGrasp_Scoria>(), 1, 1.1f, Projectile.owner);
		}
	}

	public override void AI()
	{
		Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0f);

		if (Actived)
		{
			// Simulate gravity
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > ProjectileVelocityYMax)
			{
				Projectile.velocity.Y = ProjectileVelocityYMax;
			}
		}
		else
		{
			// Keep projectile active
			Projectile.timeLeft = TimeLeftMax;

			// Following player
			var owner = Main.player[Projectile.owner];
			var destination = owner.Center + new Vector2(-30 * owner.direction, -50 * owner.gravDir);
			var movement = destination - Projectile.Center;
			if (movement.Length() < ProjectileVelocity)
			{
				Projectile.velocity = movement;
			}
			else
			{
				Projectile.velocity = movement.NormalizeSafe() * ProjectileVelocity;
			}

			// Update life time
			if (owner.HasBuff<IstafelsSunfireGraspFireBallBuff>())
			{
				owner.AddBuff(ModContent.BuffType<IstafelsSunfireGraspFireBallBuff>(), 2);
			}
			else
			{
				Projectile.Kill();
			}
		}
	}

	public void Active(NPC target)
	{
		// Shoot projectile towards the target
		Projectile.velocity = (target.Center - Main.player[Projectile.owner].Center).NormalizeSafe() * ProjectileVelocity;

		// Active the projectile
		Actived = true;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		var effect = ModAsset.IstafelsSunfireGrasp_FireBall.Value;
		effect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * 0.05f);
		effect.CurrentTechnique.Passes[0].Apply();

		var texture = Commons.ModAsset.Point.Value;
		var drawColor = new Color(1f, 1f, 1f, 0f);
		var drawScale = 0.3f * MathHelper.Lerp(InitialScale, 1f, BuildUpProgress);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, texture.Size() / 2, drawScale, SpriteEffects.None, 0);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		return false;
	}

	public override void OnKill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);

		// Draw dusts on kill
		for (int i = 0; i < 40; i++)
		{
			var offset = new Vector2(Main.rand.NextFloat(16), 0).RotatedBy(Main.rand.NextFloat() * MathHelper.TwoPi);
			Dust.NewDust(Projectile.Center + offset, 4, 4, DustID.LavaMoss, Scale: 1.4f);
		}
		for (int i = 0; i < 20; i++)
		{
			Dust.NewDust(Projectile.Center, Projectile.width / 2, Projectile.height / 2, DustID.Torch, Scale: 1.2f);
		}
	}
}