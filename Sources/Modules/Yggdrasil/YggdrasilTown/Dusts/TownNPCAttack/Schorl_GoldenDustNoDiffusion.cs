namespace Everglow.Yggdrasil.YggdrasilTown.Dusts.TownNPCAttack;

public class Schorl_GoldenDustNoDiffusion : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.velocity *= 0.85f;
		dust.scale *= 0.92f;
		dust.position += dust.velocity;
		if (dust.scale < 0.1f)
		{
			dust.active = false;
		}
		Lighting.AddLight(dust.position + new Vector2(4), new Vector3(0.4f * dust.scale, 0.44f * dust.scale, 0));
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(1f, 1f, 1f, 0f);
	}
}