using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class SlienceMirrorIIBroken : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("broken mirror");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "碎镜");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 400000;
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
            if (Projectile.timeLeft == 399999)
            {
                Projectile.timeLeft = Main.rand.Next(35, 46);
            }
            Player player = Main.player[Projectile.owner];
            if (!Ang)
            {
                Ang = true;
                Ome = Main.rand.NextFloat(-0.15f, 0.15f);
                Ros = Main.rand.NextFloat(Main.rand.NextFloat(-0.45f, 0f), Main.rand.NextFloat(0f, 0.45f));
                Theta += Main.rand.NextFloat(-3.14f, 3.14f);
                p1 = new Vector2(Main.rand.NextFloat(-1.2f, 1.2f), Main.rand.NextFloat(-1.2f, 1.2f));
                p2 = new Vector2(Main.rand.NextFloat(-1.2f, 1.2f), Main.rand.NextFloat(-1.2f, 1.2f));
                p3 = new Vector2(Main.rand.NextFloat(-1.2f, 1.2f), Main.rand.NextFloat(-1.2f, 1.2f));
                Projectile.scale = Main.rand.NextFloat(0.4f, 1.0f);
            }
            Theta += Ros;
            po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
            po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
            po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)).RotatedBy(Projectile.rotation) * 90 * Projectile.scale;
            Projectile.rotation -= Ome * 0.66f;
            Projectile.velocity *= 0.99f;
            Projectile.scale *= 0.94f;
            if (Projectile.scale < 0.08f)
            {
                Projectile.Kill();
            }
        }
        float fade = 0;


        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<VertexInfo2> Vx = new List<VertexInfo2>();
            Vx.Add(new VertexInfo2(po1 + Projectile.Center - Main.screenPosition, new Color(0, 88, 180, 0), new Vector3(0, 0, 0)));
            Vx.Add(new VertexInfo2(po2 + Projectile.Center - Main.screenPosition, new Color(0, 88, 180, 0), new Vector3(0, 0, 0)));
            Vx.Add(new VertexInfo2(po3 + Projectile.Center - Main.screenPosition, new Color(0, 88, 180, 0), new Vector3(0, 0, 0)));
            Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
        }
        private struct VertexInfo2 : IVertexType
        {
            private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            });
            public Vector2 Position;
            public Color Color;
            public Vector3 TexCoord;
            public VertexInfo2(Vector2 position, Color color, Vector3 texCoord)
            {
                this.Position = position;
                this.Color = color;
                this.TexCoord = texCoord;
            }
            public VertexDeclaration VertexDeclaration
            {
                get
                {
                    return _vertexDeclaration;
                }
            }
        }
    }
}
