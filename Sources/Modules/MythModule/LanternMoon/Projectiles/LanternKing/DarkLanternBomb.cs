using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Core.VFX;
using static Everglow.Sources.Modules.MythModule.Common.MythUtils;
using Terraria.DataStructures;
using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing.VFXs;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    public class DarkLanternBomb : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 900;
        }
        public override void OnSpawn(IEntitySource source)
        {
            float MinDis = 3000;
            foreach(NPC npc in Main.npc)
            {
                if(npc.active)
                {
                    if(npc.type == ModContent.NPCType<NPCs.LanternGhostKing.LanternGhostKing>())
                    {
                        float Dis = (npc.Center - Projectile.Center).Length();
                        if (Dis < MinDis)
                        {
                            MinDis = Dis;
                        }
                    }
                }
            }
            Projectile.timeLeft = (int)(900 + MinDis * 0.3);
        }
        public override void AI()
        {
            Projectile.ai[1] += 0.01f;
            if (Projectile.ai[1] % 0.15 == 0)
            {
                if (Projectile.frame < 2)
                {
                    Projectile.frame++;
                }
                else
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.velocity *= 0f;
            if (Projectile.timeLeft < 90)
            {
                Projectile.scale += 0.05f;
            }
            if (Projectile.timeLeft < 3)
            {
                Projectile.scale += 0.05f;
                Projectile.hostile = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D MainTex = (Texture2D)ModContent.Request<Texture2D>(Texture);
            float timeValue = (900 - Projectile.timeLeft) / 900f;
            float ColorValue = timeValue * (float)(Math.Sin(Projectile.ai[1]) + 2) / 3f;
            Color colorT = new Color(ColorValue, ColorValue, ColorValue, 0.5f * ColorValue);


            Texture2D textureLight = MythContent.QuickTexture("LanternMoon/Projectiles/LanternKing/LightEffect");
            Texture2D flameRing = MythContent.QuickTexture("UIimages/VisualTextures/CoreFlame");
            Main.spriteBatch.Draw(textureLight, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.05f, 0f, 0) * 0.4f, 0, textureLight.Size() * 0.5f, new Vector2(1, (900 - Projectile.timeLeft) / 260f), SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(textureLight, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.05f, 0f, 0) * 0.4f, MathF.PI * 0.5f, textureLight.Size() * 0.5f, new Vector2(1, (900 - Projectile.timeLeft) / 180f), SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(MythContent.QuickTexture("LanternMoon/Projectiles/LanternKing/LanternFire"), Projectile.Center - Main.screenPosition, new Rectangle(0, 30 * Projectile.frame, 20, 30), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
            for (float k = 0; k < timeValue; k += 0.5f)
            {
                if (k > 0.5)
                {
                    Main.spriteBatch.Draw(MainTex, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), Projectile.rotation, MainTex.Size() / 2f, Projectile.scale, SpriteEffects.None, 1f);
                    
                }
                else
                {
                    Main.spriteBatch.Draw(MainTex, Projectile.Center - Main.screenPosition, null, colorT, Projectile.rotation, MainTex.Size() / 2f, Projectile.scale, SpriteEffects.None, 1f);
                }
            }
            float timeValuex2 = Math.Min(timeValue * 2, 1);
            float value0 = (float)(Math.Sin(2400d / (Projectile.timeLeft + 35)) * 0.75f - 0.15f);
            value0 = Math.Max(0.05f, value0);
            value0 += 1 - timeValuex2;
            float floatValue = 1 + MathF.Sin((float)(Main.timeForVisualEffects - Projectile.timeLeft) * 0.05f) * 0.15f;
            DrawTexCircle(132 * timeValuex2, 22 * timeValuex2 + 22, new Color(0.75f, 0.45f, 0.45f, 0) * value0, Projectile.Center - Main.screenPosition, flameRing, Main.time / 17);
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
            float value = 130;
            float colorV = 0.9f * (1 - value);
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
            float width = 60;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft;
            }
            float timeValue = (900 - Projectile.timeLeft) / 900f;
            float timeValuex2 = Math.Min(timeValue * 2, 1);

            DrawTexCircle_VFXBatch(sb, value * timeValuex2, width * timeValuex2, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
        }
        public void GenerateVFXExpolode(int Frequency, float mulVelocity = 1f)
        {
            for (int g = 0; g < Frequency * 3; g++)
            {
                FlameDust cf = new FlameDust
                {
                    velocity = new Vector2(0, Main.rand.NextFloat(4.65f, 5.5f)).RotatedByRandom(6.283) * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center + new Vector2(Main.rand.NextFloat(-56f, 56f), 0).RotatedByRandom(6.283),
                    maxTime = Main.rand.Next(16, 36),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.18f, 0.18f), Main.rand.NextFloat(8f, 12f) }
                };
                VFXManager.Add(cf);
            }
            for (int g = 0; g < Frequency; g++)
            {
                FlameDust cf = new FlameDust
                {
                    velocity = new Vector2(0, Main.rand.NextFloat(6.65f, 10.5f)).RotatedByRandom(6.283) * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center,
                    maxTime = Main.rand.Next(12, 30),
                    ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.4f, 0.4f), Main.rand.NextFloat(22f, 32f) }
                };
                VFXManager.Add(cf);
            }
        }
        public override void Kill(int timeLeft)
        {
            ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
            Gsplayer.FlyCamPosition = new Vector2(0, 33).RotatedByRandom(6.283);

            GenerateVFXExpolode(24, 2.2f);

            for (int d = 0; d < 70; d++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Torch, 0, 0, 0, default, 0.6f);
                d0.velocity = new Vector2(0, Main.rand.NextFloat(3.65f, 7.5f)).RotatedByRandom(6.283);
            }

            Vector2 GorePos;
            GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
            int gra0 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore3").Type, 1f);
            Main.gore[gra0].timeLeft = Main.rand.Next(300, 600);
            GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
            int gra1 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore4").Type, 1f);
            Main.gore[gra1].timeLeft = Main.rand.Next(300, 600);
            GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
            int gra2 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore5").Type, 1f);
            Main.gore[gra2].timeLeft = Main.rand.Next(300, 600);
            GorePos = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
            int gra3 = Gore.NewGore(null, Projectile.Center, GorePos, ModContent.Find<ModGore>("Everglow/FloatLanternGore6").Type, 1f);
            Main.gore[gra3].timeLeft = Main.rand.Next(300, 600);

            int HitType = ModContent.ProjectileType<StrikePlayer>();
            foreach (Player player in Main.player)
            {
                if (player != null)
                {
                    float Dis = (player.Center - Projectile.Center).Length();
                    if(Dis < 125)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.Zero, HitType, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact.WithVolumeScale(0.4f), Projectile.Center);
        }
    }
}