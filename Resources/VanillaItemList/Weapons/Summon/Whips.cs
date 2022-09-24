using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Summon
{
    public class Whips : GlobalItem
    {
        private static List<int> vanillaWhips;
        public override void Unload()
        {
            vanillaWhips = null;
        }

        public Whips()
        {
            vanillaWhips = new List<int>
            {
                //水箭
                ItemID.WaterBolt,

                //骷髅头法术
                ItemID.BookofSkulls,

                //恶魔镰刀
                ItemID.DemonScythe,

                //咒焰
                ItemID.CursedFlames,

                //黄金尿
                ItemID.GoldenShower,

                //水晶风暴
                ItemID.CrystalStorm,

                //磁球
                ItemID.MagnetSphere,

                //利刃台风
                ItemID.RazorbladeTyphoon,

                //月耀
                ItemID.LunarFlareBook,
            };
        }
    }
}
