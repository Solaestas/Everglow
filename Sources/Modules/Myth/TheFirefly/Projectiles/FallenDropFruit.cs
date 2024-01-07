using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.TheFirefly.VFXs;
using SteelSeries.GameSense;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class FallenDropFruit : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = true;
		Projectile.timeLeft = 6000;
		Projectile.tileCollide = true;
		Projectile.aiStyle = -1;
		Projectile.width = 50;
		Projectile.height = 70;
		Projectile.penetrate = -1;
	}
	public override void OnSpawn(IEntitySource source)
	{
		base.OnSpawn(source);
	}
	public override void AI()
	{
		Projectile.rotation = 0;
		Projectile.velocity.Y += 0.15f;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		if(!Projectile.tileCollide)
		{
			return false;
		}
		Texture2D glow = ModContent.Request<Texture2D>(Texture).Value;
		Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, glow.Size() / 2f, Projectile.scale, SpriteEffects.None);
		Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), Projectile.rotation, glow.Size() / 2f, Projectile.scale, SpriteEffects.None);
		return false;
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		GenerateSplash();
		Projectile.timeLeft = 10;
		Vector2 v0 = Projectile.Center;
		Projectile.width = 360;
		Projectile.height = 200;
		Projectile.Center = v0;
		Projectile.velocity *= 0;
		Projectile.tileCollide = false;
		return false;
	}
	public void GenerateSplash()
	{
		Vector2 spawnPoint = Projectile.Center + new Vector2(0, 32);
		while(Collision.SolidCollision(spawnPoint, 5, 5))
		{
			spawnPoint.Y -= 5;
			if(spawnPoint.Y < 320)
			{
				return;
			}
		}
		for (int g = 0; g < 100; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(-150f, -75f)).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
			float mulScale = Main.rand.NextFloat(6f, 14f);
			var blood = new FireflyBlueLiquidDrop
			{
				velocity = afterVelocity / mulScale,
				Active = true,
				Visible = true,
				position = spawnPoint + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(82, 164),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) }
			};
			Ins.VFXManager.Add(blood);
		}
		for (int g = 0; g < 90; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(-12f, -3f)).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
			var blood = new FireflyBlueLiquidSplash
			{
				velocity = afterVelocity,
				Active = true,
				Visible = true,
				position = spawnPoint + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - afterVelocity,
				maxTime = Main.rand.Next(42, 164),
				scale = Main.rand.NextFloat(6f, 24f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.4f), 0 }
			};
			Ins.VFXManager.Add(blood);
		}
		for (int g = 0; g < 50; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(-17f, -3f)).RotatedBy(Main.rand.NextFloat(-1.8f, 1.8f));
			var fire = new MothBlueFireDust
			{
				velocity = afterVelocity,
				Active = true,
				Visible = true,
				position = spawnPoint + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(19, 75),
				scale = Main.rand.NextFloat(8f, 15f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, 0 }
			};
			Ins.VFXManager.Add(fire);
		}
	}
	public override bool? CanCutTiles()
	{
		return false;
	}
}