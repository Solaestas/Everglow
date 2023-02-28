﻿using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using static Everglow.Sources.Modules.MythModule.Common.MythUtils;
namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BookofSkulls
{
    internal class BookofSkullsArray : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.BookofSkulls)
            {
                Projectile.timeLeft = player.itemTime + 60;
                if (Timer < 30)
                {
                    Timer++;
                }
            }
            else
            {
                Timer--;
                if (Timer < 0)
                {
                    Projectile.Kill();
                }
            }
            Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;

            player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
            Vector2 vTOMouse = Main.MouseWorld - player.Center;
            player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
            Projectile.rotation = player.fullRotation;

            RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;

            
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.hide = false;
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1f, 1f, 1f, 1f));
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), new Color(0.6f, 0.55f, 0.45f, 0));
            return false;
        }

        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;

        public void DrawMagicArray(Texture2D tex, Color c0)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Water = tex;
            Color c1 = new Color(c0.R * 0.39f / 255f, c0.G * 0.39f / 255f, c0.B * 0.39f / 255f, c0.A * 0.39f / 255f);
            float Pdark = (Math.Abs(player.itemTime - player.itemTimeMax / 2f) + 0.2f) / player.itemTimeMax * Timer / 30f;
            Color c2 = new Color(1f * Pdark, 0.45f * Pdark * Pdark, 0f, 0f);

            DrawTexCircle(Timer * 1.6f, 22, c0, player.Center + RingPos - Main.screenPosition, Water, Main.timeForVisualEffects / 17);
            DrawTexCircle(Timer * 1.3f, 32, c1, player.Center + RingPos - Main.screenPosition, Water, -Main.timeForVisualEffects / 17);

            float timeRot = (float)(Main.timeForVisualEffects / 57d);
            Vector2 Point1 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Point2 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 2 / 3d + timeRot);
            Vector2 Point3 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 4 / 3d + timeRot);

            Vector2 Point4 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot + 0.3f);
            Vector2 Point5 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 3 / 3d + timeRot + 0.3f);
            Vector2 Point6 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 5 / 3d + timeRot + 0.3f);
            DrawTexLine(Point1, Point2, c2, c2, Water);
            DrawTexLine(Point2, Point3, c2, c2, Water);
            DrawTexLine(Point3, Point1, c2, c2, Water);

            DrawTexLine(Point4, Point5, c2, c2, Water);
            DrawTexLine(Point5, Point6, c2, c2, Water);
            DrawTexLine(Point6, Point4, c2, c2, Water);
        }

        
        public static void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
        {
            float Wid = 6f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                float Value0 = (float)(Main.timeForVisualEffects / 291 + 20) % 1f;
                float Value1 = (float)(Main.timeForVisualEffects / 291 + 20.05) % 1f;

                if (Value1 < Value0)
                {
                    float D0 = 1 - Value0;
                    Vector2 Delta = EndPos - StartPos;
                    vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(1, 1, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 1, 0)));

                    vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 1, 0)));

                    continue;
                }
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

        public void DrawWarp(VFXBatch spriteBatch)
        {
            Player player = Main.player[Projectile.owner];
            DrawTexCircle(spriteBatch,Timer * 1.2f, 52, new Color(64, 70, 255, 0), player.Center + RingPos - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), Main.timeForVisualEffects / 17);
        }
    }
}