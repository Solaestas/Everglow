namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Typeless.FixCoins
{
    public class FixCoinDamage1 : FixCoinProjectile
    {
        public override string HeatMapTexture()
        {
            return "heatmapGrey";
        }
        public override int PrefixID()
        {
            return 0;
        }
        public override int Level()
        {
            return 1;
        }
    }
}