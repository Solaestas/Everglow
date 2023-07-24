namespace Everglow.Myth.Misc.FixCoins;

public class FixCoinCrit4 : FixCoinItem
{
	public override int Level()
	{
		return 4;
	}

	public override void SSD()
	{
		Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinCrit4>();
	}
}
