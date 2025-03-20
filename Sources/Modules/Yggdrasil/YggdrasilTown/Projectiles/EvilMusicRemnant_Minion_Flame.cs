using Everglow.Yggdrasil.YggdrasilTown.VFXs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Minion_Flame : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 30;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
		Projectile.ignoreWater = true;
		Projectile.friendly = true;
	}

	public override void AI()
	{
		for (int i = 0; i < 2; i++)
		{
			float size = Main.rand.NextFloat(0.1f, 0.96f);
			var noteFlame = new EvilMusicRemnant_FlameDust
			{
				Velocity = new Vector2(0, Main.rand.NextFloat(0.3f, 1f)).RotatedByRandom(MathHelper.TwoPi) * 1.2f + Projectile.velocity * 0.2f,
				Active = true,
				Visible = true,
				Position = Projectile.Center,
				MaxTime = Main.rand.Next(24, 36),
				Scale = 14f * size,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Frame = Main.rand.Next(3),
				ai = [Main.rand.NextFloat(-0.8f, 0.8f)],
			};
			Ins.VFXManager.Add(noteFlame);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}
}