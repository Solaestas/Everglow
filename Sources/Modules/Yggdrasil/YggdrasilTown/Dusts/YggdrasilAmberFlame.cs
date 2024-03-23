namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class YggdrasilAmberFlame : ModDust
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
		return true;
	}
}