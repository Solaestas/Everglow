using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.NPCList.EventNPCs
{
    public class FrostMoonNPCs : GlobalNPC
    {
        public static List<int> vanillaFrostMoonNPCs;
        public override void Unload()
        {
            vanillaFrostMoonNPCs = null;
        }

        public FrostMoonNPCs()
        {
            vanillaFrostMoonNPCs = new List<int>
            {
                338, 339, 340, 341, 342, 343, 344, 345, 346, 347,
                348, 349, 350, 351, 352
            };
        }
    }
}
