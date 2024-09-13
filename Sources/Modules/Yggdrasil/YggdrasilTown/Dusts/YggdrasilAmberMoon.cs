namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class YggdrasilAmberMoon : ModDust
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
	}
	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		Color c = lightColor;
		c.A = 120;
		return c;
	}
	public override void OnSpawn(Dust dust)
	{
		dust.color.A = 255;
		base.OnSpawn(dust);
	}
	public override bool Update(Dust dust)
	{
		
		dust.color.A -= 15;
		dust.frame = new Rectangle(0, 0, 8, 10);
		if(dust.color.A < 200)
		{
			dust.frame.Y = 10;
		}
		if (dust.color.A < 100)
		{
			dust.frame.Y = 20;
		}
		if (dust.color.A < 15)
		{
			dust.active = false;
		}
		dust.rotation = MathF.Atan2(dust.velocity.Y ,dust.velocity.X);
		dust.position += dust.velocity;
		dust.velocity *= 0.9f;
		return false;
	}
}