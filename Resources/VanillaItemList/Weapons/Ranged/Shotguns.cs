using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Ranged
{
    public class Shotguns : GlobalItem
    {
        private static List<int> vanillaShotguns;
        public override void Unload()
        {
            vanillaShotguns = null;
        }

        public Shotguns()
        {
            vanillaShotguns = new List<int>
            {
                //三发猎枪
                ItemID.Boomstick,

                //四管霰弹枪
                ItemID.QuadBarrelShotgun,

                //霰弹枪
                ItemID.Shotgun,

                //玛瑙爆破枪
                ItemID.OnyxBlaster,

                //战术霰弹枪
                ItemID.TacticalShotgun,

                //外星霰弹枪
                ItemID.Xenopopper,
            };
        }
    }
}
