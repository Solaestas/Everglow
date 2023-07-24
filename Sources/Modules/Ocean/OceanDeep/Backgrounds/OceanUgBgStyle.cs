using Terraria;
using Terraria.ModLoader;

namespace MythMod.Backgrounds
{
	public class MythUgBgStyle : ModUgBgStyle
	{
		public override bool ChooseBgStyle()
		{
			return ((MythPlayer)Main.player[Main.myPlayer].GetModPlayer(mod, "MythPlayer")).ZoneOcean;
		}

        public override void FillTextureArray(int[] textureSlots)
		{
			textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/OceanUG0");
			textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/Blank");
			textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/OceanUG2");
			textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/OceanUG3");
		}
    }
}