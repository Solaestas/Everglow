using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.Map;

namespace Everglow.Myth.Misc.Dusts;

public class FreezeFeather : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.noGravity = true;
		dust.noLight = true;
		dust.scale *= 1f;
		dust.rotation = Main.rand.NextFloat(6.283f);
	}
	public override bool Update(Dust dust)
	{
		dust.position += dust.velocity;
		dust.rotation += MathF.Sin((float)(Main.time * 0.3 + dust.color.R)) * 0.7f;
		dust.scale *= 0.97f;
		dust.velocity *= MathF.Pow(0.97f, dust.velocity.Length());
		if (dust.position.X <= 320 || dust.position.X >= Main.maxTilesX * 16 - 320)
		{
			dust.active = false;
		}
		if (dust.position.Y <= 320 || dust.position.Y >= Main.maxTilesY * 16 - 320)
		{
			dust.active = false;
		}
		if (Collision.SolidCollision(dust.position, 8, 8))
		{
			Vector2 v0 = dust.velocity;
			int t = 0;
			while (Collision.SolidCollision(dust.position + v0, 8, 8))
			{
				t++;
				v0 = v0.RotatedByRandom(6.283);
				if (t > 10)
				{
					v0 = dust.velocity * 0.9f;
					break;
				}
			}
			dust.velocity = v0;
			dust.scale *= 0.9f;
		}
		else
		{
			dust.velocity += new Vector2(Main.windSpeedCurrent * 0.25f, 0.02f * dust.scale * dust.scale);
		}
		if (dust.scale < 0.35f)
			dust.active = false;
		return false;
	}
	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		float light = (lightColor.R + lightColor.G + lightColor.B) / 765f;
		return Color.Lerp(lightColor, new Color(light, light, light, 1), 0.9f);
	}
}
