namespace Everglow.Myth.MiscItems.FixCoins;

public class FixCoinDefense2 : FixCoinItem
{
	public override int Level()
	{
		return 2;
	}

	public override void SSD()
	{
		Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinDefense2>();
	}
}
