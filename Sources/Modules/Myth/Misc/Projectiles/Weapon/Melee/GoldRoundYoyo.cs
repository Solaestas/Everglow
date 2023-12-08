using Everglow.Commons.Weapons.Yoyos;
using Everglow.Myth.Common;
using Terraria.GameContent;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee;

public class GoldRoundYoyo : YoyoProjectile
{
	public override void SetDef()
	{
		RotatedSpeed = 0.3f;
	}
	public override void AI()
	{
		base.AI();
	}
	public override void PostDraw(Color lightColor)
	{
		Texture2D texture = ModAsset.GoldRoundYoyoGlow.Value;
		Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, SpriteEffects.None, 0);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
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
		Color color = new Color(0, 0.03f, 0.05f, 1f);
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

				color = Color.Lerp(color, new Color(1f, 1f ,0, 0), 0.02f);
				Texture2D tex = TextureAssets.FishingLine.Value;
				Main.spriteBatch.Draw(tex, new Vector2(vector.X - Main.screenPosition.X + tex.Width * 0.5f, vector.Y - Main.screenPosition.Y + tex.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, tex.Width, (int)num5)), color, rotation, new Vector2(tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
