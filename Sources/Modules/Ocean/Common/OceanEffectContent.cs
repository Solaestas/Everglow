namespace Everglow.Ocean.Common;

public class OceanEffectContent : ModSystem
{
	public static Effect DefaultEffectWave;
	public static Texture2D MainColorBlue;
	public static Texture2D MainShape;
	public static Texture2D MaskColor;
	public override void PostSetupContent()
	{
		DefaultEffectWave = ModContent.Request<Effect>("Everglow/Ocean/Effects/Wave").Value;
		MainColorBlue = ModContent.Request<Texture2D>("Everglow/Ocean/UIImages/heatmapBlue").Value;
		MainShape = ModContent.Request<Texture2D>("Everglow/Ocean/UIImages/lightline").Value;
		MaskColor = ModContent.Request<Texture2D>("Everglow/Ocean/UIImages/FogTrace").Value;
		// base.PostSetupContent();
	}
}