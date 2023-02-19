using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.NPCList.EventNPCs
{
    public class PumpkinMoonNPCs : GlobalNPC
    {
        public static List<int> vanillaPumpkinMoonNPCs;
        public override void Unload()
        {
            vanillaPumpkinMoonNPCs = null;
        }

        public PumpkinMoonNPCs()
        {
            vanillaPumpkinMoonNPCs = new List<int>
            {
                305, 306, 307, 308, 309, 310, 311, 312, 313, 314,
                315, 325, 326, 327, 328, 329, 330
            };
        }
    }
}