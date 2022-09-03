using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    internal class CrystalStormArray : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.time / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.CrystalStorm)
            {
                Projectile.timeLeft = player.itemTime + 60;
                if(Timer < 30)
                {
                    Timer++;
                }
            }
            else
            {
                Timer--;
                if(Timer < 0)
                {
                    Projectile.Kill();
                }
            }
            Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;

            player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.time / 18d) * 0.6 + 1.2) * -player.direction);
            Vector2 vTOMouse = Main.MouseWorld - player.Center;
            player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
            Projectile.rotation = player.fullRotation;

            RingPos = RingPos * 0.9f + new Vector2(-24 * player.direction, -36 * player.gravDir) * 0.1f;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
           behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.hide = false;
            //DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Lightline"), new Color(1f, 1f, 1f, 1f));
            //DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1f, 1f, 1f, 1f));
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Lightline"), new Color(0.1f, 0f, 0.9f, 0));
            return false;
        }
        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;
        public void DrawMagicArray(Texture2D tex, Color c0)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Line = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Lightline");
            Texture2D LineV = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/LightlineV");
            Texture2D Crystal = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/GreyCrystal");
            Color c1 = new Color(c0.R * 0.39f / 255f, c0.G * 0.39f / 255f, c0.B * 0.39f / 255f, c0.A * 0.39f / 255f);
            float Size = 30f;
            float timeRot = (float)(Math.Sin(Main.time / 12d) * 0.05 + 0);
            float WaveValue1 = (float)(Math.Sin(Main.time / 12d) * 0.05 + 1);
            float WaveValue2 = (float)(Math.Cos(Main.time / 12d) * 0.08 + 1);
            Vector2 Crystal1 = new Vector2((float)(1 - Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue1).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Crystal2 = new Vector2((float)(1 - Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue1).RotatedBy(Math.PI * 0.5 + timeRot);
            Vector2 Crystal3 = new Vector2((float)(1 - Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue1).RotatedBy(Math.PI * 1 + timeRot);
            Vector2 Crystal4 = new Vector2((float)(1 - Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue1).RotatedBy(Math.PI * 1.5 + timeRot);
            Vector2 Crystal5 = new Vector2((float)(3 + Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue2).RotatedBy(Math.PI * 0.25 - timeRot * 0.73f);
            Vector2 Crystal6 = new Vector2((float)(3 + Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue2).RotatedBy(Math.PI * 0.75 - timeRot * 0.73f);
            Vector2 Crystal7 = new Vector2((float)(3 + Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue2).RotatedBy(Math.PI * 1.25 - timeRot * 0.73f);
            Vector2 Crystal8 = new Vector2((float)(3 + Math.Cos(Timer / 30d * Math.PI)) * Size * WaveValue2).RotatedBy(Math.PI * 1.75 - timeRot * 0.73f);

            Vector2 Normal1 = new Vector2(1).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Normal2 = new Vector2(1).RotatedBy(Math.PI * 0.5 + timeRot);
            Vector2 Normal3 = new Vector2(1).RotatedBy(Math.PI * 1 + timeRot);
            Vector2 Normal4 = new Vector2(1).RotatedBy(Math.PI * 1.5 + timeRot);
            Vector2 Normal5 = new Vector2(1).RotatedBy(Math.PI * 0.25 - timeRot * 0.73f);
            Vector2 Normal6 = new Vector2(1).RotatedBy(Math.PI * 0.75 - timeRot * 0.73f);
            Vector2 Normal7 = new Vector2(1).RotatedBy(Math.PI * 1.25 - timeRot * 0.73f);
            Vector2 Normal8 = new Vector2(1).RotatedBy(Math.PI * 1.75 - timeRot * 0.73f);

            Vector2 Point1 = player.Center + RingPos - Main.screenPosition + Crystal1;
            Vector2 Point2 = player.Center + RingPos - Main.screenPosition + Crystal2;
            Vector2 Point3 = player.Center + RingPos - Main.screenPosition + Crystal3;
            Vector2 Point4 = player.Center + RingPos - Main.screenPosition + Crystal4;
            Vector2 Point5 = player.Center + RingPos - Main.screenPosition + Crystal5;
            Vector2 Point6 = player.Center + RingPos - Main.screenPosition + Crystal6;
            Vector2 Point7 = player.Center + RingPos - Main.screenPosition + Crystal7;
            Vector2 Point8 = player.Center + RingPos - Main.screenPosition + Crystal8;

            DrawTexLine(Point1 - Normal1 * 20, Point1 + Normal1 * 20, c0, c0, Crystal, Math.Min(Timer, Size / 2f));
            DrawTexLine(Point2 - Normal2 * 20, Point2 + Normal2 * 20, c0, c0, Crystal, Math.Min(Timer, Size / 2f));
            DrawTexLine(Point3 - Normal3 * 20, Point3 + Normal3 * 20, c0, c0, Crystal, Math.Min(Timer, Size / 2f));
            DrawTexLine(Point4 - Normal4 * 20, Point4 + Normal4 * 20, c0, c0, Crystal, Math.Min(Timer, Size / 2f));

            DrawTexLine(Point5 - Normal5 * 12, Point5 + Normal5 * 12, c0, c0, Crystal, Math.Min(Timer / 2f, Size * 0.3f));
            DrawTexLine(Point6 - Normal6 * 12, Point6 + Normal6 * 12, c0, c0, Crystal, Math.Min(Timer / 2f, Size * 0.3f));
            DrawTexLine(Point7 - Normal7 * 12, Point7 + Normal7 * 12, c0, c0, Crystal, Math.Min(Timer / 2f, Size * 0.3f));
            DrawTexLine(Point8 - Normal8 * 12, Point8 + Normal8 * 12, c0, c0, Crystal, Math.Min(Timer / 2f, Size * 0.3f));

            DrawTexLine(Point5 - Normal5 * 12, Point1 - Normal1 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
            DrawTexLine(Point6 - Normal6 * 12, Point2 - Normal2 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
            DrawTexLine(Point7 - Normal7 * 12, Point3 - Normal3 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
            DrawTexLine(Point8 - Normal8 * 12, Point4 - Normal4 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));

            DrawTexLine(Point5 - Normal5 * 12, Point3 - Normal3 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));
            DrawTexLine(Point6 - Normal6 * 12, Point4 - Normal4 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));
            DrawTexLine(Point7 - Normal7 * 12, Point1 - Normal1 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));
            DrawTexLine(Point8 - Normal8 * 12, Point2 - Normal2 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));

            DrawTexLine(Point5 - Normal5 * 12, Point4 - Normal4 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));
            DrawTexLine(Point6 - Normal6 * 12, Point1 - Normal1 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));
            DrawTexLine(Point7 - Normal7 * 12, Point2 - Normal2 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));
            DrawTexLine(Point8 - Normal8 * 12, Point3 - Normal3 * 20, c0, c0, Line, Math.Min(Timer / 4f, 1.5f));

            DrawTexLine(Point5 - Normal5 * 12, Point2 - Normal2 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
            DrawTexLine(Point6 - Normal6 * 12, Point3 - Normal3 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
            DrawTexLine(Point7 - Normal7 * 12, Point4 - Normal4 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
            DrawTexLine(Point8 - Normal8 * 12, Point1 - Normal1 * 20, c0, c0, LineV, Math.Min(Timer / 4f, 4.5f));
        }
        private void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex, float width = 6)
        {
            float Wid = width;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();
            vertex2Ds.Add(new Vertex2D(StartPos + Width, color1, new Vector3(1, 0, 0)));
            vertex2Ds.Add(new Vertex2D(EndPos + Width, color2, new Vector3(1, 1, 0)));
            vertex2Ds.Add(new Vertex2D(StartPos - Width, color1, new Vector3(0, 0, 0)));

            vertex2Ds.Add(new Vertex2D(EndPos + Width, color2, new Vector3(1, 1, 0)));
            vertex2Ds.Add(new Vertex2D(EndPos - Width, color2, new Vector3(0, 1, 0)));
            vertex2Ds.Add(new Vertex2D(StartPos - Width, color1, new Vector3(0, 0, 0)));


            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            Player player = Main.player[Projectile.owner];
            //DrawTexCircle(Timer * 1.2f, 52, new Color(64, 70, 255, 0), player.Center + RingPos - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), Main.time / 17);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
