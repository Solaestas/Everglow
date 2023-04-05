namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class EternalNight_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(209, 94, 255);
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 5;
            Projectile.GetGlobalProjectile<StabbingDrawer>().MaxLength = 1.20f;
        }
    }
}