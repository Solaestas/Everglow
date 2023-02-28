namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.WaterBolt
{
    internal class WaterBoltBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.WaterCandle;
            ItemType = ItemID.WaterBolt;
            effectColor = new Color(30, 60, 225, 100);
            string pathBase = "MagicWeaponsReplace/Textures/";
            FrontTexPath = pathBase + "WaterBolt_A";
            PaperTexPath = pathBase + "WaterBolt_C";
            BackTexPath = pathBase + "WaterBolt_B";
            GlowPath = pathBase + "WaterBolt_E";

            TexCoordTop = new Vector2(6, 0);
            TexCoordLeft = new Vector2(0, 24);
            TexCoordDown = new Vector2(22, 24);
            TexCoordRight = new Vector2(28, 0);
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];          
            int damage = player.HeldItem.damage;
            if (player.itemTime == 2)
            {
                Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed;
                int T = ProjectileID.WaterBolt;
                if (player.HasBuff(ModContent.BuffType<Buffs.WaterBoltII>()))
                {
                    damage = (int)(damage * 1.85);
                    T = ModContent.ProjectileType<NewWaterBolt>();
                }
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * 6, velocity, T, damage, player.HeldItem.knockBack, player.whoAmI);
                p.penetrate = 2;
                p.CritChance = player.GetWeaponCrit(player.HeldItem);
            }
        }
    }
}