using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Backgrounds
{
	public class MythUgBgStyle : ModUndergroundBackgroundStyle
	{
		//public override bool ChooseBgStyle()/* tModPorter Note: Removed. Create a ModBiome (or ModSceneEffect) class and override UndergroundBackgroundStyle property to return this object through Mod/ModContent.Find, then move this code into IsBiomeActive (or IsSceneEffectActive) */
		//{
		//	return ((OceanContentPlayer)Main.player[Main.myPlayer].GetModPlayer(Mod, "OceanContentPlayer")).ZoneOcean;
		//}

        public override void FillTextureArray(int[] textureSlots)
		{
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/OceanUG0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/Blank");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/OceanUG2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/OceanUG3");
		}
    }
}