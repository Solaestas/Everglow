using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles
{
    public class BloodLampProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Lamp");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.scale = 1;
        }
        Texture2D[] BLantern = new Texture2D[16];
        bool[] NoPedal = new bool[16];
        float PearlRot = 0;
        float PearlOmega = 0;
        public override ModProjectile Clone(Projectile projectile)
        {
            var clone = base.Clone(projectile) as BloodLampProj;
            BLantern = new Texture2D[16];
            NoPedal = new bool[16];
            PearlRot = 0;
            PearlOmega = 0;
            return clone;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.X * 0.05f;
            Projectile.velocity *= 0.9f * Projectile.timeLeft / 600f;
            if (Projectile.velocity.Length() > 0.3f)
            {
                Projectile.velocity.Y -= 0.25f * Projectile.timeLeft / 600f;
            }
            Vector2 Cen = new Vector2(19f, 19f);
            if (Projectile.timeLeft == 5)
            {
                Projectile.NewProjectile(null,Projectile.Center,new Vector2(0, -1),ModContent.ProjectileType<LMeteor>(),0,0,Projectile.owner);
            }
            if (Projectile.timeLeft == 540)
            {
                Projectile.timeLeft = 150;
            }
            if (Projectile.timeLeft < 150)
            {
                for (int x = 1; x < 16; x++)
                {
                    if(!NoPedal[x])
                    {
                        if (Main.rand.Next(Projectile.timeLeft) < 3)
                        {
                            NoPedal[x] = true;
                            if (Projectile.timeLeft < 10)
                            {
                                if (x == 1)
                                {
                                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.GoreType<Gores.BloodLanternBody>());
                                }
                            }
                            if (x == 2)
                            {
                                Cen = new Vector2(12f, 9f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f,0,0,ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner,Projectile.whoAmI);
                            }
                            if (x == 3)
                            {
                                Cen = new Vector2(26f, 9f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                            if (x == 4)
                            {
                                Cen = new Vector2(19f, 8f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                            }
                            if (x == 5)
                            {
                                Cen = new Vector2(34f, 14f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                            if (x == 6)
                            {
                                Cen = new Vector2(4f, 14f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                            }
                            if (x == 7)
                            {
                                Cen = new Vector2(8f, 10f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                            if (x == 8)
                            {
                                Cen = new Vector2(30f, 10f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                //Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                            }
                            if (x == 9)
                            {
                                Cen = new Vector2(8f, 25f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                            if (x == 10)
                            {
                                Cen = new Vector2(30f, 25f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                            }
                            if (x == 11)
                            {
                                Cen = new Vector2(32f, 21f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                            if (x == 12)
                            {
                                Cen = new Vector2(6f, 21f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                //Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                            }
                            if (x == 13)
                            {
                                Cen = new Vector2(19f, 23f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                            if (x == 14)
                            {
                                Cen = new Vector2(23f, 16f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                //Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                            }
                            if (x == 15)
                            {
                                Cen = new Vector2(15f, 16f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                            }
                        }
                    }
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
            for (int x = -1; x < 15; x++)
            {
                BLantern[x + 1] = MythContent.QuickTexture("LanternMoon/Projectiles/BloodLampFrame/BloodLamp_" + x.ToString());
            }
            PearlOmega += (Projectile.rotation - PearlRot) / 75f;
            PearlOmega *= 0.95f;
            PearlRot += PearlOmega;
            Color color = Lighting.GetColor((int)(Projectile.Center.X / 16d), (int)(Projectile.Center.Y / 16d));
            for (int x = 0; x < 16; x++)
            {
                float Rot = Projectile.rotation;
                Vector2 Cen = new Vector2(19f, 19f);
                if (x >= 2)
                {
                    Rot = -(float)(Math.Sin(Main.time / 26d + x)) / 7f + Projectile.rotation;
                }
                if(x == 0)
                {
                    Rot = PearlRot;
                    Cen = new Vector2(19f, 51f);
                }
                if(!NoPedal[x] || x < 2)
                {
                    Main.spriteBatch.Draw(BLantern[x], Projectile.Center - Main.screenPosition + Cen - BLantern[x].Size() / 2f/*坐标校正*/, null, color, Rot, Cen, 1, SpriteEffects.None, 0);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}