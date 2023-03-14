namespace Everglow.Myth.MiscItems.FixCoins
{
	public class FixCoinDefense5 : FixCoinItem
	{
		public override int Level()
		{
			return 5;
		}

		public override void SSD()
		{
			Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinDefense5>();
		}
	}
}
