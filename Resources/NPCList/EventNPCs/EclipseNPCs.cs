using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.NPCList.EventNPCs
{
    public class EclipseNPCs : GlobalNPC
    {
        public static List<int> vanillaEclipseNPCs;
        public override void Unload()
        {
            vanillaEclipseNPCs = null;
        }

        public EclipseNPCs()
        {
            vanillaEclipseNPCs = new List<int>
            {
                251, 253, 162, 166, 159, 158, 460, 461, 462, 463,
                466, 467, 468, 469, 477, 478, 479
            };
        }
    }
}

