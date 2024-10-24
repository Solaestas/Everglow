namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class SquamousShellWingDust : ModDust
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		Color c = lightColor;
		c.A = 20;
		return c;
	}

	public override bool Update(Dust dust)
	{
		dust.velocity *= 0.95f;
		Lighting.AddLight(dust.position, new Vector3(0, 0.4f, 0.7f) * dust.scale);
		return true;
	}
}