namespace Everglow.Commons.VFX.CommonVFXDusts.Misc;

public class CrystalScaleFlame_VanillaDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{
		dust.noGravity = true;
		dust.frame = new Rectangle(0, 0, 9, 9);
		dust.dustIndex = (int)(dust.scale * 300); // 用Index存尺寸极值
	}

	public override bool Update(Dust dust)
	{
		dust.alpha += 14;
		dust.position += dust.velocity;
		dust.velocity *= 0.95f;
		dust.rotation = MathF.Atan2(dust.velocity.Y, dust.velocity.X);
		float mulLight = 1 - dust.alpha / 255f;
		Lighting.AddLight(dust.position, new Vector3(0.5f, 0.3f, 0.6f) * mulLight);
		if (dust.alpha > 254)
		{
			dust.active = false;
		}
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		float timeValue = dust.alpha / 255f;
		timeValue -= 0.5f;
		timeValue *= 2f;
		float r = 1 - MathF.Pow(MathF.Abs(MathF.Sin(timeValue * MathHelper.Pi / 2f)), 0.3f);
		r *= 0.4f;
		float g = 1 - MathF.Pow(MathF.Abs(MathF.Sin(timeValue * MathHelper.Pi / 2f)), 0.5f);
		g *= 0.2f;
		float b = 1 - MathF.Pow(MathF.Max(MathF.Abs(timeValue) * 2f - 1, 0), 3.5f);
		b *= 0.15f;
		float rot = dust.color.G / 255f * 6;
		if (rot > 2 && rot < 4)
		{
			float rotValue = rot - 3;
			r *= (1 - MathF.Pow(MathF.Abs(MathF.Sin(rotValue * MathHelper.Pi / 2f)), 0.5f)) * 10;
			g *= (1 - MathF.Pow(MathF.Abs(MathF.Sin(rotValue * MathHelper.Pi / 2f)), 0.5f)) * 10;
			b *= (1 - MathF.Pow(MathF.Abs(MathF.Sin(rotValue * MathHelper.Pi / 2f)), 0.5f)) * 10;
		}
		return new Color?(new Color(r, g, b, 0.12f));
	}
}