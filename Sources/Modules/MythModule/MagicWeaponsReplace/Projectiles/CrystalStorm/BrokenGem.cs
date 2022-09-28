using Terraria.Localization;
using Terraria.GameContent;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    public class BrokenGem : ModProjectile, IWarpProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Gem");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "碎宝石");
        }

        private float RamdomC = -1;
        private float RamdomC2 = -1;
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

        private float Ome = 0;
        private float Ros = 0;
        private float Theta = 0;
        private bool Ang = false;
        private Vector2 p1;
        private Vector2 p2;
        private Vector2 p3;
        private Vector2 po1;
        private Vector2 po2;
        private Vector2 po3;
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

        private float fade = 0;

        private Effect ef;
        private Vector2 VS1;
        private Vector2 VS2;
        private Vector2 VS3;
        public override void PostDraw(Color lightColor)
        {

        }
        public void DrawWarp()
        {
            Color colorD = new Color(0f, 0.01f,0f);

            int DrawBase = (int)(122.5 + Math.Sin(RamdomC) * 122.5);
            List<Vertex2D> Vx = new List<Vertex2D>();

            Vx.Add(new Vertex2D(po1 + Projectile.Center - Main.screenPosition, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po2 + Projectile.Center - Main.screenPosition, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po3 + Projectile.Center - Main.screenPosition, colorD, new Vector3(0, 0, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
        }
    }
}
