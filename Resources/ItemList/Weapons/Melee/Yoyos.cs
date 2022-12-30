using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.ItemList.Weapons.Melee
{
    public class Yoyos : GlobalItem
    {
        public static List<int> vanillaYoyos;
        public override void Unload()
        {
            vanillaYoyos = null;
        }

        public Yoyos()
        {
            vanillaYoyos = new List<int>
            {
                //木悠悠球
                ItemID.WoodYoyo,

                //对打球
                ItemID.Rally,

                //抑郁球
                ItemID.CorruptYoyo,

                //血脉球
                ItemID.CrimsonYoyo,

                //亚马逊球
                ItemID.JungleYoyo,

                //代码1球
                ItemID.Code1,

                //英勇球
                ItemID.Valor,

                //喷流球
                ItemID.Cascade,
                
                //好胜球
                ItemID.FormatC,

                //渐变球
                ItemID.Gradient,

                //吉克球
                ItemID.Chik,

                //狱火球
                ItemID.HelFire,

                //冰雪悠悠球
                ItemID.Amarok,
                
                //代码2球
                ItemID.Code2,

                //叶列茨球
                ItemID.Yelets,

                //Red的抛球
                ItemID.RedsYoyo,

                //女武神悠悠球
                ItemID.ValkyrieYoyo,

                //克拉肯球
                ItemID.Kraken,

                //克苏鲁之眼
                ItemID.TheEyeOfCthulhu,

                //泰拉悠悠球
                ItemID.Terrarian,

            };
        }
    }
}
