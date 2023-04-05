namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class SilverStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(180, 191, 193);
            base.SetDefaults();
            Projectile.GetGlobalProjectile<StabbingDrawer>().MaxLength = 0.75f;
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.2f;
        }
    }
}