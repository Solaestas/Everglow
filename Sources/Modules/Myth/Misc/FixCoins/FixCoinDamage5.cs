namespace Everglow.Myth.Misc.FixCoins;

public class FixCoinDamage5 : FixCoinItem
{
	public override int Level()
	{
		return 5;
	}

	public override void SSD()
	{
		Item.shoot = ModContent.ProjectileType<Projectiles.Typeless.FixCoins.FixCoinDamage5>();
	}
}
