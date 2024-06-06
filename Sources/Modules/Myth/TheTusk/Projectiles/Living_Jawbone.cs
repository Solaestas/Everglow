using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles;

public class Living_Jawbone : ModProjectile
{
	public bool OpenMouth = false;

	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 500;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		for (int g = 0; g < 4; g++)
		{
			var blood = new BloodDrop
			{
				velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.4f, 1.1f),
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(54, 74),
				scale = Main.rand.NextFloat(6f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
		for (int g = 0; g < 2; g++)
		{
			var blood = new BloodSplash
			{
				velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.4f, 1.1f),
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(54, 74),
				scale = Main.rand.NextFloat(6f, 18f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, Main.rand.NextFloat(20.0f, 40.0f) },
			};
			Ins.VFXManager.Add(blood);
		}
	}

	public override void AI()
	{
		Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.velocity.Y += 0.2f;
		if (!OpenMouth)
		{
			Projectile.ai[0] = 0;
			if (Main.rand.NextBool(24))
			{
				OpenMouth = true;
			}
		}
		else
		{
			Projectile.ai[0] = (float)Utils.Lerp(Projectile.ai[0], 2, 0.06f);
			if (Projectile.ai[0] > 1.8f)
			{
				SoundEngine.PlaySound(SoundID.DD2_SkeletonDeath.WithVolume(0.4f * Projectile.scale), Projectile.Center);
				Projectile.ai[0] = 0;
				OpenMouth = false;
			}
		}
		if(Projectile.timeLeft < 470 && Projectile.timeLeft > 30)
		{
			if(Collision.SolidCollision(Projectile.Center, 0, 0))
			{
				Projectile.timeLeft = 10;
				Projectile.velocity *= 0.6f;
			}
		}
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float k = 0;
		bool b0 = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(64 * Projectile.scale, 0).RotatedBy(Projectile.rotation + Projectile.ai[0]), 14, ref k);
		bool b1 = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(64 * Projectile.scale, 0).RotatedBy(Projectile.rotation - Projectile.ai[0]), 14, ref k);
		return b0 || b1;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		Projectile.hide = true;
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D upJaw = ModAsset.Living_Jawbone_up.Value;
		Texture2D downJaw = ModAsset.Living_Jawbone_down.Value;
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
		{
			spriteEffects = SpriteEffects.FlipVertically;
		}
		Main.EntitySpriteDraw(upJaw, Projectile.Center - Main.screenPosition, null, Lighting.GetColor(Projectile.Center.ToTileCoordinates()), Projectile.rotation + Projectile.ai[0], new Vector2(8f, 18f), Projectile.scale, spriteEffects, 0);
		Main.EntitySpriteDraw(downJaw, Projectile.Center - Main.screenPosition, null, Lighting.GetColor(Projectile.Center.ToTileCoordinates()), Projectile.rotation - Projectile.ai[0], new Vector2(8f, 18f), Projectile.scale, spriteEffects, 0);
		return false;
	}
}