using Everglow.Sources.Commons.Function.Vertex;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class BrokenGem : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Gem");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "碎宝石");
        }
        float RamdomC = -1;
        float RamdomC2 = -1;
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 4000;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0));
        }
        float Ome = 0;
        float Ros = 0;
        float Theta = 0;
        bool Ang = false;
        Vector2 p1;
        Vector2 p2;
        Vector2 p3;
        Vector2 po1;
        Vector2 po2;
        Vector2 po3;
        public override void AI()
        {
            if (Projectile.timeLeft == 3999)
            {
                Projectile.timeLeft = Main.rand.Next(3987, 3998);
                if (RamdomC == -1)
                {
                    RamdomC = Main.rand.NextFloat(0f, 1500f);
                    RamdomC2 = Main.rand.NextFloat(0f, 0.005f);
                }
            }
            if (Projectile.timeLeft == 3985)
            {
                Projectile.friendly = true;
            }
            RamdomC += RamdomC2;
            Player player = Main.player[Projectile.owner];
            if (!Ang)
            {
                Ang = true;
                Ome = Main.rand.NextFloat(-0.15f, 0.15f);
                Ros = Main.rand.NextFloat(-0.15f, 0.15f);
                Theta += Main.rand.NextFloat(-3.14f, 3.14f);
                p1 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
                p2 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
                p3 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
            }
            Theta += Ros;
            po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
            po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
            po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
            Projectile.rotation -= Ome * 0.66f;
            Projectile.velocity *= 0.99f;
            Projectile.scale *= 0.95f;
            if (Projectile.scale < 0.05f)
            {
                Projectile.Kill();
            }
        }
        float fade = 0;

        private Effect ef;
        Vector2 VS1;
        Vector2 VS2;
        Vector2 VS3;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            List<Vertex2D> Vy = new List<Vertex2D>();
            Color colorD = Color.White;
            Vector2 v1 = po1 + Projectile.Center - Main.screenPosition;
            Vector2 v2 = po2 + Projectile.Center - Main.screenPosition;
            Vector2 v3 = po3 + Projectile.Center - Main.screenPosition;
            if (VS1 == Vector2.Zero)
            {
                VS1 = v1;
            }
            if (VS2 == Vector2.Zero)
            {
                VS2 = v2;
            }
            if (VS3 == Vector2.Zero)
            {
                VS3 = v3;
            }
            Vy.Add(new Vertex2D(v1, colorD, new Vector3(VS1.X / Main.screenTarget.Width, VS1.Y / Main.screenTarget.Height, 0)));
            Vy.Add(new Vertex2D(v2, colorD, new Vector3(VS2.X / Main.screenTarget.Width, VS2.Y / Main.screenTarget.Height, 0)));
            Vy.Add(new Vertex2D(v3, colorD, new Vector3(VS3.X / Main.screenTarget.Width, VS3.Y / Main.screenTarget.Height, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = Main.screenTargetSwap;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vy.ToArray(), 0, Vy.Count - 2);

            Color Co0 = new Color(0, 0, 255);
            if (Projectile.ai[0] == 2)
            {
                Co0 = new Color(255, 0, 0);
            }
            if (Projectile.ai[0] == 3)
            {
                Co0 = new Color(165, 0, 236);
            }
            if (Projectile.ai[0] == 4)
            {
                Co0 = new Color(200, 200, 200);
            }
            if (Projectile.ai[0] == 5)
            {
                Co0 = new Color(241, 159, 0);
            }
            int DrawBase = (int)(122.5 + Math.Sin(RamdomC) * 122.5);
            List<Vertex2D> Vx = new List<Vertex2D>();
            colorD = new Color((DrawBase + Co0.R) / 8, (DrawBase + Co0.G) / 8, (DrawBase + Co0.B) / 8, 0);
            Vx.Add(new Vertex2D(po1 + Projectile.Center - Main.screenPosition, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po2 + Projectile.Center - Main.screenPosition, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po3 + Projectile.Center - Main.screenPosition, colorD, new Vector3(0, 0, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
        }
    }
}
