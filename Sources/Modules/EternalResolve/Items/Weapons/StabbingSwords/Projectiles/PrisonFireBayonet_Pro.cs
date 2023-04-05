namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class PrisonFireBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = Color.Orange;
            // Projectile.GetGlobalProjectile<StabbingDrawer>().fadeColor = 0.8f;
            //Projectile.GetGlobalProjectile<StabbingDrawer>().fadeScale = 0.93f;
            //Projectile.GetGlobalProjectile<StabbingDrawer>().TradeLength = 30;
            //Projectile.GetGlobalProjectile<StabbingDrawer>().TredeShade = 0.75f;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Shade = 0.65f;
            base.SetDefaults();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.buffImmune[BuffID.OnFire] = false;
            target.AddBuff(BuffID.OnFire, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}