namespace Everglow.Commons.Utilities.ItemList.Weapons.Magic;

public class MagicBooks : GlobalItem
{
	public static List<int> vanillaMagicBooks;

	public override void Unload()
	{
		vanillaMagicBooks = null;
	}

	public MagicBooks()
	{
		vanillaMagicBooks = new List<int>()
		{
			// 水箭
			ItemID.WaterBolt,

			// 骷髅头法术
			ItemID.BookofSkulls,

			// 恶魔镰刀
			ItemID.DemonScythe,

			// 咒焰
			ItemID.CursedFlames,

			// 黄金尿
			ItemID.GoldenShower,

			// 水晶风暴
			ItemID.CrystalStorm,

			// 磁球
			ItemID.MagnetSphere,

			// 利刃台风
			ItemID.RazorbladeTyphoon,

			// 月耀
			ItemID.LunarFlareBook,
		};
	}
}