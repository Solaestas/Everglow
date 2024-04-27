using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CyanBullet : TrailingProjectile
{
	public override void SetDef()
	{	
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.hostile = true;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 6;
		Projectile.timeLeft = 3600;
		TrailTexture = Commons.ModAsset.Trail.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
		TrailColor = new Color(0.3f, 0.7f, 0.8f, 0f);
		TrailWidth = 6f;
	}
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.damage += 2;
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if(TimeTokill < 0)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.7f, 0.8f));
		}
		else
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.7f, 0.8f) * TimeTokill / 7f);
		}
		if(Projectile.timeLeft == 3540)
		{
			Projectile.damage -= 2;
		}
		base.AI();
	}
	public override void DrawSelf()
	{
		return;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
	}
	public override void KillMainStructure()
	{
		SoundEngine.PlaySound(SoundID.NPCHit4.WithVolumeScale(0.8f), Projectile.Center);
		for (int i = 0;i < 5;i++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2.0f, 24f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new Spark_MoonBladeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(3, 7),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(16f, 27.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				noGravity = true,
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) }
			};
			Ins.VFXManager.Add(spark);
		}
		base.KillMainStructure();
	}
}