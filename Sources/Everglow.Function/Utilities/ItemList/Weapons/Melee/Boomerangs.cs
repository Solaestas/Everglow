namespace Everglow.Commons.Utilities.ItemList.Weapons.Melee;

public class Boomerangs : GlobalItem
{
	public static List<int> vanillaBoomerangs;

	public override void Unload()
	{
		vanillaBoomerangs = null;
	}

	public Boomerangs()
	{
		vanillaBoomerangs = new List<int>
		{
				// 木回旋镖
				ItemID.WoodenBoomerang,

				// 附魔回旋镖
				ItemID.EnchantedBoomerang,

				// 水果蛋糕旋刃
				ItemID.FruitcakeChakram,

				// 血腥砍刀
				ItemID.BloodyMachete,

				// 蘑菇回旋镖
				ItemID.Shroomerang,

				// 冰雪回旋镖
				ItemID.IceBoomerang,

				// 荆棘旋刃
				ItemID.ThornChakram,

				// 战斗扳手
				ItemID.CombatWrench,

				// 烈焰回旋镖
				ItemID.Flamarang,

				// 三尖回旋镖
				ItemID.Trimarang,

				// 飞刀
				ItemID.FlyingKnife,

				// 中士联盾
				ItemID.BouncingShield,

				// 光辉飞盘
				ItemID.LightDisc,

				// 香蕉回旋镖
				ItemID.Bananarang,

				// 疯狂飞斧
				ItemID.PossessedHatchet,

				// 圣骑士锤
				ItemID.PaladinsHammer,
		};
	}
}