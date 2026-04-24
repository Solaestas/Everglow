namespace Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;

public class LanternGhostKingSkyEffect : ModSystem
{
	public float SkyPower = 0f;

	public bool Active = false;

	public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
	{
		if(Active)
		{
			Color colorShine = new Color(1f, 0.3f, 0.2f, 1f);
			if (SkyPower > 0)
			{
				SkyPower -= 0.05f;
				tileColor = Color.Lerp(tileColor, colorShine, SkyPower);
				backgroundColor = Color.Lerp(backgroundColor, colorShine, SkyPower);
				Main.ColorOfTheSkies = Color.Lerp(tileColor, colorShine, SkyPower);
			}
			else
			{
				SkyPower = 0;
				Active = false;
			}
		}
		base.ModifySunLightColor(ref tileColor, ref backgroundColor);
	}
}