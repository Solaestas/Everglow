namespace Everglow.Food.ItemList.Weapons.Melee
{
	public class Flails : GlobalItem
	{
		public static List<int> vanillaFlails;

		public override void Unload()
		{
			vanillaFlails = null;
		}

		public Flails()
		{
			vanillaFlails = new List<int>
			{
                // 链刀
                ItemID.ChainKnife,

                // 链锤
                ItemID.Mace,

                // 烈焰链锤
                ItemID.FlamingMace,

                // 链球
                ItemID.BallOHurt,

                // 血肉之球
                ItemID.TheMeatball,

                // 蓝月
                ItemID.BlueMoon,

                // 阳炎之怒
                ItemID.Sunfury,

                // 锚
                ItemID.Anchor,

                // 致胜炮
                ItemID.KOCannon,

                // 滴滴怪致残者
                ItemID.DripplerFlail,

                // 铁链血滴子
                ItemID.ChainGuillotines,

                // 太极连枷
                ItemID.DaoofPow,

                // 花冠
                ItemID.FlowerPow,

                // 石巨人之拳
                ItemID.GolemFist,

                // 猪鲨链球
                ItemID.Flairon,
			};
		}
	}
}