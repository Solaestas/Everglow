using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic;

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
	private bool initialization = true;
	private bool stick = false;
	private int u = 0;
	private NPC m = Main.npc[0];
	private Vector2 v = new Vector2(0, 0);
	private int r = 0;
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
		if (stick && m.active)
		{
			r += 1;
			float yz = m.Hitbox.Width * m.Hitbox.Width + m.Hitbox.Height * m.Hitbox.Height;
			yz = (float)Math.Sqrt(yz) / 3f;
			Projectile.position = m.Center - v * yz / v.Length();
			if (r % 4 == 0)
			{
				int Dam = (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f) / 4d);
				if (Dam < 1)
					Dam = 1;
				m.StrikeNPC(Dam, Projectile.knockBack, Projectile.direction, Main.rand.Next(100) < Projectile.ai[0]);
				Player p = Main.LocalPlayer;
				p.dpsDamage += (int)(Dam * (100 + Projectile.ai[0]) / 100d);
				Projectile.penetrate--;
			}
		}
		if (stick && !m.active)
			Projectile.active = false;
		float num2 = Projectile.Center.X;
		float num3 = Projectile.Center.Y;
		float num4 = 400f;
		bool flag = false;
		for (int j = 0; j < 200; j++)
		{
			if (Main.npc[j].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
			{
				float num5 = Main.npc[j].position.X + Main.npc[j].width / 2;
				float num6 = Main.npc[j].position.Y + Main.npc[j].height / 2;
				float num7 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num5) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num6);
				if (num7 < num4)
				{
					num4 = num7;
					num2 = num5;
					num3 = num6;
					flag = true;
				}
			}
		}
		if (flag)
		{
			float num8 = 20f;
			var vector1 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
			float num9 = num2 - vector1.X;
			float num10 = num3 - vector1.Y;
			float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
			num11 = num8 / num11;
			num9 *= num11;
			num10 *= num11;
			Projectile.velocity.X = (Projectile.velocity.X * 90f + num9) / 91f;
			Projectile.velocity.Y = (Projectile.velocity.Y * 90f + num10) / 91f;
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
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
		stick = true;
		v = Projectile.position - target.position;
		Projectile.friendly = false;
		m = target;
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