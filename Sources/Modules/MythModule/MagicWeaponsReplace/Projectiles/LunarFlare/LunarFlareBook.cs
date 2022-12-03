namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.LunarFlare
{
    internal class LunarFlareBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.UltraBrightTorch;
            ItemType = ItemID.LunarFlareBook;
            ProjType = -1;
            effectColor = new Color(15, 125, 175, 0);
            MulDamage = 2.5f;
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];
            for(int x = 0;x < 4;x++)
            {
                if (player.itemTime == player.itemTimeMax - 2 && player.HeldItem.type == ItemType)
                {
                    Vector2 StartPos = Projectile.Center + new Vector2(Main.rand.NextFloat(-600, 600), -1000);
                    Vector2 vToMouse = Main.MouseWorld - StartPos;
                    Vector2 velocity = Utils.SafeNormalize(vToMouse, Vector2.Zero) * player.HeldItem.shootSpeed * Main.rand.NextFloat(0.85f,1.15f);
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), StartPos + velocity * MulStartPosByVelocity, velocity * MulVelocity * 8, ModContent.ProjectileType<LunarFlareII>(), (int)(player.HeldItem.damage * MulDamage), player.HeldItem.knockBack, player.whoAmI);
                    p.CritChance = (int)player.GetCritChance(DamageClass.Generic);
                }
            }
        }
    }
}
