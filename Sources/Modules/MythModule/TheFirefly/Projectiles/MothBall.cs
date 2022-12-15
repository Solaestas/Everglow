using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class MothBall : ModProjectile
    {
        private float r = 0;
        private Vector2 v0;
        private int Fra = 0;
        private int FraX = 0;
        private int FraY = 0;
        private Vector2[] vB = new Vector2[15];
        private Vector2[] vloB = new Vector2[15];
        private int[] yB = new int[15];
        private float Stre2 = 1;
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("幻蝶泡");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            int p = Player.FindClosest(Projectile.Center, 1000, 1000);

            if (p is >= 0 and < 255)
            {
                float speed = MathHelper.Clamp((300 - Projectile.timeLeft) * 0.1f, 0, 30);
                speed *= MathHelper.Clamp(Vector2.Distance(Projectile.Center, Main.player[p].Center) / 300, 1, 2f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.player[p].Center) * speed, 0.015f);
            }
            if (Projectile.timeLeft < 50)
            {
                Projectile.velocity *= 0.95f;
            }

            #region Origin

            if (Stre2 > 0)
            {
                Stre2 -= 0.005f;
            }
            if (Projectile.timeLeft == 300)
            {
                v0 = Projectile.Center;
                for (int g = 0; g < 15; g++)
                {
                    vB[g] = new Vector2(0, Main.rand.Next(0, 35)).RotatedByRandom(6.283);
                    vloB[g] = new Vector2(0, Main.rand.NextFloat(0.4f, 2.8f)).RotatedByRandom(6.283);
                    yB[g] = Main.rand.Next(4) * 36;
                }
            }
            if (Projectile.timeLeft > 240)
            {
                r += 1f;
            }
            if (Projectile.timeLeft is <= 240 and >= 60)
            {
                r = 60 + (float)(10 * Math.Sin((Projectile.timeLeft - 60) / 60d * Math.PI));
            }
            if (Projectile.timeLeft < 60 && r > 0.5f)
            {
                r -= 1f;
            }
            int Dx = (int)(r * 1.5f);
            int Dy = (int)(r * 1.5f);
            Fra = ((600 - Projectile.timeLeft) / 3) % 30;
            FraX = (Fra % 6) * 270;
            FraY = (Fra / 6) * 290;
            if (v0 != Vector2.Zero)
            {
                // Projectile.position = v0 - new Vector2(Dx, Dy) / 2f;
            }

            Projectile.width = Dx;
            Projectile.height = Dy;
            for (int g = 0; g < 15; g++)
            {
                if (yB[g] > 102)
                {
                    yB[g] = 0;
                }
                if (Projectile.timeLeft % 10 == 0)
                {
                    yB[g] += 36;
                }
                vB[g] += vloB[g];
                if (vB[g].Length() > 80)
                {
                    double a1 = Math.Atan2(vloB[g].Y, vloB[g].X);
                    double a2 = Math.Atan2(vB[g].Y, vB[g].X);
                    double a3 = a2 - a1;
                    vloB[g] = -vloB[g].RotatedBy(a3 * 2);
                    vB[g] += vloB[g];
                }
            }

            #endregion Origin
        }

        public override void Kill(int timeLeft)
        {
            /*震屏
            MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Shake = 6;
            float Str = 1;

            mplayer.ShakeStrength = Str;*/
            ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
            mplayer.FlyCamPosition = new Vector2(0, 48).RotatedByRandom(6.283);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            for (int h = 0; h < 120; h += 3)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.PureBlue>(), 0, 0, 0, default(Color), 15f * Main.rand.NextFloat(0.7f, 2.9f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3 * 6;
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 4.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 27.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 36; y++)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var npc = NPC.NewNPCDirect(Projectile.GetSource_FromAI(), (int)(Projectile.Center.X + vB[y].X), (int)(Projectile.Center.Y + vB[y].Y), ModContent.NPCType<NPCs.Bosses.Butterfly>());
                        npc.velocity = Main.rand.NextVector2Unit() * Main.rand.Next(4, 12);
                        npc.netUpdate2 = true;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        var npc = NPC.NewNPCDirect(Projectile.GetSource_FromAI(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<NPCs.Bosses.Butterfly>());
                        npc.velocity = Main.rand.NextVector2Unit() * Main.rand.Next(2, 5);
                        npc.netUpdate2 = true;
                    }
                }

                int player = Player.FindClosest(Projectile.Center, 1000, 1000);
                float X = 0;
                if (player is >= 0 and < 255)
                {
                    X = Projectile.DirectionTo(Main.player[player].Center).ToRotation();
                }
                for (int h = 0; h < 36; h++)
                {
                    if (h % 6 < 3)
                    {
                        Vector2 v = new Vector2(0, 12f).RotatedBy(h * MathHelper.TwoPi / 36f + X);
                        Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center + v, v, ModContent.ProjectileType<BlueMissil>(), Projectile.damage, 0f, Main.myPlayer, 2);
                    }
                }
            }

            //Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.CorruptMoth.FruitBomb>(), 0, 0f, Main.myPlayer, 1);
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(FraX, 10 + FraY, 270, 270), new Color(1f, 1f, 1f, 0), Projectile.rotation, new Vector2(135f, 135f), r / 60f, SpriteEffects.None, 0f);
            for (int g = 0; g < 15; g++)
            {
                SpriteEffects eff = SpriteEffects.None;
                if (vloB[g].X > 0)
                {
                    eff = SpriteEffects.FlipHorizontally;
                }
                Main.spriteBatch.Draw(Common.MythContent.QuickTexture("TheFirefly/Projectiles/ButterflyDream"), Projectile.Center + vB[g] * r / 60f - Main.screenPosition, new Rectangle(0, yB[g], 36, 34), new Color(0.2f, 0.5f, 1f, 0), Projectile.rotation, new Vector2(18f, 17f), r / 60f, eff, 0f);
            }
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/CorruptLight");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * Stre2), (int)(255 * Stre2), (int)(255 * Stre2), 0), Projectile.rotation, new Vector2(168f, 168f), Projectile.scale * 2 * r / 60f, SpriteEffects.None, 0);
            return false;
        }
    }
}