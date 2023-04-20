namespace Everglow.Food.ItemList.Weapons.Melee
{
	public class Spears : GlobalItem
    {
        public static List<int> vanillaSpears;
        public override void Unload()
        {
            vanillaSpears = null;
        }

        public Spears()
        {
            vanillaSpears = new List<int>
            {
                //长矛
                ItemID.Spear,

                //三叉戟
                ItemID.Trident,

                //风暴长矛
                ItemID.ThunderSpear,

                //腐叉
                ItemID.TheRottedFork,

                //剑鱼
                ItemID.Swordfish,

                //暗黑长戟
                ItemID.DarkLance,

                //钴薙刀
                ItemID.CobaltNaginata,

                //钯金刺矛
                ItemID.PalladiumPike,
                
                //秘银长戟
                ItemID.MythrilHalberd,

                //山铜长戟
                ItemID.OrichalcumHalberd,

                //精金关刀
                ItemID.AdamantiteGlaive,

                //钛金三叉戟
                ItemID.TitaniumTrident,

                //永恒之枪
                ItemID.Gungnir,
                
                //恐怖关刀
                ItemID.MonkStaffT2,//什么答辩名字（二阶和尚法杖）fxxk U re!

                //叶绿镋
                ItemID.ChlorophytePartisan,

                //蘑菇长矛
                ItemID.MushroomSpear,
                
                //黑曜石剑鱼
                ItemID.ObsidianSwordfish,
                
                //北极
                ItemID.NorthPole,
            };
        }
    }
}
