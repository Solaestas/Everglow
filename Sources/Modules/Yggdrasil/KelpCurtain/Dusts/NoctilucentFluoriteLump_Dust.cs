namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

public class NoctilucentFluoriteLump_Dust : ModDust
{
	public override bool Update(Dust dust)
	{
		Lighting.AddLight(dust.position, new Vector3(186, 242, 244) / 255f * dust.scale);
		return base.Update(dust);
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(1f, 1f, 1f, 0);
	}
}