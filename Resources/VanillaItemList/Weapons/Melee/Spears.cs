using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Melee
{
    public class Spears : GlobalItem
    {
        private static List<int> vanillaSpears;
        public override void Unload()
        {
            vanillaSpears = null;
        }

        public Spears()
        {
            vanillaSpears = new List<int>
            {
                
            };
        }
    }
}
