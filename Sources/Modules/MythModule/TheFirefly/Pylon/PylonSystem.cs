using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Pylon
{
    internal class PylonSystem : ModSystem
    {
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Set("P_A_T", ModContent.GetInstance<FireflyPylon_TileEntity>().IsDestoryed);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.TryGet("P_A_T", out bool flag))
            {
                ModContent.GetInstance<FireflyPylon_TileEntity>().IsDestoryed = flag;
            }
            else
            {
                ModContent.GetInstance<FireflyPylon_TileEntity>().IsDestoryed = false;
            }
        }
    }

    internal class Global_Pylon : GlobalPylon
    {
        public override void PostValidTeleportCheck(TeleportPylonInfo destinationPylonInfo, TeleportPylonInfo nearbyPylonInfo, ref bool destinationPylonValid, ref bool validNearbyPylonFound, ref string errorKey)
        {
            if (destinationPylonInfo.ModPylon is FireflyPylon or ShabbyPylon)
            {
                errorKey = "";
            }
        }
    }
}