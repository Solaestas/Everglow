using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MEACModule.Items;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.GameContent;


namespace Everglow.Sources.Modules.IIIDModule.Projectiles.NonIIIDProj.PlanetBefallArray
{
    public class PlanetBefallArray : ModProjectile ,IBloomProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.HeldItem.type ==ModContent.ItemType<VortexVanquisherItem>())
            {
                Projectile.timeLeft = player.itemTime + 60;
                if (Timer < 20)
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

    
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        { 
            Projectile.hide = false;
            DrawMagicArray(Color.Black);
            return false;
        }
        public void DrawBloom()
        {
            Color c = Color.White;
            PreDraw(ref c);
        }
        internal int Timer = 0;

        public void DrawMagicArray(Color c0)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D PlantBeFallIn = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/NonIIIDProj/PlanetBefallArray/PlantBeFallIn").Value;
            Texture2D PlantBeFallOut = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/NonIIIDProj/PlanetBefallArray/PlantBeFallOut").Value;
            Texture2D GeoElement = ModContent.Request<Texture2D>("Everglow/Sources/Modules/IIIDModule/Projectiles/NonIIIDProj/PlanetBefallArray/GeoElement").Value;
            Vector2 p = Projectile.Center - Main.screenPosition - new Vector2(GeoElement.Width, GeoElement.Height) / 8;
            Main.spriteBatch.Draw(GeoElement,new Rectangle((int)p.X, (int)p.Y, GeoElement.Width/4, GeoElement.Height/4) ,Color.White);
            DrawTexCircle(Timer* 30f, 100, Color.Gold, Projectile.Center - Main.screenPosition, PlantBeFallOut, Main.timeForVisualEffects / 1500 + MathHelper.PiOver4);
            List<Vertex2D> In = new List<Vertex2D>();

            Vector2 Point1 = Projectile.Center  - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI*0  - Main.timeForVisualEffects / 500);
            Vector2 Point2 = Projectile.Center  - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 1 / 2d - Main.timeForVisualEffects / 500);
            Vector2 Point3 = Projectile.Center  - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 2 / 2d - Main.timeForVisualEffects / 500);
            Vector2 Point4 = Projectile.Center  - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 3 / 2d - Main.timeForVisualEffects / 500);
            In.Add(new Vertex2D(Point1, Color.Gold, new Vector3(0, 0, 0)));
            In.Add(new Vertex2D(Point2, Color.Gold, new Vector3(1, 0, 0)));
            In.Add(new Vertex2D(Point4, Color.Gold, new Vector3(0, 1, 0)));
            In.Add(new Vertex2D(Point3, Color.Gold, new Vector3(1, 1, 0)));
            if (In.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = PlantBeFallIn;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, In.ToArray(), 0, In.Count - 2);
            }
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

        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex, float AddValue = 0)
        {
            float Wid =282.842f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();
            float Value0 = (float)( 20 + AddValue) % 1f;
            float Value1 = (float)(20.1 + AddValue) % 1f;
            if (Value1 < Value0)
            {
                float D0 = 1 - Value0;
                Vector2 Delta = EndPos - StartPos;

                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width, color2, new Vector3(1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width, color2, new Vector3(1, 1, 0)));

                vertex2Ds.Add(new Vertex2D(StartPos + Width, color1, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width, color1, new Vector3(0, 1, 0)));



                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width, color1, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width, color1, new Vector3(0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width, color2, new Vector3(1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width, color2, new Vector3(1, 1, 0)));
            }
            else
            {
                vertex2Ds.Add(new Vertex2D(StartPos + Width, color1, new Vector3(0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width, color1, new Vector3(0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width, color2, new Vector3(1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width, color2, new Vector3(1, 1, 0)));
            }

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);
        }
    }
}
