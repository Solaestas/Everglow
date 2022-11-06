namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.GoldenShower
{
    internal class GoldenShowerBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.Ichor;
            ItemType = ItemID.GoldenShower;
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];
            ConstantUsingTime++;
            if(Projectile.timeLeft == 10000)
            {
                for (int d = 0; d < 16; d++)
                {
                    Vector2 velocity = new Vector2(0, Main.rand.NextFloat(-16f, -12f)).RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f));
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                }
            }
            if (player.itemTime <= 0 || player.HeldItem.type != ItemID.GoldenShower)
            {
                if (Timer < 0)
                {
                    int Rain = Math.Min(ConstantUsingTime / 6, 120);
                    for (int d = 0; d < Rain; d++)
                    {
                        Vector2 velocity = new Vector2(0, Main.rand.NextFloat(-16f, -12f)).RotatedBy(Main.rand.NextFloat(-(Rain / 120f), (Rain / 120f)));
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                    }
                    ConstantUsingTime = 0;
                    Projectile.Kill();
                }
            }
            if (player.itemTime == 2)
            {
                if(Main.mouseRight)
                {
                    player.statMana -= 4;
                    for (int d = 0; d < 3; d++)
                    {
                        Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * player.HeldItem.shootSpeed * 1.3f;
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                    }
                }
                else
                {
                    Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed * 1.3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
                }
            }
        }
        internal int ConstantUsingTime = 0;
    }
}