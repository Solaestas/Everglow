namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

public class Husk : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.scale -= 0.1f;
		if (dust.scale < 0.05f)
		{
			dust.active = false;
		}
		return base.Update(dust);
	}
}