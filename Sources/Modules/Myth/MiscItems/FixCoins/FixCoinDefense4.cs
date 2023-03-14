namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
	public class FixCoinDefense4 : FixCoinItem
	{
		public override int Level()
		{
			return 4;
		}

		public override void SSD()
		{
			Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FixCoins.FixCoinDefense4>();
		}
	}
}
