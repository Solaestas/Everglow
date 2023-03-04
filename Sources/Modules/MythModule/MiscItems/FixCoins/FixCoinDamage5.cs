namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
    public class FixCoinDamage5 : FixCoinItem
    {
        public override int Level()
        {
            return 5;
        }

        public override void SSD()
        {
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FixCoins.FixCoinDamage5>();
        }
    }
}
