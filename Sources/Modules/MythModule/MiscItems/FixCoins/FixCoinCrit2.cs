namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
    public class FixCoinCrit2 : FixCoinItem
    {
        public override int Level()
        {
            return 2;
        }

        public override void SSD()
        {
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FixCoins.FixCoinCrit2>();
        }
    }
}
