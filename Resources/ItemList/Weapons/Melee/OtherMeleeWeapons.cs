using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Resources.ItemList.Weapons.Melee
{
    public class OtherMeleeWeapons : GlobalItem
    {
        public static List<int> vanillaOtherMeleeWeapons;
        public override void Unload()
        {
            vanillaOtherMeleeWeapons = null;
        }

        public OtherMeleeWeapons()
        {
            vanillaOtherMeleeWeapons = new List<int>
            {
                //泰拉魔刃
                ItemID.Terragrim,

                //Arkhalis剑
                ItemID.Arkhalis,

                //骑枪
                ItemID.JoustingLance,

                //暗影焰刀
                ItemID.ShadowFlameKnife,

                //神圣骑枪
                ItemID.HallowJoustingLance,

                //瞌睡章鱼
                ItemID.MonkStaffT1,//😅😅😅

                //腐化者之戟
                ItemID.ScourgeoftheCorruptor,

                //暗影骑枪
                ItemID.ShadowJoustingLance,
                
                //吸血鬼刀
                ItemID.VampireKnives,

                //天龙之怒
                ItemID.MonkStaffT3,//😅😅😅

                //破晓之光
                ItemID.DayBreak,

                //日耀喷发剑
                ItemID.SolarEruption,

                //天顶剑
                ItemID.Zenith,

            };
        }
    }
}
