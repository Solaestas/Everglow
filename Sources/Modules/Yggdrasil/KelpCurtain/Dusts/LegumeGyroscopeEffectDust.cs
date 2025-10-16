namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

public class LegumeGyroscopeEffectDust : ModDust
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

	public override Color? GetAlpha(Dust dust, Color lightColor) => base.GetAlpha(dust, lightColor);
}