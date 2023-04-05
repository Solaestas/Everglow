namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class IronStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(160, 144, 134);
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.2f;
            Projectile.GetGlobalProjectile<StabbingDrawer>().MaxLength = 0.70f;
        }
    }
}