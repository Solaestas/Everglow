using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;
using static Everglow.Sources.Modules.MythModule.Common.MythUtils;
namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.GoldenShower
{
    internal class GoldenShowerArray : ModProjectile, IWarpProjectile
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
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.GoldenShower)
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
            float k0 = (31 - Timer) / 30f;
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1 - k0, 1 - k0, 1 - k0, 1 - k0), true);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect Fade = MythContent.QuickEffect("Effects/Fade");

            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * (Projectile.ai[0] == 0 ? Main.GameViewMatrix.ZoomMatrix : Main.Transform);
            Fade.Parameters["uTransform"].SetValue(model * projection);
            Fade.Parameters["alphaValue"].SetValue(Commons.Core.Utils.MathUtils.Sqrt(k0));
            Fade.Parameters["tex0"].SetValue(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/GoldenShower/Golden"));
            Fade.CurrentTechnique.Passes[0].Apply();
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), new Color(255, 199, 0, 0));
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;

        public void DrawMagicArray(Texture2D tex, Color c0, bool MinusByScreenPos = false)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Water = tex;
            Color c1 = new Color(c0.R * 0.39f / 255f, c0.G * 0.39f / 255f, c0.B * 0.39f / 255f, c0.A * 0.39f / 255f);
            Vector2 Zero = Vector2.Zero;
            if(MinusByScreenPos)
            {
                Zero = -Main.screenPosition;
            }
            DrawTexCircle(30 * 1.6f, 22, c0, player.Center + RingPos + Zero, Water, Main.timeForVisualEffects / 17);
            DrawTexCircle(30 * 1.3f, 32, c1, player.Center + RingPos + Zero, Water, -Main.timeForVisualEffects / 17);

            float timeRot = (float)(Main.timeForVisualEffects / 57d);
            Vector2 Point1 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Point2 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 2 / 3d + timeRot);
            Vector2 Point3 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 4 / 3d + timeRot);

            Vector2 Point4 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot);
            Vector2 Point5 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 3 / 3d + timeRot);
            Vector2 Point6 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 5 / 3d + timeRot);
            DrawTexLine(Point1, Point2, c1, c1, Water);
            DrawTexLine(Point2, Point3, c1, c1, Water);
            DrawTexLine(Point3, Point1, c1, c1, Water);

            DrawTexLine(Point4, Point5, c1, c1, Water);
            DrawTexLine(Point5, Point6, c1, c1, Water);
            DrawTexLine(Point6, Point4, c1, c1, Water);
        }
        public void DrawMagicArray(VFXBatch spriteBatch,Texture2D tex, Color c0, bool MinusByScreenPos = false)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Water = tex;
            Color c1 = new Color(c0.R * 0.39f / 255f, c0.G * 0.39f / 255f, c0.B * 0.39f / 255f, c0.A * 0.39f / 255f);
            Vector2 Zero = Vector2.Zero;
            if (MinusByScreenPos)
            {
                Zero = -Main.screenPosition;
            }
            DrawTexCircle(spriteBatch,30 * 1.6f, 22, c0, player.Center + RingPos + Zero, Water, Main.timeForVisualEffects / 17);
            DrawTexCircle(spriteBatch, 30 * 1.3f, 32, c1, player.Center + RingPos + Zero, Water, -Main.timeForVisualEffects / 17);

            float timeRot = (float)(Main.timeForVisualEffects / 57d);
            Vector2 Point1 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Point2 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 2 / 3d + timeRot);
            Vector2 Point3 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 4 / 3d + timeRot);

            Vector2 Point4 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot);
            Vector2 Point5 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 3 / 3d + timeRot);
            Vector2 Point6 = player.Center + RingPos + Zero + new Vector2(0, 30 * 1.8f).RotatedBy(Math.PI * 5 / 3d + timeRot);
            DrawTexLine(spriteBatch, Point1, Point2, c1, c1, Water);
            DrawTexLine(spriteBatch, Point2, Point3, c1, c1, Water);
            DrawTexLine(spriteBatch, Point3, Point1, c1, c1, Water);

            DrawTexLine(spriteBatch, Point4, Point5, c1, c1, Water);
            DrawTexLine(spriteBatch, Point5, Point6, c1, c1, Water);
            DrawTexLine(spriteBatch, Point6, Point4, c1, c1, Water);
        }




        public void DrawWarp(VFXBatch spriteBatch)
        {
            DrawMagicArray(spriteBatch, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), new Color((int)(255 * (Math.Sin(Main.timeForVisualEffects * 0.12f) + 1) / 2d), 69, 0, 0), true);
        }
    }
}