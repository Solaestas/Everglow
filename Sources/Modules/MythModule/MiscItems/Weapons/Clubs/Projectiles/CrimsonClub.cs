namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class CrimsonClub : ClubProj_metal
    {
        public override void SetDef()
        {
            ReflectStrength = 3f;
            ReflectTexturePath = "Everglow/Sources/Modules/MythModule/MiscItems/Weapons/Clubs/Projectiles/CrimsonClub_light";
        }
        public override void AI()
        {
            base.AI();
            if (Omega > 0.1f)
            {
                for (float d = 0.1f; d < Omega; d += 0.1f)
                {
                    GenerateDust();
                }
            }
            else
            {
                GenerateDust();
            }
        }
        private void GenerateDust()
        {
            Vector2 v0 = new Vector2(1, 1);
            v0 *= Main.rand.NextFloat(Main.rand.NextFloat(1, HitLength), HitLength);
            v0.X *= Projectile.spriteDirection;
            if (Main.rand.NextBool(2))
            {
                v0 *= -1;
            }
            v0 = v0.RotatedBy(Projectile.rotation);
            float Speed = Math.Min(Omega, 0.28f);
            Dust dust = Dust.NewDustDirect(Projectile.Center + v0 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.Blood, -v0.Y * Speed, v0.X * Speed, 0, default, Main.rand.NextFloat(0.8f, 1.4f));
            dust.noGravity = true;       
        }
    }
}
