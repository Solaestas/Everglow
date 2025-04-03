namespace Everglow.Food.ItemList.Weapons.Summon
{
	public class Sentrys : GlobalItem
	{
		public static List<int> vanillaSentrys;

		public override void Unload()
		{
			vanillaSentrys = null;
		}

		public Sentrys()
		{
			vanillaSentrys = new List<int>
			{
				// 闪电光环魔杖
				ItemID.DD2LightningAuraT1Popper,

                // 爆炸烈焰魔杖
                ItemID.DD2FlameburstTowerT1Popper,

                // 爆炸机关魔杖
                ItemID.DD2ExplosiveTrapT1Popper,

                // 弩车魔杖
                ItemID.DD2BallistraTowerT1Popper,


				// 闪电光环手杖
				ItemID.DD2LightningAuraT2Popper,

                // 爆炸烈焰手杖
                ItemID.DD2FlameburstTowerT2Popper,

                // 爆炸机关手杖
                ItemID.DD2ExplosiveTrapT2Popper,

                // 弩车手杖
                ItemID.DD2BallistraTowerT2Popper,


				// 闪电光环法杖
				ItemID.DD2LightningAuraT3Popper,

                // 爆炸烈焰法杖
                ItemID.DD2FlameburstTowerT3Popper,

                // 爆炸机关法杖
                ItemID.DD2ExplosiveTrapT3Popper,

                // 弩车法杖
                ItemID.DD2BallistraTowerT3Popper,

				// 眼球激光塔
				ItemID.HoundiusShootius,

                // 蜘蛛女王法杖
                ItemID.QueenSpiderStaff,

                // 寒霜九头龙法杖
                ItemID.StaffoftheFrostHydra,

                // 月亮传送门法杖
                ItemID.MoonlordTurretStaff,

                // 七彩水晶法杖
                ItemID.RainbowCrystalStaff,
			};
		}
	}
}