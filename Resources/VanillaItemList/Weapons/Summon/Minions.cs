using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.VanillaItemList.Weapons.Summon
{
    public class Minions : GlobalItem
    {
        private static List<int> vanillaMinions;
        public override void Unload()
        {
            vanillaMinions = null;
        }

        public Minions()
        {
            vanillaMinions = new List<int>
            {
                //雀杖
                ItemID.BabyBirdStaff,

                //史莱姆法杖
                ItemID.SlimeStaff,

                //小雪怪法杖
                ItemID.FlinxStaff,
                
                //黄蜂法杖
                ItemID.HornetStaff,
                
                //阿比盖尔的花
                ItemID.AbigailsFlower,

                //吸血鬼青蛙法杖
                ItemID.VampireFrogStaff,

                //小鬼法杖
                ItemID.ImpStaff,

                //刃杖
                ItemID.Smolstar,
                
                //蜘蛛法杖
                ItemID.SpiderStaff,

                //海盗法杖
                ItemID.PirateStaff,

                //血红法杖
                ItemID.SanguineStaff,

                //魔眼法杖
                ItemID.OpticStaff,
                
                //致命球法杖
                ItemID.DeadlySphereStaff,

                //矮人法杖
                ItemID.PygmyStaff,

                //乌鸦法杖
                ItemID.RavenStaff,

                //沙漠虎杖
                ItemID.StormTigerStaff,

                //暴风雨法杖
                ItemID.TempestStaff,

                //泰拉棱镜
                ItemID.EmpressBlade, 
                
                //外星法杖
                ItemID.XenoStaff,

                //星尘细胞法杖
                ItemID.StardustCellStaff,

                //星尘之龙法杖
                ItemID.StardustDragonStaff,

            };
        }
    }
}
