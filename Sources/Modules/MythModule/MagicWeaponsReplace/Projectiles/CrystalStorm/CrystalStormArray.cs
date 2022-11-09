using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    internal class CrystalStormArray : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.CrystalStorm)
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
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CrystalDarkline"), new Color(0.6f, 0.6f, 0.6f, 0.6f));

            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/ElecLine"), new Color(15, 100, 255, 0));


            return false;
        }

        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;

        public void DrawMagicArray(Texture2D tex, Color c0)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Water = tex;
            Texture2D Crystalline = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Crystalline");
            Texture2D CrystalLight = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CrystalLight");
            if(tex == MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CrystalDarkline"))
            {
                Crystalline = tex;
                CrystalLight = tex;
            }
            Color c1 = new Color(155,0,225,0);
            DrawTexSquire(Timer * 2.88f, 11, c0, player.Center + RingPos - Main.screenPosition, Water, -Main.timeForVisualEffects / 300);
            DrawTexSquire(Timer * 3.1f, 24, c0, player.Center + RingPos - Main.screenPosition, Crystalline, -Main.timeForVisualEffects / 300);


            DrawTexSquire(Timer * 3.18f, 11, c0, player.Center + RingPos - Main.screenPosition, Water, -Main.timeForVisualEffects / 300 + MathHelper.PiOver4);
            DrawTexSquire(Timer * 3.3f, 24, c0, player.Center + RingPos - Main.screenPosition, Crystalline, -Main.timeForVisualEffects / 300 + MathHelper.PiOver4);

            DrawTexCircle(Timer * 2.67f, 75, c0, player.Center + RingPos - Main.screenPosition, Crystalline, Main.timeForVisualEffects / 400 + MathHelper.PiOver4);
            DrawTexCircle(Timer * 1.3f, 30, c1, player.Center + RingPos - Main.screenPosition, Crystalline, Main.timeForVisualEffects / 400 + MathHelper.PiOver4);

            float timeRot = (float)(Main.timeForVisualEffects / 240d);
            Vector2 Point1 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Point2 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 1 / 4d + timeRot);
            Vector2 Point3 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 2 / 4d + timeRot);
            Vector2 Point4 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 3 / 4d + timeRot);
            Vector2 Point5 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 4 / 4d + timeRot);
            Vector2 Point6 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 5 / 4d + timeRot);
            Vector2 Point7 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 6 / 4d + timeRot);
            Vector2 Point8 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 7 / 4d + timeRot);

            Vector2 Point1_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 0 + timeRot + 0.2);
            Vector2 Point2_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 1 / 4d + timeRot + 0.2);
            Vector2 Point3_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 2 / 4d + timeRot + 0.2);
            Vector2 Point4_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 3 / 4d + timeRot + 0.2);
            Vector2 Point5_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 4 / 4d + timeRot + 0.2);
            Vector2 Point6_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 5 / 4d + timeRot + 0.2);
            Vector2 Point7_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 6 / 4d + timeRot + 0.2);
            Vector2 Point8_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.4f).RotatedBy(Math.PI * 7 / 4d + timeRot + 0.2);

            Vector2 InnerPoint1 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 0 - timeRot);
            Vector2 InnerPoint2 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 1 / 4d - timeRot);
            Vector2 InnerPoint3 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 2 / 4d - timeRot);
            Vector2 InnerPoint4 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 3 / 4d - timeRot);
            Vector2 InnerPoint5 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 4 / 4d - timeRot);
            Vector2 InnerPoint6 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 5 / 4d - timeRot);
            Vector2 InnerPoint7 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 6 / 4d - timeRot);
            Vector2 InnerPoint8 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 7 / 4d - timeRot);

            Vector2 InnerPoint1_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 0 - timeRot + 0.2);
            Vector2 InnerPoint2_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 1 / 4d - timeRot + 0.2);
            Vector2 InnerPoint3_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 2 / 4d - timeRot + 0.2);
            Vector2 InnerPoint4_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 3 / 4d - timeRot + 0.2);
            Vector2 InnerPoint5_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 4 / 4d - timeRot + 0.2);
            Vector2 InnerPoint6_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 5 / 4d - timeRot + 0.2);
            Vector2 InnerPoint7_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 6 / 4d - timeRot + 0.2);
            Vector2 InnerPoint8_ = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 0.8f).RotatedBy(Math.PI * 7 / 4d - timeRot + 0.2);

            DrawTexLine(Point1_, Point3, c0, c0, CrystalLight, 0.1f);
            DrawTexLine(Point2_, Point4, c0, c0, CrystalLight, 0.4f);
            DrawTexLine(Point3_, Point5, c0, c0, CrystalLight, 0.2f);
            DrawTexLine(Point4_, Point6, c0, c0, CrystalLight, 0.8f);
            DrawTexLine(Point5_, Point7, c0, c0, CrystalLight, 0.5f);
            DrawTexLine(Point6_, Point8, c0, c0, CrystalLight, 0.7f);
            DrawTexLine(Point7_, Point1, c0, c0, CrystalLight, 0.3f);
            DrawTexLine(Point8_, Point2, c0, c0, CrystalLight, 0.1f);

            DrawTexLine(Point1, Point4, c0, c0, CrystalLight, 0.6f);
            DrawTexLine(Point8, Point5, c0, c0, CrystalLight, 0.9f);

            DrawTexLine(Point2, Point7, c0, c0, CrystalLight, 0.1f);
            DrawTexLine(Point3, Point6, c0, c0, CrystalLight, 0.4f);

            DrawTexLine(Point1_, Point4, c0, c0, CrystalLight, 0.2f);
            DrawTexLine(Point8_, Point5, c0, c0, CrystalLight, 0.8f);

            DrawTexLine(Point2_, Point7, c0, c0, CrystalLight, 0.5f);
            DrawTexLine(Point3_, Point6, c0, c0, CrystalLight, 0.7f);


            DrawTexLine(InnerPoint1_, InnerPoint3, c1, c1, CrystalLight, 0.1f);
            DrawTexLine(InnerPoint2_, InnerPoint4, c1, c1, CrystalLight, 0.4f);
            DrawTexLine(InnerPoint3_, InnerPoint5, c1, c1, CrystalLight, 0.2f);
            DrawTexLine(InnerPoint4_, InnerPoint6, c1, c1, CrystalLight, 0.8f);
            DrawTexLine(InnerPoint5_, InnerPoint7, c1, c1, CrystalLight, 0.5f);
            DrawTexLine(InnerPoint6_, InnerPoint8, c1, c1, CrystalLight, 0.7f);
            DrawTexLine(InnerPoint7_, InnerPoint1, c1, c1, CrystalLight, 0.3f);
            DrawTexLine(InnerPoint8_, InnerPoint2, c1, c1, CrystalLight, 0.1f);

            DrawTexLine(InnerPoint1, InnerPoint4, c1, c1, CrystalLight, 0.6f);
            DrawTexLine(InnerPoint8, InnerPoint5, c1, c1, CrystalLight, 0.9f);

            DrawTexLine(InnerPoint2, InnerPoint7, c1, c1, CrystalLight, 0.1f);
            DrawTexLine(InnerPoint3, InnerPoint6, c1, c1, CrystalLight, 0.4f);

            DrawTexLine(InnerPoint1_, InnerPoint4, c1, c1, CrystalLight, 0.2f);
            DrawTexLine(InnerPoint8_, InnerPoint5, c1, c1, CrystalLight, 0.8f);

            DrawTexLine(InnerPoint2_, InnerPoint7, c1, c1, CrystalLight, 0.5f);
            DrawTexLine(InnerPoint3_, InnerPoint6, c1, c1, CrystalLight, 0.7f);

        }

        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        private static void DrawTexSquire(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < 5; h++)
            {
                float Value0 = (float)(h / 4f + Main.timeForVisualEffects / 191d) % 1f;
                float Value1 = (float)((h + 1) / 4f + Main.timeForVisualEffects / 191d) % 1f;

                circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(Math.PI / 2 * h + addRot), color, new Vector3(Value0, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(Math.PI / 2 * h + addRot), color, new Vector3(Value0, 0, 0)));

                if (Value1 < Value0)
                {
                    float D0 = (1 - Value0) * 4;
                    Vector2 Delta = new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(Math.PI / 2 * (h + 1) + addRot) - new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(Math.PI / 2 * h + addRot);
                    Vector2 DeltaWidth = new Vector2(0, radious).RotatedBy(Math.PI / 2 * (h + 1) + addRot) - new Vector2(0, radious).RotatedBy(Math.PI / 2 * h + addRot);

                    if(h < 4)
                    {
                        circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(Math.PI / 2 * h + addRot) + Delta * D0, color, new Vector3(1, 1, 0)));
                        circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(Math.PI / 2 * h + addRot) + DeltaWidth * D0, color, new Vector3(1, 0, 0)));

                        circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(Math.PI / 2 * h + addRot) + Delta * D0, color, new Vector3(0, 1, 0)));
                        circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(Math.PI / 2 * h + addRot) + DeltaWidth * D0, color, new Vector3(0, 0, 0)));
                    }

                }
            }
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }

        }

        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex, float AddValue = 0)
        {
            float Wid = 24f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();
            float Value0 = (float)(Main.timeForVisualEffects / 291d + 20 + AddValue) % 1f;
            float Value1 = (float)(Main.timeForVisualEffects / 291d + 20.1 + AddValue) % 1f;
            if (Value1 < Value0)
            {
                float D0 = 1 - Value0;
                Vector2 Delta = EndPos - StartPos;

                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width, color2, new Vector3(1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width, color2, new Vector3(1, 1, 0)));

                vertex2Ds.Add(new Vertex2D(StartPos + Width, color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width, color1, new Vector3(Value0, 1, 0)));



                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width, color1, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width, color1, new Vector3(0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width, color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width, color2, new Vector3(Value1, 1, 0)));
            }
            else
            {
                vertex2Ds.Add(new Vertex2D(StartPos + Width, color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width, color1, new Vector3(Value0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width, color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width, color2, new Vector3(Value1, 1, 0)));
            }

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);
        }
    }
}