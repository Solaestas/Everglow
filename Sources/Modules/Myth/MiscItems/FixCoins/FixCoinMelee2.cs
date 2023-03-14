namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
	public class FixCoinMelee2 : FixCoinItem
	{
		public override int Level()
		{
			return 2;
		}

		public override void SSD()
		{
			Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FixCoins.FixCoinMelee2>();
		}
	}
}
