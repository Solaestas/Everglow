namespace Everglow.Myth.Misc.Dusts.Slingshots;

public class JungleSpore : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.noGravity = true;
		dust.frame = new Rectangle(0, 0, 16, 16);
		dust.alpha = 0;
		dust.rotation = dust.scale * 0.3f;//用旋转角度存尺寸极值
	}

	public override bool Update(Dust dust)
	{
		dust.alpha += 5;
		dust.position += dust.velocity;
		dust.velocity += new Vector2(0, 0.015f).RotatedByRandom(MathHelper.Pi * 2d);
		dust.velocity *= 0.95f;
		dust.scale = (float)Math.Sin(dust.alpha / 255d * Math.PI) * dust.rotation;
		Lighting.AddLight(dust.position, 0.16f * dust.scale / 1.8f, 0.655f * dust.scale / 1.8f, 0f * dust.scale / 1.8f);
		if (Collision.SolidCollision(dust.position, 8, 8))
		{
			Vector2 v0 = dust.velocity;
			int T = 0;
			while (Collision.SolidCollision(dust.position + v0, 8, 8))
			{
				T++;
				v0 = v0.RotatedByRandom(6.283);
				if (T > 10)
				{
					v0 *= -1;
					break;
				}
			}
			dust.velocity = v0;
		}
		if (dust.alpha > 254)
			dust.active = false;

		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color?(new Color(255, 255, 255, 0f));
	}
}