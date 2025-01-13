namespace Everglow.Commons.Utilities.ItemList.Weapons.Summon;

public class Whips : GlobalItem
{
	public static List<int> vanillaWhips;

	public override void Unload()
	{
		vanillaWhips = null;
	}

	public Whips()
	{
		vanillaWhips = new List<int>()
		{
			// 皮鞭
			ItemID.BlandWhip,

			// 荆鞭
			ItemID.ThornWhip,

			// 脊柱骨鞭
			ItemID.BoneWhip,

			// 鞭炮
			ItemID.FireWhip,

			// 冷鞭
			ItemID.CoolWhip,

			// 迪郎达尔
			ItemID.SwordWhip,

			// 暗黑收割
			ItemID.ScytheWhip,

			// 晨星
			ItemID.MaceWhip,

			// 万花筒
			ItemID.RainbowWhip,
		};
	}
}