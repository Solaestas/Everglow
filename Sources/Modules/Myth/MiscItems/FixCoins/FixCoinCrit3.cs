namespace Everglow.Myth.MiscItems.FixCoins;

public class FixCoinCrit3 : FixCoinItem
{
	public override int Level()
	{
		return 3;
	}

	public override void SSD()
	{
		Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinCrit3>();
	}
}
