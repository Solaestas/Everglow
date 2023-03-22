using Terraria.ModLoader;

namespace Everglow.TwilightForest.Biomes
{
	public class TwilightForestUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots) 
		{
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestFar");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestFar");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestFar");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestFar");
		}
	}
}