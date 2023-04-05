namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class BloodGoldBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = Color.Red;
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.5f;
        }
    }
}