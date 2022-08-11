using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Everglow.Sources.Modules.MythModule.TheTusk
{
    // Acts as a container for "downed boss" flags.
    // Set a flag like this in your bosses OnKill hook:
    //    NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);

    // Saving and loading these flags requires TagCompounds, a guide exists on the wiki: https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound
    public class DownedBossSystem : ModSystem
    {
        public static bool downedTusk = false;
        public static bool downedMoth = false;
        public static string TuskName;
        public static string MothName;
        // public static bool downedOtherBoss = false;
        public override void OnWorldLoad()
        {
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                TuskName = "鲜血獠牙";
            }
            else
            {
                TuskName = "Bloody Tusk";
            }
            if (Language.ActiveCulture.Name == "zh-Hans")
            {
                MothName = "腐檀巨蛾";
            }
            else
            {
                MothName = "Corrupt Moth";
            }
            downedTusk = false;
            downedMoth = false;
            // downedOtherBoss = false;
        }

        public override void OnWorldUnload()
        {
            downedTusk = false;
            downedMoth = false;
            // downedOtherBoss = false;
        }

        // We save our data sets using TagCompounds.
        // NOTE: The tag instance provided here is always empty by default.
        public override void LoadWorldData(TagCompound tag)
        {
            downedTusk = tag.ContainsKey("downedTusk");
            downedMoth = tag.ContainsKey("downedMoth");
            // downedOtherBoss = tag.ContainsKey("downedOtherBoss");
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedTusk)
            {
                tag["downedTusk"] = true;
            }
            if (downedMoth)
            {
                tag["downedMoth"] = true;
            }

            // if (downedOtherBoss) {
            //	tag["downedOtherBoss"] = true;
            // }
        }
    }
}
