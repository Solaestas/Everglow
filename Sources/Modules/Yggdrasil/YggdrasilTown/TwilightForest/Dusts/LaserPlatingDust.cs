namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Dusts;

public class LaserPlatingDust : ModDust
{
	public override bool Update(Dust dust)
	{
		return base.Update(dust);
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(dust.color.R, dust.color.G, dust.color.B, 0);
	}
}