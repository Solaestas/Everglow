namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Proj
{
    public class VegetationBayonet_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = new Color(153, 255, 122);
            base.SetDefaults();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.buffImmune[BuffID.Poisoned] = false;
            target.AddBuff(BuffID.Poisoned, 240);
            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}