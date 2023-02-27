using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class JellyPower : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("JellyPower");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "啫喱喷流");
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 9;
            Projectile.extraUpdates = 4;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 12000;
            Projectile.hostile = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            if (Tokill >= 0 && Tokill <= 2)
            {
                Projectile.Kill();
            }
            Player player = Main.player[Projectile.owner];
            if (Tokill <= 15 && Tokill > 0)
            {
                Projectile.Center = vp;
                Projectile.velocity = Projectile.oldVelocity;
            }
            Tokill--;
            Projectile.velocity.Y += 0.03f;
        }
        Vector2 vp = Vector2.Zero;
        int Tokill = -1;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Weapons.Slingshots.Projectiles.KSSlingshotHit>(), (int)((double)Projectile.damage), Projectile.knockBack, Projectile.owner, DrawC, 2 - Projectile.ai[0]);
            if (Projectile.penetrate >= 2)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.velocity *= 0.98f;
            }
            if (Projectile.penetrate < 2)
            {
                Projectile.velocity = Projectile.oldVelocity;
                vp = Projectile.Center;

                Tokill = 15;
                float a = Main.rand.NextFloat(0, 500.5f);
                Player player = Main.player[Projectile.owner];
                Projectile.friendly = false;
                Projectile.damage = 0;
                Projectile.tileCollide = false;
                Projectile.ignoreWater = true;
                Projectile.aiStyle = -1;
            }
            if (Projectile.ai[0] == 0 && Projectile.penetrate >= 6)
            {
                Player player = Main.player[Projectile.owner];
                for (int g = 0; g < 3; g++)
                {
                    int h = Projectile.NewProjectile(null, Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.3f, 0.6f), Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 1);
                    Main.projectile[h].penetrate = 3;
                }
            }
            return false;
        }
        int TrueL = 0;//真实的长度
        float DrawC = 0;
        public override void Kill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/JellyPower").Value;
            Texture2D t2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/JellyPowerL").Value;
            int frameHeight = t.Height;
            Vector2 drawOrigin = new Vector2(t.Width * 0.5f, t.Height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(1f, Projectile.gfxOffY);
                Color color = new Color(165, 165, 165, 105);
                Color color2 = new Color((int)(color.R * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.G * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.B * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (int)(color.A * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
                //Main.spriteBatch.Draw(t, drawPos, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(t2, drawPos, null, new Color(color2.R * 2 / (k + 4) * Projectile.penetrate / 9f / 255f, color2.R * 2 / (k + 4) * Projectile.penetrate / 9f / 255f, color2.R * 2 / (k + 4) * Projectile.penetrate / 9f / 255f, 0), Projectile.rotation, new Vector2(30), Projectile.scale * Projectile.penetrate / 9f, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            DrawC = 0.3f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            TrueL = 1;
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
                float width = 6;
                if (Projectile.timeLeft > 30)
                {
                    width = 6;
                }
                else
                {
                    width = Projectile.timeLeft / 5f;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                if (normalDir.Length() < 0.2f)
                {
                    normalDir = Projectile.velocity / Projectile.velocity.Length();
                }
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueL;
                var color = Color.Lerp(new Color(DrawC, DrawC, DrawC, 0), new Color(0, 0, 0, 0), factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(4, 4) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(4, 4) - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }
            List<Vertex2D> triangleList = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                Vector2 va = Projectile.velocity * 1.1f;
                if (Tokill <= 44 && Tokill > 0)
                {
                    va = Projectile.velocity * 0.05f;
                }
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + va, new Color(DrawC, DrawC, DrawC, 0), new Vector3(0, 0.5f, 1));
                triangleList.Add(bars[1]);
                triangleList.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }
                Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/EShootKS").Value;
                Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
