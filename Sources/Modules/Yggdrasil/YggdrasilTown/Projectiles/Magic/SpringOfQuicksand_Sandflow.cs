using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;

public class SpringOfQuicksand_Sandflow : ModProjectile
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicProjectiles;

	public override void SetDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 120;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = 8;
		Projectile.ignoreWater = true;
		Projectile.friendly = true;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
	}

	public override void OnSpawn(IEntitySource source) => base.OnSpawn(source);

	public override void AI()
	{
		float mulTime = 1f;
		if(Projectile.timeLeft < 60f)
		{
			mulTime = Projectile.timeLeft / 60f;
		}
		var flow = new SpringOfQuicksand_SandTrail
		{
			Velocity = Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.85f, 1.15f),
			Active = true,
			Visible = true,
			Position = Projectile.Center,
			MaxTime = Main.rand.NextFloat(75f, 100f) * mulTime,
			Scale = Main.rand.NextFloat(16f, 24f),
			Fade = 0,
		};
		Ins.VFXManager.Add(flow);
		for (int k = 0; k < 15; k++)
		{
			var dust = new SpringOfQuicksand_Dust
			{
				Velocity = Projectile.velocity.RotatedByRandom(0.05f) * Main.rand.NextFloat(0.55f, 0.82f),
				Active = true,
				Visible = true,
				Position = Projectile.oldPos[Main.rand.Next(Projectile.oldPos.Count())] + new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 24).RotatedByRandom(MathHelper.TwoPi),
				MaxTime = Main.rand.NextFloat(45f, 85f) * mulTime,
				Scale = Main.rand.NextFloat(0.1f, 0.5f),
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				RanSeed = Main.rand.NextFloat(MathHelper.TwoPi),
			};
			Ins.VFXManager.Add(dust);
		}
		Projectile.velocity.Y += 0.15f;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		for (int k = 0; k < Projectile.oldPos.Length; k++)
		{
			float fall = k * k * 0.5f;
			fall = Math.Min(fall, 150);
			if(targetHitbox.Intersects(new Rectangle((int)Projectile.oldPos[k].X, (int)Projectile.oldPos[k].Y, Projectile.width, (int)fall)))
			{
				return true;
			}
		}
		return base.Colliding(projHitbox, targetHitbox);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
}