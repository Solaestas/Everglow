using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Graphics.Effects;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.NPCs.LanternGhostKing
{
    [AutoloadBossHead]
    public class LanternGhostKing : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lantern Ghost King");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼鬼王");
            Main.npcFrameCount[NPC.type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.damage = 100;
            if (Main.expertMode)
            {
                NPC.lifeMax = 20000;
            }
            else
            {
                NPC.lifeMax = 30000;
            }
            NPC.npcSlots = 14f;
            NPC.width = 250;
            NPC.height = 150;
            NPC.defense = 50;
            NPC.value = 20000;
            NPC.aiStyle = -1;
            NPC.boss = false;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit3;
        }
        private int A2 = -2;
        private int U = 0;
        public static bool Canai = false;
        private bool NearDie = false;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.1f;
            NPC.frameCounter %= (double)Main.npcFrameCount[NPC.type];
            int num = (int)NPC.frameCounter;
            NPC.frame.Y = num * frameHeight;
        }
        private Vector2 VLan = new Vector2(0, 0);
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            if (A2 > 14400)
            {
                lightStre = (14680 - A2) / 240f;
            }
            else
            {
                lightStre = 1;
            }
            if (Main.dayTime)
            {
                canDespawn = true;
            }
            else
            {
                canDespawn = false;
            }
            if (A2 == -2)
            {
                Cirposi = NPC.Center;
            }
            if (Canai)
            {
                A2 += 1;
            }
            else
            {
                A2 = -1;
            }
            Lighting.AddLight(NPC.Center, (float)(255 - NPC.alpha) * 0.75f / 255f * NPC.scale, (float)(255 - NPC.alpha) * 0.24f * NPC.scale / 255f, (float)(255 - NPC.alpha) * 0f / 255f * NPC.scale);
            if (false/*NPC.CountNPCS(mod.NPCType("FloatLantern>()) + NPC.CountNPCS(mod.NPCType("LanternSprite>()) + NPC.CountNPCS(mod.NPCType("RedPackBomber>()) + NPC.CountNPCS(mod.NPCType("PaperCuttingBat>()) + NPC.CountNPCS(mod.NPCType("HappinessZombie>()) > 0*/)
            {
                NPC.dontTakeDamage = true;
            }
            else
            {
                if (!NearDie)
                {
                    NPC.dontTakeDamage = false;
                }
            }
            /*if (NPC.CountNPCS(mod.NPCType("FloatLantern>()) + NPC.CountNPCS(mod.NPCType("LanternSprite>()) + NPC.CountNPCS(mod.NPCType("RedPackBomber>()) + NPC.CountNPCS(mod.NPCType("PaperCuttingBat>()) + NPC.CountNPCS(mod.NPCType("HappinessZombie>()) <= 0 && !Canai)
            {

            }*/
            Canai = true;
            NPC.boss = true;
            if (player.dead)
            {
                if (NPC.life < NPC.lifeMax)
                {
                    NPC.life += 10;
                }
                else
                {
                    NPC.life = NPC.lifeMax;
                }
            }
            if (A2 <= 0)
            {
                NPC.rotation = NPC.velocity.X / 120f;
                Vector2 v = player.Center + new Vector2((float)Math.Sin(A2 / 40f) * 500f, (float)Math.Sin((A2 + 200) / 40f) * 50f - 350) - NPC.Center;
                if (NPC.velocity.Length() < 9f)
                {
                    NPC.velocity += v / v.Length() * 0.35f;
                }
                NPC.velocity *= 0.96f;
                Cirpo = NPC.Center;
                CirRpur = 1200;
            }
            if (A2 < 700 && A2 > 0)
            {
                NPC.rotation = NPC.velocity.X / 120f;
                Vector2 v = player.Center + new Vector2((float)Math.Sin(A2 / 40f) * 500f, (float)Math.Sin((A2 + 200) / 40f) * 50f - 350) - NPC.Center;
                if (NPC.velocity.Length() < 9f)
                {
                    NPC.velocity += v / v.Length() * 0.35f;
                }
                NPC.velocity *= 0.96f;
                if (A2 % 30 == 1)
                {
                    Vector2 v2 = new Vector2(0, Main.rand.Next(0, 2500) / 10000f).RotatedByRandom(Math.PI * 2f);
                    Vector2 v4 = new Vector2(0, Main.rand.Next(0, 7000) / 1000f).RotatedByRandom(Math.PI * 2f);
                    if (A2 % 60 == 1)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + v4.X, NPC.Center.Y + 110f + v4.Y, NPC.velocity.X / 3f + v2.X, NPC.velocity.Y * 0.25f + v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.GoldLanternLine>(), 75, 0f, player.whoAmI, 0f, 0f);
                    }
                    for (int h = 0; h < 15; h++)
                    {
                        Vector2 vn = new Vector2(0, -20).RotatedBy(Main.rand.NextFloat(-2f, 2f) + NPC.rotation);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + vn.X * 5, NPC.Center.Y + vn.Y * 5, NPC.velocity.X + vn.X, NPC.velocity.Y + vn.Y, ModContent.ProjectileType<Projectiles.LanternKing.GoldLanternLine4>(), 25, 0f, player.whoAmI, 0, 0);
                    }
                }
                Cirpo = NPC.Center;
                CirRpur = 1200;
            }
            if (A2 >= 700 && A2 < 1500)
            {
                if (A2 % 250 == 0)
                {
                    VLan = new Vector2(0, -300).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f) * Math.PI);
                }
                if (A2 % 250 < 20)
                {
                    NPC.velocity *= 0.95f;
                    NPC.rotation *= 0.95f;
                }
                if (A2 % 250 < 30 && A2 % 250 < 20)
                {
                    NPC.velocity *= 0;
                    NPC.rotation *= 0.95f;
                }
                if (A2 % 250 == 30)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 v1 = new Vector2(0, 100).RotatedBy(i / 6d * Math.PI);
                        Vector2 v2 = v1 + NPC.Center;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X, v2.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern>(), 50, 0f, player.whoAmI, 120 - A2 % 250, (float)(i / 6d * Math.PI));
                    }
                }
                if (A2 % 250 == 60)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        Vector2 v1 = new Vector2(0, 150).RotatedBy(i / 12d * Math.PI);
                        Vector2 v2 = v1 + NPC.Center;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X, v2.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern>(), 50, 0f, player.whoAmI, 120 - A2 % 250, (float)(i / 12d * Math.PI));
                    }
                }
                if (A2 % 250 == 90)
                {
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 v1 = new Vector2(0, 200).RotatedBy(i / 18d * Math.PI);
                        Vector2 v2 = v1 + NPC.Center;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X, v2.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern>(), 50, 0f, player.whoAmI, 120 - A2 % 250, (float)(i / 18d * Math.PI));
                    }
                }
                /*if(A2 % 250 == 120)
                {
                    //Main.PlaySound(2, (int)NPC.position.X, (int)NPC.position.Y, 36, 1f, 0f);
                    for (int i = 0; i < 8; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.FireBallWave>(), 0, 0, player.whoAmI, 0f, 0f);
                    }
                }*/
                if (A2 % 250 > 120)
                {
                    Vector2 v = player.Center + VLan + player.velocity * 30f;
                    NPC.velocity += (v - NPC.Center) / (v - NPC.Center).Length() * 0.25f;
                    if (NPC.velocity.Length() > 20f)
                    {
                        NPC.velocity *= 0.96f;
                    }
                }
                Cirpo = NPC.Center;
                CirRpur = 1200;
            }
            if (A2 >= 1500 && A2 < 1700)
            {
                NPC.rotation *= 0.95f;
                NPC.velocity *= 0.95f;
                if (A2 == 1600)
                {
                    for (int j = 0; j < 150; j++)
                    {
                        Vector2 v2 = new Vector2(0, Main.rand.Next(Main.rand.Next(0, 1200), 1200)).RotatedByRandom(Math.PI * 2);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLanternBomb>(), 50, 0f, player.whoAmI, v2.Length() / 4f, 0);
                    }
                }
            }
            if (A2 >= 1700 && A2 < 2000)
            {
                NPC.rotation *= 0.95f;
                NPC.velocity *= 0.95f;
                if (A2 == 1700)
                {
                    for (int t = 0; t < Main.projectile.Length; t++)
                    {
                        if (Main.projectile[t].type == ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern>() && Main.projectile[t].active && Main.projectile[t].timeLeft > 180)
                        {
                            Main.projectile[t].timeLeft = 180;
                        }
                    }
                }
                if (A2 % 9 == 0)
                {
                    float dx = (A2 - 1700) / 300f;
                    for (int j = 0; j < 6; j++)
                    {
                        Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * j / 3d + dx * dx * 4);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern2>(), 50, 0f, player.whoAmI, 0, 0);
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * j / 1.5 + dx * dx * 4);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern3>(), 50, 0f, player.whoAmI, 0, 0);
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * (j + 1.5) / 1.5 + dx * dx * 4);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern4>(), 50, 0f, player.whoAmI, 0, 0);
                    }
                }
            }
            if (A2 >= 2000 && A2 < 2500)
            {
                NPC.rotation *= 0.95f;
                Vector2 v = player.Center + new Vector2(0, -350) - NPC.Center;
                if (NPC.velocity.Length() < 9f)
                {
                    NPC.velocity += v / v.Length() * 0.35f;
                    NPC.velocity.X += player.velocity.X * 0.07f;
                }
                NPC.velocity *= 0.96f;
                if (A2 % 60 == 0)
                {
                    NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y + 100, ModContent.NPCType<NPCs.FloatLantern>(), 0, 0, 0, 0, 0, 255);
                }
                Cirpo = NPC.Center;
                CirRpur = 1200;
            }
            if (A2 >= 2500 && A2 < 3000)
            {
                NPC.rotation *= 0.95f;
                Vector2 vz = player.Center + new Vector2(0, -350) - NPC.Center;
                if (NPC.velocity.Length() < 9f)
                {
                    NPC.velocity += vz / vz.Length() * 0.35f;
                    NPC.velocity.X += player.velocity.X * 0.07f;
                }
                NPC.velocity *= 0.96f;
                if (A2 % 6 == 0)
                {
                    Vector2 v = new Vector2(0, -1.8f).RotatedBy(Main.rand.NextFloat(-1f, 1f));
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + Main.rand.Next(-600, 600), player.Center.Y - 500, v.X + NPC.velocity.X, v.Y + NPC.velocity.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern>(), 50, 0f, player.whoAmI, 0, 0);
                }
                if (A2 % 120 == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 v0 = new Vector2(0, -Main.rand.Next(120, 570)).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
                        Vector2 v = v0 / 1000000f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + v0.X, player.Center.Y + v0.Y, -v.X, -v.Y, ModContent.ProjectileType<Projectiles.LanternKing.ExplodeLantern>(), 50, 0f, player.whoAmI, 0, 0);
                    }
                }
                Cirpo = NPC.Center;
                CirRpur = 1200;
            }
            if (A2 >= 13000 && A2 < 13700)
            {
                NPC.rotation *= 0.95f;
                NPC.velocity *= 0.95f;
                if (A2 % 9 == 0)
                {
                    float dx = (A2 - 1700) / 300f;
                    for (int j = 0; j < 6; j++)
                    {
                        Vector2 v2 = new Vector2(0, 1);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirpo.X + (j - 2.5f) * 360 + (float)(Math.Sin(A2 / 50f + j) * 150f), Cirpo.Y - 1400, v2.X, v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern2>(), 16, 0f, player.whoAmI, 0, 0);
                        //Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirpo.X + (j - 2.5f) * 200 + (Adding % 100 - 50), Cirpo.Y - 1000, v2.X, v2.Y * 2.5f, ModContent.ProjectileType<Projectiles.LanternKing.GoldLanternLine6>(), 50, 0f, player.whoAmI, 0, 0);
                    }
                }
                if (A2 == 13000)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirposi.X, Cirposi.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern3>(), 16, 0f, player.whoAmI, j, 360);
                    }
                    for (int j = 0; j < 30; j++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirposi.X, Cirposi.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern3>(), 16, 0f, player.whoAmI, j + 1, 720);
                    }
                    for (int j = 0; j < 30; j++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirposi.X, Cirposi.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern3>(), 16, 0f, player.whoAmI, j + 2, 1080);
                    }
                }
                if (A2 % 300 == 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Vector2 v = new Vector2(0, 120).RotatedBy(j / 5d * Math.PI);
                        Vector2 v2 = v.RotatedBy(Math.PI * 1.5) / v.Length();
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirposi.X + v.X, Cirposi.Y + v.Y, v2.X, v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern2>(), 16, 0f, player.whoAmI, 0, 0);
                    }
                    for (int j = 0; j < 10; j++)
                    {
                        Vector2 v = new Vector2(0, 300).RotatedBy(j / 5d * Math.PI);
                        Vector2 v2 = v.RotatedBy(Math.PI * 1.5) / v.Length();
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), Cirposi.X + v.X, Cirposi.Y + v.Y, v2.X, v2.Y, ModContent.ProjectileType<Projectiles.LanternKing.floatLantern2>(), 16, 0f, player.whoAmI, 0, 0);
                    }
                }
                Cirpo = NPC.Center + new Vector2(0, -500);
                CirRpur = 600;
                Adc = (float)(Math.Sin((A2 - 13000f) / 600d * Math.PI) / 400f);
            }
            if (A2 >= 13700 && A2 < 14400)
            {
                NPC.rotation *= 0.95f;
                NPC.velocity *= 0.95f;
                if (A2 == 13900)
                {
                    for (int l = 0; l < 5; l++)
                    {
                        Vector2 v0 = new Vector2(0, -7).RotatedBy(Main.rand.NextFloat(0f, 6.28f));
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v0, ModContent.ProjectileType<Projectiles.LanternKing.Redlight>(), 0, 0f, player.whoAmI, 0, 0);
                    }
                }
                if (A2 == 13700)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 48, 0, ModContent.ProjectileType<Projectiles.LanternKing.GoldLanternLine8>(), 0, 0f, player.whoAmI, 0, 0);
                }
                Cirpo = NPC.Center + new Vector2(0, -300);
                CirRpur = 1000;
            }
            /*if(A2 == 13700)
            {
                for (int t = 0; t < Main.Projectile.Length; t++)
                {
                    if (Main.projectile[t].type == ModContent.ProjectileType<Projectiles.LanternKing.DarkLantern3>() && Main.projectile[t].active && Main.projectile[t].timeLeft > 60)
                    {
                        Main.projectile[t].timeLeft = 60;
                    }
                }
                for (int t = 0; t < Main.Projectile.Length; t++)
                {
                    if (Main.projectile[t].type == ModContent.ProjectileType<Projectiles.LanternKing.floatLantern2>() && Main.projectile[t].active && Main.projectile[t].timeLeft > 60)
                    {
                        Main.projectile[t].timeLeft = 60;
                    }
                }
            }*/
            if (A2 == 2998)
            {
                A2 = 1;
            }
            if (A2 == 2999)
            {
                A2 = 1;
            }
            if (A2 >= 14200)
            {
                NPC.StrikeNPC(10005, 0, 1);
            }
            if (Main.dayTime)
            {
                NPC.velocity.Y += 1;
            }
            if (A2 % 15 == 0)
            {
                U += 1;
            }
            CirR = CirR * 0.99f + CirRpur * 0.01f;
            Cirposi = Cirposi * 0.99f + Cirpo * 0.01f;
        }
        public static float Adc = 0;
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
        }
        public override bool CheckActive()
        {
            return this.canDespawn;
        }
        private bool canDespawn;
        public override void HitEffect(int hitDirection, double damage)
        {
            //MythPlayer mplayer = Main.player[player.whoAmI].GetModPlayer<MythPlayer>();
            /*if(20000 - (int)(NPC.life / (float)NPC.lifeMax * 10000f) < 20000)
            {
                mplayer.LanternMoonPoint = 20000 - (int)(NPC.life / (float)NPC.lifeMax * 10000f);
            }*/
            if (NPC.life <= 0)
            {
                if (!NearDie)
                {
                    NearDie = true;
                    NPC.life = 10000;
                    NPC.active = true;
                    NPC.dontTakeDamage = true;
                    A2 = 12999;
                    return;
                }
                else
                {

                    float vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    Vector2 vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr0 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore0").Type, 1f);
                    Main.gore[gr0].timeLeft = 900;
                    for (int f = 0; f < 13; f++)
                    {
                        vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                        int gra0 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore3").Type, 1f);
                        Main.gore[gra0].timeLeft = Main.rand.Next(600, 900);
                        vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                        int gra1 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore4").Type, 1f);
                        Main.gore[gra1].timeLeft = Main.rand.Next(600, 900);
                        vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                        int gra2 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore5").Type, 1f);
                        Main.gore[gra2].timeLeft = Main.rand.Next(600, 900);
                        vF = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
                        int gra3 = Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/FloatLanternGore6").Type, 1f);
                        Main.gore[gra3].timeLeft = Main.rand.Next(600, 900);
                    }
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr1 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore1").Type, 1f);
                    Main.gore[gr1].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr2 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore2").Type, 1f);
                    Main.gore[gr2].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr3 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore3").Type, 1f);
                    Main.gore[gr3].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr4 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore4").Type, 1f);
                    Main.gore[gr4].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr5 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
                    Main.gore[gr5].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr6 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
                    Main.gore[gr6].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr7 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
                    Main.gore[gr7].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr8 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
                    Main.gore[gr8].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr9 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
                    Main.gore[gr9].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr10 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
                    Main.gore[gr10].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr11 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
                    Main.gore[gr11].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr12 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
                    Main.gore[gr12].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr13 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore7").Type, 1f);
                    Main.gore[gr13].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr14 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore8").Type, 1f);
                    Main.gore[gr14].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr15 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore9").Type, 1f);
                    Main.gore[gr15].timeLeft = 900;
                    vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                    vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                    int gr16 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore10").Type, 1f);
                    Main.gore[gr16].timeLeft = 900;
                    for (int i = 0; i < 8; i++)
                    {
                        vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                        vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                        int gr17 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore11").Type, 1f);
                        Main.gore[gr17].timeLeft = 900;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                        vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                        int gr18 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore12").Type, 1f);
                        Main.gore[gr18].timeLeft = 900;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        vFX = Main.rand.NextFloat(-0.4f, 0.4f);
                        vF = new Vector2(vFX, -(float)(Math.Cos(vFX * Math.PI)) * 6f);
                        int gr19 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), vF, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore12").Type, 1f);
                        Main.gore[gr19].timeLeft = 900;
                    }

                    /*if (mplayer.LanternMoonWave == 15)
                    {
                        mplayer.LanternMoonPoint = 20010;
                    }
                    for (int k = 0; k <= 30; k++)
                    {
                        Vector2 v0 = new Vector2(0, Main.rand.Next(0, 140)).RotatedByRandom(Math.PI * 2);
                        int num4 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + v0.X, NPC.Center.Y + v0.Y, 0, 0, ModContent.ProjectileType<Projectiles.LanternKing.MeltingpotBlaze"), 0, 0, player.whoAmI, Main.rand.Next(1000, 3000) / 1000f, 0f);
                    }
                    for (int k = 0; k <= 10; k++)
                    {
                        Vector2 v0 = new Vector2(0, Main.rand.Next(0, 140)).RotatedByRandom(Math.PI * 2);
                        float a = (float)Main.rand.Next(0, 720) / 360 * 3.141592653589793238f;
                        float m = (float)Main.rand.Next(0, 50000);
                        float l = (float)Main.rand.Next((int)m, 50000) / 1800;
                        int num4 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + v0.X, NPC.Center.Y + v0.Y, (float)((float)l * Math.Cos((float)a)) * 0.36f, (float)((float)l * Math.Sin((float)a)) * 0.36f, ModContent.ProjectileType<Projectiles.LanternKing.火山溅射"), 0, 0, player.whoAmI, 0f, 0f);
                        Main.projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
                    }*/
                }
            }
        }
        private Vector2 Cirpo;
        public static Vector2 Cirposi;
        public static float CirR = 0;
        private float CirRpur = 1200;
        private float Adding = 0;
        private int Fy = 0;
        private int fyc = 0;
        private float Cl = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Adding += 0.004f;
            if (Adding <= 1)
            {
                Cl = Adding;
            }
            else
            {
                Cl = 1;
            }
            fyc += 1;
            if (fyc == 8)
            {
                fyc = 0;
                Fy += 1;
            }
            if (Fy > 3)
            {
                Fy = 0;
            }
            //Mod mod = ModLoader.GetMod("MythMod");
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            /*for(int Maximun = 0;Maximun < 90;Maximun++)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/LanternKing/LanternFire").Value, Cirposi - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, CirR).RotatedBy(Maximun / 45d * Math.PI + Adding), new Rectangle?(new Rectangle(0, 30 * ((Fy + Maximun) % 4), 20, 30)), new Color(Cl * 0.8f, Cl * 0.8f, Cl * 0.8f,0), 0, new Vector2(10, 15), 1f, SpriteEffects.None, 1f);
            }*/
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(250, 110), NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
        Vector2 vCircle = Vector2.Zero;
        bool CheckNewBoss = true;
        int PauseCool = 120;
        float lightStre = 1;
        bool CheckAutoPause = false;
        bool HasCheckAutoPause = false;
        private Effect ef;
        public static float VagueStre = 0;

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/VagueBoss").Value;
            if (CheckNewBoss)
            {
                if (Main.netMode != 0)
                {
                    CheckNewBoss = false;
                    return;
                }
                if (!HasCheckAutoPause)
                {
                    CheckAutoPause = Main.autoPause;
                    HasCheckAutoPause = true;
                }
                Main.autoPause = true;
                Main.InGameUI.IsVisible = true;
                Main.gamePaused = true;
                ef.Parameters["Stren"].SetValue(VagueStre);
                if (VagueStre < 0.08f)
                {
                    VagueStre += 0.002f;
                }
                else
                {
                    VagueStre = 0.08f;
                }
                if (!Filters.Scene["VagueBoss"].IsActive())
                {
                    Filters.Scene.Activate("VagueBoss");
                }
                if (PauseCool > 0)
                {
                    PauseCool--;
                }
                else
                {
                    if (Main.mouseLeft)
                    {
                        CheckNewBoss = false;
                        Main.gamePaused = false;
                        Main.autoPause = CheckAutoPause;
                    }
                }
            }
            if (!CheckNewBoss)
            {
                if (!Main.dedServ)
                {
					Music = Common.MythContent.QuickMusic("DashCore");
				}
                ef.Parameters["Stren"].SetValue(VagueStre);
                if (VagueStre > 0f)
                {
                    VagueStre -= 0.002f;
                }
                else
                {
                    VagueStre = 0;
                    if (Filters.Scene["VagueBoss"].IsActive())
                    {
                        Filters.Scene.Deactivate("VagueBoss");
                    }
                }
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Texture2D tg2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternGhostKing/LanternGhostKingGlow2").Value;
            Texture2D tg3 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternGhostKing/LanternGhostKingGlow3").Value;
            Vector2 value = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector = new Vector2((float)(texture.Width / 2), (float)(texture.Height / Main.npcFrameCount[NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)tg2.Width, (float)(tg2.Height / Main.npcFrameCount[NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + NPC.gfxOffY);
            Main.spriteBatch.Draw(tg2, vector2 + new Vector2(0, 60), new Rectangle(0, U % 4 * 220, 500, 220), new Color((int)(100 * lightStre), (int)(100 * lightStre), (int)(100 * lightStre), 0), NPC.rotation, vector, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tg3, vector2 + new Vector2(0, 34), new Rectangle(0, U % 3 * 280, 500, 220), new Color((int)(325 * lightStre), (int)(325 * lightStre), (int)(325 * lightStre), 0), NPC.rotation, vector, 1f, SpriteEffects.None, 0f);

            for (int h = 0; h < 200; h++)
            {
                ACircleR[h] = 900 + (float)(Math.Sin(Main.time / 79d) * 50) + (float)(Math.Sin(h / 20d * Math.PI) * 7);
            }
            if (!Main.gamePaused)
            {
                float StanL = (900 + (float)(Math.Sin(Main.time / 79d) * 50) + 3.5f) * 0.94f;
                Vector2 v3 = player.Center - Cirposi;
                if (Math.Abs(v3.Length() - StanL) < 40)
                {
                    if (!player.dead)
                    {
                        Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Bosses.Acytaea.Projectiles.playerHit>(), NPC.damage / 4, 0, 0, 0, 0);
                        player.AddBuff(BuffID.OnFire, 300);
                    }
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx0 = new List<Vertex2D>();

            Vector2 vf = Cirposi - Main.screenPosition;

            for (int h = 0; h < 200; h++)
            {
                Color color3 = new Color((int)(255 * lightStre), (int)(255 * lightStre), (int)(255 * lightStre), 0);
                Vector2 v0 = new Vector2(0, ACircleR[h]).RotatedBy(h / 100d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, ACircleR[(h + 1) % 200]).RotatedBy((h + 1) / 100d * Math.PI + CirR0);
                Vx0.Add(new Vertex2D(vf + v0, color3, new Vector3((CirPro0 + h / 20f) % 1f, 0, 0)));
                Vx0.Add(new Vertex2D(vf + v1, color3, new Vector3(Math.Clamp((CirPro0 + (h) / 20f) % 1f + 1f / 20f, 0, 1f), 0, 0)));
                Vx0.Add(new Vertex2D(vf, color3, new Vector3(Math.Clamp((CirPro0 + (h) / 20f) % 1f + 0.5f / 20f, 0, 1f), 1, 0)));
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternFlame0").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx0.ToArray(), 0, Vx0.Count / 3);
            CirR0 += 0.001f;
            CirPro0 += 0.009f;


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx4 = new List<Vertex2D>();
            for (int h = 0; h < 200; h++)
            {
                Color color3 = new Color((int)(255 * lightStre), (int)(255 * lightStre), (int)(255 * lightStre), 0);
                Vector2 v0 = new Vector2(0, ACircleR[h]).RotatedBy(h / 100d * Math.PI + CirR4 + MathHelper.TwoPi * 1000000d);
                Vector2 v1 = new Vector2(0, ACircleR[(h + 1) % 200]).RotatedBy((h + 1) / 100d * Math.PI + CirR4 + MathHelper.TwoPi * 1000000d);
                Vx4.Add(new Vertex2D(vf + v0, color3, new Vector3((CirPro4 + h / 20f) % 1f, 0, 0)));
                Vx4.Add(new Vertex2D(vf + v1, color3, new Vector3(Math.Clamp((CirPro4 + (h) / 20f) % 1f + 1f / 20f, 0, 1f), 0, 0)));
                Vx4.Add(new Vertex2D(vf, color3, new Vector3(Math.Clamp((CirPro4 + (h) / 20f) % 1f + 0.5f / 20f, 0, 1f), 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternFlame0").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx4.ToArray(), 0, Vx4.Count / 3);
            CirR4 -= 0.004f;
            CirPro4 += 0.004f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx1 = new List<Vertex2D>();
            for (int h = 0; h < 200; h++)
            {
                Color color3 = new Color((int)(180 * lightStre), (int)(180 * lightStre), (int)(180 * lightStre), 0);
                Vector2 v0 = new Vector2(0, ACircleR[h]).RotatedBy(h / 100d * Math.PI + CirR1);
                Vector2 v1 = new Vector2(0, ACircleR[(h + 1) % 200]).RotatedBy((h + 1) / 100d * Math.PI + CirR1);
                Vx1.Add(new Vertex2D(vf + v0, color3, new Vector3((CirPro1 + h / 20f) % 1f, 0, 0)));
                Vx1.Add(new Vertex2D(vf + v1, color3, new Vector3(Math.Clamp((CirPro1 + (h) / 20f) % 1f + 1f / 20f, 0, 1f), 0, 0)));
                Vx1.Add(new Vertex2D(vf, color3, new Vector3(Math.Clamp((CirPro1 + (h) / 20f) % 1f + 0.5f / 20f, 0, 1f), 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternFlame1").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx1.ToArray(), 0, Vx1.Count / 3);
            CirR1 -= 0.0004f;
            CirPro1 += 0.002f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx2 = new List<Vertex2D>();
            for (int h = 0; h < 200; h++)
            {
                Color color3 = new Color((int)(150 * lightStre), (int)(150 * lightStre), (int)(150 * lightStre), 0);
                Vector2 v0 = new Vector2(0, ACircleR[h]).RotatedBy(h / 100d * Math.PI + CirR2 * -0.4f + MathHelper.TwoPi * 1000000d);
                Vector2 v1 = new Vector2(0, ACircleR[(h + 1) % 200]).RotatedBy((h + 1) / 100d * Math.PI + CirR2 * -0.4f + MathHelper.TwoPi * 1000000d);
                Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3((CirPro2 + h / 20f) % 1f, 0, 0)));
                Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(Math.Clamp((CirPro2 + (h) / 20f) % 1f + 1f / 20f, 0, 1f), 0, 0)));
                Vx2.Add(new Vertex2D(vf, color3, new Vector3(Math.Clamp((CirPro2 + (h) / 20f) % 1f + 0.5f / 20f, 0, 1f), 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternFlame2").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
            CirR2 += 0.002f;
            CirPro2 += 0.003f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx3 = new List<Vertex2D>();

            for (int h = 0; h < 200; h++)
            {
                Color color3 = new Color((int)(90 * lightStre), (int)(90 * lightStre), (int)(90 * lightStre), 0);
                Vector2 v0 = new Vector2(0, ACircleR[h]).RotatedBy(h / 100d * Math.PI + CirR3);
                Vector2 v1 = new Vector2(0, ACircleR[(h + 1) % 200]).RotatedBy((h + 1) / 100d * Math.PI + CirR3);
                Vx3.Add(new Vertex2D(vf + v0, color3, new Vector3((CirPro3 + h / 20f) % 1f, 0, 0)));
                Vx3.Add(new Vertex2D(vf + v1, color3, new Vector3(Math.Clamp((CirPro3 + (h) / 20f) % 1f + 1f / 20f, 0, 1f), 0, 0)));
                Vx3.Add(new Vertex2D(vf, color3, new Vector3(Math.Clamp((CirPro3 + (h) / 20f) % 1f + 0.5f / 20f, 0, 1f), 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternFlame2").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);
            CirR3 -= 0.002f;
            CirPro3 += 0.008f;
        }
        float[] ACircleR = new float[200];
        float CirR0 = 0;
        float CirPro0 = 0;
        float CirR1 = 0;
        float CirPro1 = 0;
        float CirR2 = 0;
        float CirPro2 = 0;
        float CirR3 = 0;
        float CirPro3 = 0;
        float CirR4 = 0;
        float CirPro4 = 0;
    }
}
