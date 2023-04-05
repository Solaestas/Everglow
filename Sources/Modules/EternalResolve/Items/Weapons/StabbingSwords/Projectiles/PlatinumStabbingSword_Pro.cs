namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class PlatinumStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(206, 226, 226);
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.2f;
        }
    }
}