using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.NPCList.EventNPCs
{
    public class BloodMoonNPCs : GlobalNPC
    {
        public static List<int> vanillaBloodMoonNPCs;
        public override void Unload()
        {
            vanillaBloodMoonNPCs = null;
        }

        public BloodMoonNPCs()
        {
            vanillaBloodMoonNPCs = new List<int>
            {
                47,52, 53, 57, 109,168,186, 187, 188, 189, 190, 191, 192,
                193, 194, 316, 317, 318, 319, 320,
                321, 322, 323, 324, 331, 332,378, 430, 431, 432,332,
                434, 435, 436, 464, 465,470, 489,
                490,587,590,591,618,619,620,621,622,623
            };
        }
    }
}

