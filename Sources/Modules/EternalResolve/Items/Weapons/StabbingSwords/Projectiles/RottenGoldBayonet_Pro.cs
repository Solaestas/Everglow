namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class RottenGoldBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(211, 145, 255);
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.5f;
        }
    }
}