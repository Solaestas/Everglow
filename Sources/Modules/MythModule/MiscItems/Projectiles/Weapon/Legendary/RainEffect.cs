using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class RainEffect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 150;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 80;
        }
        bool Start = false;
        Vector2 Cent;
        Vector2 Acc;
        float Ome = 0;
        float kx = 1;
        int AimN = -1;
        public override void AI()
        {
            if (!Start)
            {
                Projectile.velocity = new Vector2(Main.rand.NextFloat(0, 10f), 0).RotatedByRandom(6.28);
                Acc = new Vector2(Main.rand.NextFloat(0, 0.35f), 0).RotatedByRandom(6.28);
                Cent = Projectile.Center;
                Projectile.position += new Vector2(0, Main.rand.NextFloat(50f, 200f)).RotatedByRandom(6.28);
                Ome = Main.rand.NextFloat(-0.16f, 0.16f);
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<RainArrowDrop2>())
                    {
                        if (i > Projectile.whoAmI)
                        {
                            Projectile.active = false;
                            break;
                        }
                    }
                }
                if (AimN == -1)
                {
                    for (int f = 0; f < 200; f++)
                    {
                        if (Main.projectile[f].type == ModContent.ProjectileType<RainArrowDrop2>())
                        {
                            AimN = f;
                            break;
                        }
                    }
                }
                Start = true;
            }
            if (AimN != -1)
            {
                Cent = Main.projectile[AimN].Center;
            }
            Vector2 v0 = Cent - Projectile.Center;
            if (v0.Length() >= 12)
            {
                Vector2 v = Cent - (Projectile.Center + Projectile.velocity * 30);
                Vector2 v2 = v / v.Length() * 0.05f * (float)(1 + Math.Log(v.Length() + 1));

                Acc *= 0.95f;
                Projectile.velocity += (Acc + v2);
                Projectile.velocity = Projectile.velocity.RotatedBy(Ome);
                Ome *= 0.96f;
                kx = 20 - v0.Length() / 12f;
                if (kx < 1)
                {
                    kx = 1;
                }
            }
            else
            {
                Projectile.velocity *= 0.8f;
                kx -= 1;
                if (kx <= 1)
                {
                    Projectile.active = false;
                    ;
                }
            }
            if (Main.projectile[AimN].type != ModContent.ProjectileType<RainArrowDrop2>() || !Main.projectile[AimN].active)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        private Effect ef;
        int TrueL = 1;
        float ka = 0;
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 4;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / 15f;
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
                var factor = 1f;
                if (Projectile.oldPos.Length > 0)
                {
                    factor = i / (float)TrueL;
                }
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                Lighting.AddLight(Projectile.oldPos[i], (float)(255 - Projectile.alpha) * 1.2f / 50f * ka * (1 - factor), (float)(255 - Projectile.alpha) * 0.7f / 50f * ka * (1 - factor), 0);
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(0.5f, 0.5f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f, new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
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
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/DashCore/StarTrail").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
    }
}