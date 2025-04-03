namespace Everglow.Food.ItemList.Weapons.Magic
{
	public class MagicGuns : GlobalItem
	{
		public static List<int> vanillaMagicGuns;

		public override void Unload()
		{
			vanillaMagicGuns = null;
		}

		public MagicGuns()
		{
			vanillaMagicGuns = new List<int>
			{
                // 太空枪
                ItemID.SpaceGun,

                // 蜜蜂枪
                ItemID.BeeGun,

                // 灰冲击枪
                ItemID.ZapinatorGray,

                // 激光步枪
                ItemID.LaserRifle,

                // 橙冲击枪
                ItemID.ZapinatorOrange,

                // 吹叶机
                ItemID.LeafBlower,

                // 彩虹枪
                ItemID.RainbowGun,

                // 胡蜂枪
                ItemID.WaspGun,

                // 高温射线枪
                ItemID.HeatRay,

                // 激光机枪
                ItemID.LaserMachinegun,

                // 充能爆破炮
                ItemID.ChargedBlasterCannon, // 这踏马是枪?

                // 泡泡枪
                ItemID.BubbleGun,
			};
		}
	}
}