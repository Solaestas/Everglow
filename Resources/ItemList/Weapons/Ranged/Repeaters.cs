using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.ItemList.Weapons.Ranged
{
    public class Repeaters : GlobalItem
    {
        public static List<int> vanillaRepeaters;
        public override void Unload()
        {
            vanillaRepeaters = null;
        }

        public Repeaters()
        {
            vanillaRepeaters = new List<int>
            {
                //钴弩
                ItemID.CobaltRepeater,

                //钯金弩
                ItemID.PalladiumRepeater,
                
                //秘银弩
                ItemID.MythrilRepeater,

                //山铜弩
                ItemID.OrichalcumRepeater,

                //精金弩
                ItemID.AdamantiteRepeater,

                //钛金弩
                ItemID.TitaniumRepeater,

                //神圣弩
                ItemID.HallowedRepeater,

                //叶绿弩
                ItemID.ChlorophyteShotbow,
            };
        }
    }
}
