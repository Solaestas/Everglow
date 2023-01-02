using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.LanternKing
{
    class GoldLanternLine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Lantern Line");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼须1");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        float ka = 0;
        int AIMNpc = -1;
        Vector2 AIMpos;
        bool HasColid = false;
        bool HasPro = false;
        public override void AI()
        {
            Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            AIMpos = new Vector2(-700, 0).RotatedBy(Projectile.timeLeft / 18f * Projectile.ai[0]);
            ka = 1;
            if (Projectile.timeLeft < 60f)
            {
                ka = Projectile.timeLeft / 60f;
            }

            if (Projectile.timeLeft >= 60)
            {
                Vector2 v2 = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v2, ModContent.ProjectileType<GoldLanternLine2>(), 2, 0, player.whoAmI, 0, 0);
            }
            if (Projectile.timeLeft >= 285)
            {
                Sca = (float)(Math.Sqrt((300 - Projectile.timeLeft) / 15f));
            }
            Vector2 v = Vector2.Normalize(player.Center - Projectile.Center) * 0.15f;
            Projectile.velocity.Y += 0.2f;
            Projectile.velocity += v;
            if (HasColid)
            {
                if (!HasPro)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Vector2 va = Projectile.velocity.RotatedBy(j / 5f * Math.PI);
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, va * 3f, ModContent.ProjectileType<GoldLanternLine3>(), 0, 0, player.whoAmI, 0, 0);
                    }
                    HasPro = true;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!HasColid)
            {
                HasColid = true;
                if (Projectile.timeLeft >= 60)
                {
                    Projectile.timeLeft = 60;
                }
            }
            Projectile.velocity *= 0.2f;
            return false;
        }
        public override void Kill(int timeLeft)
        {
            /*Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
            for (int j = 0; j < 10; j++)
            {
                Vector2 v2 = Projectile.velocity.RotatedBy(j / 5f * Math.PI);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, v2, ModContent.ProjectileType<Projectiles.GoldLanternLine3>(), 0, 0, player.whoAmI, 0, 0);
            }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public static float Timer = 0;
        public static int WHOAMI = -1;
        public static int Typ = -1;
        int TrueL = 1;
        float x = 0;
        float Sca = 0;
        public override void PostDraw(Color lightColor)
        {
            if (!HasColid)
            {
                x += 0.01f;
                float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
                float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, 0, new Vector2(128f, 128f), K * 2.4f * Sca, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.8f, 0f, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(128f, 128f), K * 2.4f * Sca, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.75), new Vector2(128f, 128f), M * 2.4f * Sca, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.6f, 0f, 0) * 0.4f, (float)(Math.PI * 0.25), new Vector2(128f, 128f), M * 2.4f * Sca, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, x * 6f, new Vector2(128f, 128f), (M + K) * 2.4f * Sca, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.4f, 0f, 0) * 0.4f, -x * 6f, new Vector2(128f, 128f), (float)Math.Sqrt(M * M + K * K) * 2.4f * Sca, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 12;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / 5f;
            }
            TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                Lighting.AddLight(Projectile.oldPos[i], (float)(255 - Projectile.alpha) * 1.2f / 50f * ka * (1 - factor), (float)(255 - Projectile.alpha) * 0.7f / 50f * ka * (1 - factor), 0);
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
                Vx.Add(bars[1]);
                Vx.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapLanternLine").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
    }
}
