using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Melee
{
    public class Boomerangs : GlobalItem
    {
        private static List<int> vanillaBoomerangs;
        public override void Unload()
        {
            vanillaBoomerangs = null;
        }

        public Boomerangs()
        {
            vanillaBoomerangs = new List<int>
            {
                //木回旋镖
                ItemID.WoodenBoomerang,

                //附魔回旋镖
                ItemID.EnchantedBoomerang,

                //水果蛋糕旋刃
                ItemID.FruitcakeChakram,

                //血腥砍刀
                ItemID.BloodyMachete,

                //蘑菇回旋镖
                ItemID.Shroomerang,

                //冰雪回旋镖
                ItemID.IceBoomerang,

                //荆棘旋刃
                ItemID.ThornChakram,

                //电工妹扳手
                ItemID.CombatWrench,

                //冰雪回旋镖
                ItemID.Flamarang,
                
                //飞刀
                ItemID.FlyingKnife,

                //美队盾
                ItemID.BouncingShield,

                //光明飞盘
                ItemID.LightDisc,

                //香蕉回旋镖
                ItemID.Bananarang,

                //疯狂飞斧
                ItemID.PossessedHatchet,

                //帕拉丁之锤
                ItemID.PaladinsHammer,
            };
        }
    }
}
