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
        internal float ConstantUsingTime = 0;
        public override void SpecialAI()
        {
            ConstantUsingTime+=1;
        }
        public override void Kill(int timeLeft)
        {
            int HitType = ModContent.ProjectileType<HurricaneMask>();
            float WindHole = Math.Min(ConstantUsingTime / 720f - 0.2f, 1f);
            if(WindHole > 0 && WindHole < 0.3f)
            {
                WindHole = 0.3f;
            }
            if (WindHole > 0)
            {

                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, HitType, Projectile.damage, Projectile.knockBack * 6, Projectile.owner, WindHole/*ai[0]代表强度*/, 0);
                p.CritChance = (int)Main.player[Projectile.owner].GetCritChance(DamageClass.Generic);
            }
            ConstantUsingTime = 0;
            base.Kill(timeLeft);
        }
    }
}