namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

public class WitherWoodTorchDust : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.velocity.Y -= 0.03f;
		Lighting.AddLight(dust.position, new Vector3(0.7f, 0.3f, 0.8f) * dust.scale);
		return true;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(1f, 1f, 1f, 0);
	}
}