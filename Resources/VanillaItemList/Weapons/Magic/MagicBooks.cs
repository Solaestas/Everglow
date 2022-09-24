using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Magic
{
    public class MagicBooks : GlobalItem
    {
        private static List<int> vanillaMagicBooks;
        public override void Unload()
        {
            vanillaMagicBooks = null;
        }

        public MagicBooks()
        {
            vanillaMagicBooks = new List<int>
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
