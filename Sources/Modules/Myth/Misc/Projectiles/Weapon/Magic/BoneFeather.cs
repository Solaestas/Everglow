using Terraria;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Magic;

public class BoneFeather : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("BoneFeather");
			}
	public override void SetDefaults()
	{
		Projectile.width = 34;
		Projectile.height = 34;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360;
		Projectile.alpha = 0;
		Projectile.penetrate = 3;
		Projectile.scale = 1;
	}
	private float r2 = 120;
	public override void AI()
	{
		Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		Projectile.velocity *= 1.01f;
		if (Main.rand.NextBool(6))
		{
			/*int num90 = */
			var d = Dust.NewDustDirect(Projectile.Center - new Vector2(2, 2), 0, 0, ModContent.DustType<Dusts.Bones>(), Alpha: 0, Scale: Main.rand.NextFloat(0.3f, 1.1f));
			//int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(12, 12) - Projectile.velocity, 16, 16, 4, 0f, 0f, 100, default(Color), 1.2f);
			d.noGravity = true;
			d.velocity *= 0.25f;
		}
		if (r2 > 0)
		{
			var d = Dust.NewDustDirect(Projectile.Center - new Vector2(2, 2), 0, 0, ModContent.DustType<Dusts.BoneFlame>(), Alpha: 0, Scale: 2.5f * r2 / 90f);
			d.noGravity = true;
			d.velocity *= 0f;
			r2 -= 2;
		}
		if (Projectile.timeLeft % 2 == 0)
		{
			int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<Dusts.Bones2>(), 0, 0, 0, default, 1f);
			Main.dust[r].noGravity = true;
			Main.dust[r].velocity = Projectile.velocity;
			Main.dust[r].rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 0.95f;
			int r2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<Dusts.Bones2>(), 0, 0, 0, default, 1f);
			Main.dust[r2].noGravity = true;
			Main.dust[r2].velocity = Projectile.velocity;
			Main.dust[r2].rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 0.95f;
		}
		
		if (Projectile.position.X <= 320 || Projectile.position.X >= Main.maxTilesX * 16 - 320)
		{
			Projectile.Kill();
		}
		if (Projectile.position.Y <= 320 || Projectile.position.Y >= Main.maxTilesY * 16 - 320)
		{
			Projectile.Kill();
		}
	}
	public override void Kill(int timeLeft)
	{
		SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, Projectile.Center);
		for (int j = 0; j < 15; j++)
		{
			/*int num2 = */
			var d = Dust.NewDustDirect(Projectile.Center - new Vector2(2, 2), 0, 0, ModContent.DustType<Dusts.Bones>(), Alpha: 0, Scale: Main.rand.NextFloat(0.3f, 1.1f));
			d.noGravity = true;
			if (r2 > 0)
			{
				Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
				var d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(2, 2) + v0, 0, 0, ModContent.DustType<Dusts.BoneFlame>(), Alpha: 0, Scale: 5f * r2 / 90f);
				d2.noGravity = true;
				//d.velocity *= 0f;
				r2 -= 2;
			}
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Projectile.friendly = false;
		target.defense = (int)(target.defense * 0.96f);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		// SpriteEffects helps to flip texture horizontally and vertically
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (Projectile.spriteDirection == -1)
			spriteEffects = SpriteEffects.FlipHorizontally;

		// Getting texture of projectile
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		// Calculating frameHeight and current Y pos dependence of frame
		// If texture without animation frameHeight = texture.Height is always and startY is always 0
		int frameHeight = texture.Height / Main.projFrames[Projectile.type];
		int startY = frameHeight * Projectile.frame;

		// Get this frame on texture
		var sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
		Vector2 origin = sourceRectangle.Size() / 2f;

		// If image isn't centered or symetrical you can specify origin of the sprite
		// (0,0) for the upper-left corner 
		float offsetX = 20f;
		origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

		// If sprite is vertical
		// float offsetY = 20f;
		// origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


		// Appling lighting and draw current frame
		Color drawColor = Projectile.GetAlpha(lightColor);
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
		// It's important to return false, otherwise we also draw the original texture.
		return false;
	}
}