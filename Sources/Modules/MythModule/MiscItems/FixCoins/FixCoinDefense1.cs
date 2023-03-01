namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
    public class FixCoinDefense1 : FixCoinItem
    {
        public override int Level()
        {
            return 1;
        }

        public override void SSD()
        {
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FixCoins.FixCoinDefense1>();
        }
    }
}
