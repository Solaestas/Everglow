namespace Everglow.Food.Dusts;

public class PotatoDust : ModDust
{
	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		Color color = lightColor;
		color *= 0.75f;
		return color;
	}
}
