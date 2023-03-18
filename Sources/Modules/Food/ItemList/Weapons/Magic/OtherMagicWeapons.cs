namespace Everglow.Food.ItemList.Weapons.Magic
{
	public class OtherMagicWeapons : GlobalItem
    {
        public static List<int> vanillaOtherMagicWeapons;
        public override void Unload()
        {
            vanillaOtherMagicWeapons = null;
        }

        public OtherMagicWeapons()
        {
            vanillaOtherMagicWeapons = new List<int>
            {
                //魔法飞刀
                ItemID.MagicDagger,

                //蛇发女妖头
                ItemID.MedusaHead,

                //血荆棘
                ItemID.SharpTears,

                //神灯烈焰
                ItemID.SpiritFlame,

                //暗影焰妖娃
                ItemID.ShadowFlameHexDoll,

                //魔法竖琴
                ItemID.MagicalHarp,

                //毒气瓶
                ItemID.ToxicFlask,

                //星星吉他
                ItemID.SparkleGuitar,

                //夜光
                ItemID.FairyQueenMagicItem, //又一个答辩名字，差不多得了
                
                //星云奥秘
                ItemID.NebulaArcanum,

                //星云烈焰
                ItemID.NebulaBlaze,

                //终极棱镜
                ItemID.LastPrism,

                # region 这些我不好说
                /*
                //血雨法杖
                ItemID.CrimsonRod,

                //冰雪魔杖
                ItemID.IceRod,

                //爬藤怪法杖
                ItemID.ClingerStaff,

                //雨云魔杖
                ItemID.NimbusRod,
                */
                #endregion
            };
        }
    }
}
