namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class TopazSpearDust : ModDust
{
	public override bool Update(Dust dust)
	{
		Lighting.AddLight(dust.position + new Vector2(4), new Vector3(0.7f * dust.scale, 0.7f * dust.scale, 0));
		return base.Update(dust);
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return lightColor * 0.5f;
	}
}