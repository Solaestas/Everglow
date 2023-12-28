namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee
{
	public class GlowMoonBlade : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;//友好
			Projectile.melee = true;//近战
			Projectile.ignoreWater = true;//不被水影响
			Projectile.tileCollide = false;//能穿墙，反义为false
			Projectile.timeLeft = 255;//存在时间，60是1秒
			Projectile.alpha = 10;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1f;//大小
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
		}
		float m = 0;
		public override void AI()
		{
			Projectile.rotation = (float)
					System.Math.Atan2(Projectile.velocity.Y,
			Projectile.velocity.X) + 1.57f;

			Player player = Main.player[Projectile.owner];

			if (Projectile.timeLeft == 254)
			{
				m = (Projectile.Center - player.Center).ToRotation();
			}

			if (Projectile.timeLeft <= 254)
			{


			}
			if(Collision.SolidCollision(Projectile.position + new Vector2(10), Projectile.width - 20, Projectile.height - 20))
			{
				Projectile.scale *= 0.85f;
				Projectile.velocity *= 0.1f;
				if(Projectile.scale < 0.01f)
				{
					Projectile.active = false;
				}
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			double range = Projectile.scale;
			double range2 = Projectile.scale;
	    	Texture2D tex = ModAsset.GlowMoonBlade.Value;
			Vector2 drawOrigin = tex.Size() * 0.5f;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				range2 *= 1.041;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - (int)((k + 3) * 1.5)) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(tex, drawPos, null, color, Projectile.rotation, drawOrigin, (float)range, SpriteEffects.None, 0f);

				Vector2 drawPos2 = Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color2 = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - (int)((k + 8) * 1.5)) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(tex, drawPos2, null, color2, Projectile.rotation, drawOrigin, (float)range2, SpriteEffects.None, 0f);

			}
			return true;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(24, 120);
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}