namespace Everglow.Myth.MiscItems.FixCoins
{
	public class FixCoinDamage4 : FixCoinItem
	{
		public override int Level()
		{
			return 4;
		}

		public override void SSD()
		{
			Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinDamage4>();
		}
	}
}
