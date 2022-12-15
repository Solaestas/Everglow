using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Melee
{
    public class ShortSwords : GlobalItem
    {
        private static List<int> vanillaShortSwords;
        public override void Unload()
        {
            vanillaShortSwords = null;
        }

        public ShortSwords()
        {
            vanillaShortSwords = new List<int>
            {
                //铜短剑
                ItemID.CopperShortsword,

                //锡短剑
                ItemID.TinShortsword,

                //铁短剑
                ItemID.IronShortsword,

                //铅短剑
                ItemID.LeadShortsword,

                //银短剑
                ItemID.SilverShortsword,

                //钨短剑
                ItemID.TungstenShortsword,

                //金短剑
                ItemID.GoldShortsword,

                //铂金短剑
                ItemID.PlatinumShortsword,

                //伞
                ItemID.Umbrella,
                
                //悲剧雨伞
                ItemID.TragicUmbrella,

                //标尺
                ItemID.Ruler,

                //罗马短剑
                ItemID.Gladius,

                //星光
                ItemID.PiercingStarlight,//其实不算,但也没地方放了
            };
        }
    }
}
