namespace Everglow.Yggdrasil.KelpCurtain.Dusts;

public class WoodlandWraithStaff_Spore : ModDust
{
	public override bool Update(Dust dust)
	{
		dust.position += dust.velocity;
		dust.velocity.X *= 1.02f;
		dust.velocity.Y -= 0.005f;
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