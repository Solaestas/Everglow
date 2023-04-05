namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class LeadStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(85, 114, 123);
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.2f;
            Projectile.GetGlobalProjectile<StabbingDrawer>().MaxLength = 0.72f;
        }
    }
}