using Terraria.ModLoader;

namespace Everglow.TwilightForest.Biomes
{
	public class TwilightForestUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots) 
		{
			string Path = "TwilightForest/Backgrounds/TwilightForestUnderground";
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "0");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "0");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "0");
		}
	}
}