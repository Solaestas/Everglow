using Terraria.Audio;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic
{
	public class FireFeather : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 1080;
			Projectile.penetrate = 1;
		}
		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			Projectile.velocity *= 1.01f;
			int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(12, 12) - Projectile.velocity, 16, 16, 6, 0f, 0f, 100, default, Main.rand.NextFloat(1f, 2.6f));
			Main.dust[num90].noGravity = true;
			Main.dust[num90].velocity *= 0.5f;
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
				Projectile.velocity.X = (Projectile.velocity.X * 200f + num9) / 201f;
				Projectile.velocity.Y = (Projectile.velocity.Y * 200f + num10) / 201f;
			}
			int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + Projectile.velocity / Projectile.velocity.Length() * 12f, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2>(), 0, 0, 0, default, 4f);
			Main.dust[r].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
			for (int j = 0; j < 15; j++)
			{
				for (int z = 0; z < 4; z++)
				{
					Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
					int dus = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, v2.X, v2.Y, 100, default, 1.8f);
					Main.dust[dus].noGravity = true;
					Main.dust[dus].velocity = v2;
				}
				Vector2 v1 = new Vector2(0, Main.rand.NextFloat(0, 16f)).RotatedByRandom(MathHelper.TwoPi);
				Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 3.6f)).RotatedByRandom(MathHelper.TwoPi);
				int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4) + v1, 0, 0, ModContent.DustType<LanternMoon.Dusts.Flame2>(), v3.X, v3.Y, 0, default, 6f);
				Main.dust[r].noGravity = true;
				Main.dust[r].velocity = v3;
			}
			for (int i = 0; i < 47; i++)
			{
				Vector2 v = new Vector2(0, Main.rand.NextFloat(2, 7)).RotatedByRandom(MathHelper.TwoPi);
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FartInAJar, v.X, v.Y, 150, default, Main.rand.NextFloat(0.8f, 2.1f));
			}
			for (int i = 0; i < 5; i++)
			{
				Gore.NewGore(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(0.7f, 2.2f)).RotatedByRandom(MathHelper.TwoPi), Main.rand.Next(61, 64), Main.rand.NextFloat(1f, 1.6f));
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(24, 600);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			int startY = frameHeight * Projectile.frame;
			var sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			float offsetX = 20f;
			origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
	}
}