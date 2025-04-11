using Everglow.Yggdrasil.YggdrasilTown.Dusts.TownNPCAttack;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.TownNPCAttack;

public class Schorl_Explosion : ModProjectile
{
	public int Timer;

	public override void SetDefaults()
	{
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 60;
		Projectile.ArmorPenetration = 0;
		Projectile.friendly = true;
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		for (int i = 0; i < 30; i++)
		{
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Schorl_GoldenDust>());
			dust.position = Projectile.Center;
			dust.velocity = new Vector2(0, MathF.Sin(i / 30f * MathHelper.TwoPi * 5) + 3).RotatedBy(Projectile.velocity.ToRotationSafe() + i / 30f * MathHelper.TwoPi) * 0.6f;
		}
		for (int t = 0; t < 3; t++)
		{
			for (int i = 0; i < 80; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<Schorl_GoldenDustNoDiffusion>());
				dust.position = Projectile.Center;
				var dir = new Vector2(0, 6).RotatedBy(i / 80f * MathHelper.TwoPi);
				dir.Y *= 0.5f + 0.35f * MathF.Sin(t * 2 + Projectile.whoAmI * 0.7f);
				dir = dir.RotatedBy(t * 2 + Projectile.whoAmI);
				dust.velocity = dir;
			}
		}
		for (int i = 0; i < 30; i++)
		{
			var sFlame = new Schorl_Flame
			{
				Velocity = new Vector2(0, Main.rand.NextFloat(0.3f, 2f)).RotatedByRandom(MathHelper.TwoPi),
				Active = true,
				Visible = true,
				Position = Projectile.Center + new Vector2(0, MathF.Sqrt(Main.rand.NextFloat()) * 32).RotatedByRandom(MathHelper.TwoPi),
				MaxTime = Main.rand.Next(24, 36),
				Scale = Main.rand.Next(14, 26),
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Frame = Main.rand.Next(3),
				ai = new float[] { Main.rand.NextFloat(-0.8f, 0.8f) },
			};
			Ins.VFXManager.Add(sFlame);
		}
	}

	public override void AI()
	{
	}

	public override bool ShouldUpdatePosition() => false;

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return base.Colliding(projHitbox, targetHitbox);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
}