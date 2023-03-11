using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.ItemList.Weapons.Summon
{
    public class Whips : GlobalItem
    {
        public static List<int> vanillaWhips;
        public override void Unload()
        {
            vanillaWhips = null;
        }

        public Whips()
        {
            vanillaWhips = new List<int>
            {
                //皮鞭
                ItemID.BlandWhip,

                //荆鞭
                ItemID.ThornWhip,

                //骨鞭
                ItemID.BoneWhip,
                
                //鞭炮
                ItemID.FireWhip,

                //冷鞭
                ItemID.CoolWhip,

                //郎迪达尔
                ItemID.SwordWhip,

                //黑暗收割者
                ItemID.ScytheWhip,

                //晨星
                ItemID.MaceWhip,

                //万花筒
                ItemID.RainbowWhip,

            };
        }
    }
}
