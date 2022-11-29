using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Ranged
{
    public class OtherRengedWeapons : GlobalItem
    {
        private static List<int> vanillaOtherRengedWeapons;
        public override void Unload()
        {
            vanillaOtherRengedWeapons = null;
        }

        public OtherRengedWeapons()
        {
            vanillaOtherRengedWeapons = new List<int>
            {
                //信号枪
                ItemID.FlareGun,

                //麦芽酒投掷器
                ItemID.AleThrowingGlove,

                //吹管
                ItemID.Blowpipe,

                //吹枪
                ItemID.Blowgun,

                //雪球炮
                ItemID.SnowballCannon,

                //彩弹枪
                ItemID.PainterPaintballGun,
                
                //鱼叉枪
                ItemID.Harpoon,

                //300颗
                ItemID.StarCannon,

                //毒液枪
                ItemID.Toxikarp,

                //飞镖手枪
                ItemID.DartPistol,

                //飞镖步枪
                ItemID.DartRifle,

                //火焰喷射器
                ItemID.Flamethrower,

                //水虎鱼枪
                ItemID.PiranhaGun,

                //精灵熔炉
                ItemID.EldMelter,//确定不是拼错了？

                //超级300颗
                ItemID.SuperStarCannon,

                //钉枪
                ItemID.NailGun,

                //毒刺发射器
                ItemID.Stynger,

                //杰克南瓜灯发射器
                ItemID.JackOLanternLauncher,

            };
        }
    }
}
