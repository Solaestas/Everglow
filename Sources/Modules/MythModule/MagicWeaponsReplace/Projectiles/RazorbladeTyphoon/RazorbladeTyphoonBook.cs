namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.RazorbladeTyphoon
{
    internal class RazorbladeTyphoonBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            UseGlow = false;
            ItemType = ItemID.RazorbladeTyphoon;
            DustType = DustID.RazorbladeTyphoon;
            ProjType = ModContent.ProjectileType<TyphoonII>();
        }
        public override void Kill(int timeLeft)
        {
            int HitType = ModContent.ProjectileType<HurricaneMask>();
            Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, HitType, Projectile.damage, Projectile.knockBack * 6, Projectile.owner, 18/*ai[0]代表强度*/);
            p.CritChance = (int)Main.player[Projectile.owner].GetCritChance(DamageClass.Generic);
            base.Kill(timeLeft);
        }
    }
}