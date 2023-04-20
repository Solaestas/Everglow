namespace Everglow.Myth.MiscItems.FixCoins;

public class FixCoinDefense3 : FixCoinItem
{
	public override int Level()
	{
		return 3;
	}

	public override void SSD()
	{
		Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinDefense3>();
	}
}
