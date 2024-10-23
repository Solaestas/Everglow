namespace Everglow.Yggdrasil.Common.Dusts;

public class YggdrasilWorldPylonDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.noGravity = true;
		dust.noLight = false;
		dust.scale *= 0.5f;
		dust.alpha = 0;
		dust.velocity.Y = -1f;
		dust.frame.Y = Main.rand.Next(3) * 9;
	}

	public override bool Update(Dust dust)
	{
		Dust dust2 = dust;
		dust.position += dust.velocity;
		dust2.rotation += 0.1f * dust.scale;
		if (dust.color.A > 30)
		{
			dust.color.A -= 10;
			dust.scale += 0.05f;
		}
		dust.scale *= 0.98f;
		if (dust.scale < 0.05f)
		{
			dust.active = false;
		}

		if(dust.frame.Y <= 10)
		{
			Lighting.AddLight(dust.position, new Vector3(0.5f, 0.4f, 0.35f) * dust.scale);
		}
		else
		{
			Lighting.AddLight(dust.position, new Vector3(0.15f, 0.32f, 0.55f) * dust.scale);
		}
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		if (dust.frame.Y <= 10)
		{
			return new Color(0.5f, 0.4f, 0.35f, 0.5f);
		}
		return new Color(0.15f, 0.32f, 0.55f, 0.5f);
	}
}