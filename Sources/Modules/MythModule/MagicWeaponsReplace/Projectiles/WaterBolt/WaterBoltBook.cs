namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.WaterBolt
{
    internal class WaterBoltBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.WaterCandle;
            ItemType = ItemID.WaterBolt;
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];          
            int damage = player.HeldItem.damage;
            if (player.itemTime == 2)
            {
                Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed;
                int T = ProjectileID.WaterBolt;
                if (player.HasBuff(ModContent.BuffType<MagicWeaponsReplace.Buffs.WaterBoltII>()))
                {
                    damage = (int)(damage * 1.85);
                    T = ModContent.ProjectileType<Projectiles.WaterBolt.NewWaterBolt>();
                }
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * 6, velocity, T, damage, player.HeldItem.knockBack, player.whoAmI);
                p.penetrate = 2;
                p.CritChance = (int)player.GetCritChance(DamageClass.Generic);
            }
        }
    }
}