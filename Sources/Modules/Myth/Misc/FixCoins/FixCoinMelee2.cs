namespace Everglow.Myth.Misc.FixCoins;

public class FixCoinMelee2 : FixCoinItem
{
	public override int Level()
	{
		return 2;
	}

	public override void SSD()
	{
		Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinMelee2>();
	}
}
