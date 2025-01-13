namespace Everglow.Commons.Utilities.ItemList.Weapons.Magic;

public class OtherMagicWeapons : GlobalItem
{
	public static List<int> vanillaOtherMagicWeapons;

	public override void Unload()
	{
		vanillaOtherMagicWeapons = null;
	}

	public OtherMagicWeapons()
	{
		vanillaOtherMagicWeapons = new List<int>()
		{
			// 魔法飞刀
			ItemID.MagicDagger,

			// 蛇发女妖头
			ItemID.MedusaHead,

			// 神灯烈焰
			ItemID.SpiritFlame,

			// 暗影焰妖娃
			ItemID.ShadowFlameHexDoll,

			// 血荆棘
			ItemID.SharpTears,

			// 魔法竖琴
			ItemID.MagicalHarp,

			// 毒气瓶
			ItemID.ToxicFlask,

			// 夜光
			ItemID.FairyQueenMagicItem,

			// 星星吉他
			ItemID.SparkleGuitar,

			// 星云奥秘
			ItemID.NebulaArcanum,

			// 星云烈焰
			ItemID.NebulaBlaze,

			// 终极棱镜
			ItemID.LastPrism,

			// 血雨法杖
			ItemID.CrimsonRod,

			// 冰雪魔杖
			ItemID.IceRod,

			// 爬藤怪法杖
			ItemID.ClingerStaff,

			// 雨云魔杖
			ItemID.NimbusRod,
		};
	}
}