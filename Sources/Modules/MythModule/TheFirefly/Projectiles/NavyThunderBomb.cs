using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using static Everglow.Sources.Modules.MythModule.Common.MythUtils;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.Audio;
using Everglow.Sources.Commons.Core.VFX;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class NavyThunderBomb : ModProjectile, IWarpProjectile
    {
        private float r = 20;
        private int Fra = 0;
        private int FraX = 0;
        private int FraY = 0;
        private float Stre2 = 1;
        public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/MothBall";
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Navy Thunder Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            if (Stre2 > 0)
            {
                Stre2 -= 0.005f;
            }
            if (Projectile.timeLeft > 260)
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
            Fra = ((600 - Projectile.timeLeft) / 3) % 30;
            FraX = (Fra % 6) * 270;
            FraY = (Fra / 6) * 290;
            if (Projectile.timeLeft < 10)
            {
                Projectile.friendly = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            /*震屏
            MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Shake = 6;
            float Str = 1;

            mplayer.ShakeStrength = Str;*/
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            Player player = Main.player[Projectile.owner];
            ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
            Gsplayer.FlyCamPosition = new Vector2(0, 150).RotatedByRandom(6.283);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BombShakeWave>(), 0, 0, Projectile.owner, 0.4f, 2f);
            float k1 = Math.Clamp(Projectile.velocity.Length(), 1, 3);
            float k2 = Math.Clamp(Projectile.velocity.Length(), 6, 10);
            float k0 = 1f / (Projectile.ai[0] + 2) * 2 * k2;
            float X = 0;
            for (int h = 0; h < 18; h++)
            {
                if (h % 3 < 1)
                {
                    Vector2 v = new Vector2(0, 12f).RotatedBy(h * MathHelper.TwoPi / 18f + X);
                    if (!Main.hardMode) // Until the DownedSystem is reimplemented and decided on, Main.hardMode is used. This can change
                    {
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v, v, ModContent.ProjectileType<BlueMissilFriendly>(), (int)(Projectile.damage * 0.35f), 0f, Projectile.owner);
                    }
                    else
                    {
                        Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v, v, ModContent.ProjectileType<BlueMissilFriendly>(), (int)(Projectile.damage * 0.55f), 0f, Projectile.owner);
                    }
                }
            }
            for (int j = 0; j < 8 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
                int dust0 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * Projectile.scale * 0.4f * k0);
                Main.dust[dust0].noGravity = true;
            }
            for (int j = 0; j < 16 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
                int dust1 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(3.7f, 5.1f) * k0);
                Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
            }
            for (int j = 0; j < 16 * k0; j++)
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * k1;
                int dust1 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<MothSmog>(), v0.X, v0.Y, 100, default, Main.rand.NextFloat(3.7f, 5.1f) * k0);
                Main.dust[dust1].alpha = (int)(Main.dust[dust1].scale * 50 / k0);
                Main.dust[dust1].rotation = Main.rand.NextFloat(0, 6.283f);
            }
            foreach (NPC target in Main.npc)
            {
                float Dis = (target.Center - Projectile.Center).Length();

                if (Dis < 150)
                {
                    if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy() && target.active)
                    {
                        bool crit = Main.rand.NextBool(33, 100);
                        target.StrikeNPC(Projectile.damage, 2f, 1, crit);

                        player.addDPS(Math.Max(0, target.defDamage));
                    }
                }
            }

            for (int h = 0; h < 120; h += 3)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.PureBlue>(), 0, 0, 0, default, 15f * Main.rand.NextFloat(0.7f, 2.9f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3 * 6;
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 4.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 27.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 36; y++)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        

        public static void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
        {
            float Wid = 6f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                float Value0 = (float)(Main.time / 291d + 20) % 1f;
                float Value1 = (float)(Main.time / 291d + 20.03) % 1f;
                vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));
            }

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Water = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/ElecLine");
            Texture2D WaterS = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade");
            float value0 = (float)(Math.Sin(800d / (Projectile.timeLeft + 35)) * 0.75f + 0.25f) * (300 - Projectile.timeLeft) / 300f;
            value0 = Math.Max(0, value0);
            //DrawTexCircle(132, 22, new Color(value0, value0, value0, value0), Projectile.Center - Main.screenPosition, WaterS, Main.time / 17);
            DrawTexCircle(122, 42, new Color(0.33f * value0, 0.33f * value0, 0.33f * value0, 0.33f * value0), Projectile.Center - Main.screenPosition, WaterS, -Main.time / 17);
            DrawTexCircle(132, 32, new Color(0, 0.45f * value0, 1f * value0, 0), Projectile.Center - Main.screenPosition, Water, Main.time / 17);
            DrawTexCircle(122, 42, new Color(0, 0.15f * value0, 0.33f * value0, 0), Projectile.Center - Main.screenPosition, Water, -Main.time / 17);

            Texture2D Dark = Common.MythContent.QuickTexture("TheFirefly/Projectiles/BlueFlameDark");
            Main.spriteBatch.Draw(Dark, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 1f), Projectile.rotation, new Vector2(68f, 68f), r / 60f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(FraX, FraY + 10, 270, 270), new Color(1f, 1f, 1f, 0), Projectile.rotation, new Vector2(135f, 135f), r / 420f, SpriteEffects.None, 0f);
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/CorruptLight");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(Stre2, Stre2, Stre2, 0), Projectile.rotation, new Vector2(168f, 168f), Projectile.scale * r / 210f, SpriteEffects.None, 0);
            if (Projectile.timeLeft <= 60)
            {
                float k3 = (60 - Projectile.timeLeft) / 40f;
                k3 *= k3;
                Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(Stre2, 1f, 1f, 0), Projectile.rotation, new Vector2(168f, 168f), k3, SpriteEffects.None, 0);
            }
            return false;
        }

        private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();

            for (int h = 0; h < radious / 2; h += 1)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 2)
            {
                spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
            }
        }
        public void DrawWarp(VFXBatch sb)
        {
            float value = (300 - Projectile.timeLeft) / (300f);
            value = MathF.Sqrt(value);
            float colorV = 0.9f * (1 - value);
            if (Projectile.ai[0] >= 10)
            {
                colorV *= 10;
            }
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
            float width = 60;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft;
            }

            DrawTexCircle_VFXBatch(sb, value * 27 * Projectile.ai[0], width * 2, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
    }
}