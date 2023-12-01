using Everglow.Commons.Weapons.Yoyos;
using Everglow.Food.Dusts;
using Terraria.GameContent;

namespace Everglow.Food.Projectiles;

public class CheeseYoyo_proj : YoyoProjectile
{
	public override void SetDef()
	{
		RotatedSpeed = 0.3f;
	}
	public override void AI()
	{
		base.AI();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for(int i = 0;i < 6;i++)
		{
			Dust.NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<CheeseDust>());
		}
		base.OnHitNPC(target, hit, damageDone);
	}
	public override void DrawString(Vector2 to = default)
	{
		Player player = Main.player[Projectile.owner];
		Vector2 mountedCenter = player.MountedCenter;
		Vector2 vector = mountedCenter;
		vector.Y += player.gfxOffY;
		if (to != default)
			vector = to;
		float num = Projectile.Center.X - vector.X;
		float num2 = Projectile.Center.Y - vector.Y;
		Math.Sqrt((double)(num * num + num2 * num2));
		float rotation;
		if (!Projectile.counterweight)
		{
			int num3 = -1;
			if (Projectile.position.X + Projectile.width / 2 < player.position.X + player.width / 2)
				num3 = 1;
			num3 *= -1;
			player.itemRotation = (float)Math.Atan2((double)(num2 * num3), (double)(num * num3));
		}
		bool checkSelf = true;
		if (num == 0f && num2 == 0f)
			checkSelf = false;
		else
		{
			float num4 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			num4 = 12f / num4;
			num *= num4;
			num2 *= num4;
			vector.X -= num * 0.1f;
			vector.Y -= num2 * 0.1f;
			num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
			num2 = Projectile.position.Y + Projectile.height * 0.5f - vector.Y;
		}
		while (checkSelf)
		{
			float num5 = 12f;
			float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			float num7 = num6;
			if (float.IsNaN(num6) || float.IsNaN(num7))
				checkSelf = false;
			else
			{
				if (num6 < 20f)
				{
					num5 = num6 - 8f;
					checkSelf = false;
				}
				num6 = 12f / num6;
				num *= num6;
				num2 *= num6;
				vector.X += num;
				vector.Y += num2;
				num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
				num2 = Projectile.position.Y + Projectile.height * 0.1f - vector.Y;
				if (num7 > 12f)
				{
					float num8 = 0.3f;
					float num9 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
					if (num9 > 16f)
						num9 = 16f;
					num9 = 1f - num9 / 16f;
					num8 *= num9;
					num9 = num7 / 80f;
					if (num9 > 1f)
						num9 = 1f;
					num8 *= num9;
					if (num8 < 0f)
						num8 = 0f;
					num8 *= num9;
					num8 *= 0.5f;
					if (num2 > 0f)
					{
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
					else
					{
						num9 = Math.Abs(Projectile.velocity.X) / 3f;
						if (num9 > 1f)
							num9 = 1f;
						num9 -= 0.5f;
						num8 *= num9;
						if (num8 > 0f)
							num8 *= 2f;
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
				}
				rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
				int stringColor = player.stringColor;
				Color color = WorldGen.paintColor(stringColor);
				if (color.R < 75)
					color.R = 75;
				if (color.G < 75)
					color.G = 75;
				if (color.B < 75)
					color.B = 75;
				if (stringColor == 13)
					color = new Color(20, 20, 20);
				else if (stringColor == 14 || stringColor == 0)
				{
					color = new Color(200, 200, 200);
				}
				else if (stringColor == 28)
				{
					color = new Color(163, 116, 91);
				}
				else if (stringColor == 27)
				{
					color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
				color.A = (byte)(color.A * 0.4f);

				color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
				color = new Color((byte)(color.R * 1f), (byte)(color.G * 0.9f), (byte)(color.B * 0.2f), (byte)(color.A * 0.5f));
				Texture2D tex = TextureAssets.FishingLine.Value;
				Main.spriteBatch.Draw(tex, new Vector2(vector.X - Main.screenPosition.X + tex.Width * 0.5f, vector.Y - Main.screenPosition.Y + tex.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, tex.Width, (int)num5)), color, rotation, new Vector2(tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
