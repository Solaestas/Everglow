using Terraria;
using Terraria.Localization;
namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

public class GoldRound : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("GoldRound");
			}
	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.penetrate = 1;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 100;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}
	private float Omega = 0.4f;
	public override void AI()
	{
		Projectile.velocity *= 0.98f;
		if (Projectile.timeLeft > 90)
		{
			Projectile.rotation += Omega;
			Projectile.alpha -= 5;
		}
		else
		{
			if (Projectile.timeLeft == 90)
			{
				int l = -1;
				for (int i = 0; i < 200; i++)
				{
					float Dist = (Main.npc[i].Center - Projectile.Center).Length();
					if (!Main.npc[i].dontTakeDamage && !Main.npc[i].friendly && Main.npc[i].active && Main.npc[i].CanBeChasedBy() && Dist < 600)
					{
						if (l == -1)
							l = i;
						else if ((Main.npc[i].Center - Projectile.Center).Length() < (Main.npc[l].Center - Projectile.Center).Length())
						{
							l = i;
						}
					}
				}
				if (l != -1)
				{
					Projectile.velocity += (Main.npc[l].Center - Projectile.Center) / (Main.npc[l].Center - Projectile.Center).Length() * 17f;
				}
				/*else
                    {
                        Projectile.velocity *= 0;
                    }*/
			}
			//Projectile.rotation += Omega;
			Projectile.velocity *= 0.975f;
			if (Projectile.timeLeft < 52)
			{
				Projectile.alpha += 5;
				Projectile.tileCollide = false;
			}
			Projectile.rotation += Omega;
		}
		if (Projectile.timeLeft < 52)
			Projectile.alpha += 5;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int i = 0; i < 15; i++)
		{
			Vector2 v = new Vector2(0, Main.rand.NextFloat(1.5f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			int num = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 87, 0f, 0f, 100, default, 1.2f);
			Main.dust[num].velocity *= v;
			Main.dust[num].noGravity = true;
		}
	}
	public override void OnKill(int timeLeft)
	{
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/Misc/Projectiles/Weapon/Melee/GoldRound").Value;
		int frameHeight = t.Height;
		var drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
		for (int k = 0; k < Projectile.oldPos.Length; k++)
		{
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
			var color = new Color(240, 240, 240, 0);
			if (Projectile.timeLeft < 60)
				color = new Color(Projectile.timeLeft * 4, Projectile.timeLeft * 4, Projectile.timeLeft * 4, 0);
			var color2 = new Color((int)(color.R * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.G * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.B * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.A * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
			Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation, drawOrigin, Projectile.scale * (Projectile.oldPos.Length - k) / Projectile.oldPos.Length, SpriteEffects.None, 0f);
		}
		return false;
	}
}
