namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class LamorchDust : ModDust
{
	public override void OnSpawn(Dust dust)
	{
	}

	public override bool Update(Dust dust)
	{
		Lighting.AddLight(dust.position, new Vector3(0.7f, 0.3f, 0.8f) * dust.scale);
		return true;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(1f, 1f, 1f, 0);
	}
}