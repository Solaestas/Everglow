namespace Everglow.Sources.Modules.MythModule.MiscItems.FixCoins
{
    public class FixCoinDefense3 : FixCoinItem
    {
        public override int Level()
        {
            return 3;
        }

        public override void SSD()
        {
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Typeless.FixCoins.FixCoinDefense3>();
        }
    }
}
