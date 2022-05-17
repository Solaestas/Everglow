using ReLogic.Content;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles
{
    public class BloodLampProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            for (int x = -1; x < 15; x++)
            {
                BLantern[x + 1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/BloodLampFrame/BloodLamp_" + x.ToString());
            }
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
        //这种贴图每个Proj都是一样的也不会变化，干脆直接readonly然后在SSD里Request，然后所有Proj共用一个数组
        private readonly Asset<Texture2D>[] BLantern = new Asset<Texture2D>[16];
        //这个bool数组每个Proj不同，所以要到Clone里new，但是直接构造是不必要的，因为不是clone获得的那个实例不会调用AI与Draw
        private bool[] NoPedal;
        //值类型就不必在clone里重新初始化了
        private float PearlRot = 0;
        private float PearlOmega = 0;
        private int Col = 100;
        private float Vl = -1;
        public override ModProjectile Clone(Projectile projectile)
        {
            var clone = base.Clone(projectile) as BloodLampProj;
            NoPedal = new bool[16];
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
            Col = Math.Clamp(Col + Main.rand.Next(-45, 55),0,255);
            if(Projectile.timeLeft == 600)
            {
                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sources/Modules/MythModule/LanternMoon/Sounds/PowerBomb"), Projectile.Center);
                if(Vl == -1)
                {
                    Vl = Main.musicVolume;
                }
            }
            if (Main.audioSystem is LegacyAudioSystem system)
            {
                if(Projectile.timeLeft >= 65)
                {
                    Main.musicVolume *= 0.98f;
                }
                else
                {
                    Main.musicVolume = Main.musicVolume * 0.96f + Vl * 0.04f;
                    if(Projectile.timeLeft == 1)
                    {
                        Main.musicVolume = Vl;
                    }
                }
            }
            if (Projectile.timeLeft == 75)
            {
                Col = 255;
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center,new Vector2(0, -1),ModContent.ProjectileType<LMeteor>(),0,0,Projectile.owner);//金色的核心
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position,Vector2.Zero,ModContent.GoreType<Gores.BloodLanternBody>());
                for(int h = 0;h < 11;h++)
                {
                    int f = Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(9, 24f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffectGra>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                    Main.projectile[f].scale = Main.rand.NextFloat(0.75f,1.25f);
                    Main.projectile[f].timeLeft = Main.rand.Next(80, 130);
                }
                NoPedal[1] = true;

            }
            if(Projectile.timeLeft == 70)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, new Vector2(0, -1), ModContent.ProjectileType<RainbowWave>(), 0, 0, Projectile.owner);//彩虹光环
            }
            if (Projectile.timeLeft == 430)
            {
                Projectile.timeLeft = 150;
            }
            if (Projectile.timeLeft < 150)
            {
                for (int x = 2; x < 16; x++)
                {
                    if (!NoPedal[x] && Main.rand.Next(Projectile.timeLeft) < 3)
                    {
                        NoPedal[x] = true;
                        Vector2 Cen;//移动到变量使用附近了
                        switch (x)
                        {
                            case 2:
                                Cen = new Vector2(12f, 9f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 3:
                                Cen = new Vector2(26f, 9f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 4:
                                Cen = new Vector2(19f, 8f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 5:
                                Cen = new Vector2(34f, 14f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 6:
                                Cen = new Vector2(4f, 14f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 7:
                                Cen = new Vector2(8f, 10f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                break;
                            case 8:
                                Cen = new Vector2(30f, 10f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 9:
                                Cen = new Vector2(8f, 25f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                break;
                            case 10:
                                Cen = new Vector2(30f, 25f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center + Cen - BLantern[x].Size() / 2f, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 11:
                                Cen = new Vector2(32f, 21f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                break;
                            case 12:
                                Cen = new Vector2(6f, 21f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 13:
                                Cen = new Vector2(19f, 23f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                break;
                            case 14:
                                Cen = new Vector2(23f, 16f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                Projectile.NewProjectile(null, Projectile.Center, new Vector2(0, Main.rand.NextFloat(12, 20f)).RotatedByRandom(6.283), ModContent.ProjectileType<LBloodEffect>(), 0, 0, Projectile.owner, Projectile.whoAmI);
                                break;
                            case 15:
                                Cen = new Vector2(15f, 16f) - new Vector2(6f, 7f);
                                Dust.NewDust(Projectile.Center + Cen - BLantern[x].Size() / 2f, 0, 0, ModContent.DustType<Dusts.BloodPedal>());
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public override void PostDraw(Color lightColor)
        {
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
                else if (x == 0)
                {
                    Rot = PearlRot;
                    Cen = new Vector2(19f, 51f);
                }
                if (!NoPedal[x] || x < 1)
                {
                    Main.spriteBatch.Draw(BLantern[x].Value, Projectile.Center - Main.screenPosition + Cen - BLantern[x].Size() / 2f/*坐标校正*/, null, color, Rot, Cen, 1, SpriteEffects.None, 0);
                    if (!NoPedal[x] && x == 1)
                    {
                        Vector2 Cen2 = new Vector2(19f, 19f);
                        Texture2D Glow = MythContent.QuickTexture("LanternMoon/Projectiles/BloodLampFrame/BloodLamp_Glow");
                        Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition + Cen2 - Glow.Size() / 2f/*坐标校正*/, null, new Color(Col, Col, Col, 0), Projectile.rotation, Cen2, 1, SpriteEffects.None, 0);
                    }
                }
            }
            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}