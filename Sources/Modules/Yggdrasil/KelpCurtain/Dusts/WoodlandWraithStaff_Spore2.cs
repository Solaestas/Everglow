namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

/// <summary>
/// Will not move different in velocity x and y.
/// </summary>
public class WoodlandWraithStaff_Spore2 : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.position += dust.velocity;
		dust.velocity *= 0.97f;
		dust.scale *= 0.98f;
		if(dust.scale < 0.05f)
		{
			dust.active = false;
		}
		return false;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		Color c0 = lightColor;
		c0.A *= 0;
		return c0;
	}
}