﻿namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.NPCs
{
    public class YggdrasilTownNPC : GlobalNPC
    {
        private static HashSet<int> yggdrasilTownNPCTypes = new HashSet<int>();
        public static void RegisterMothLandNPC(int type) => yggdrasilTownNPCTypes.Add(type);
    }
}
