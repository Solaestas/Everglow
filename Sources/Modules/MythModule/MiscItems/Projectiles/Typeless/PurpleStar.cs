using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Typeless
{
	public class PurpleStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1080;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Projectile.velocity *= 0.98f;
            Vector2 v = new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center;
            Projectile.velocity += v / 1500f;
            if (v.Length() < 40)
            {
                Projectile.Kill();
            }
        }
        internal int[] Ty = { ModContent.ItemType<MiscItems.FixCoins.FixCoinCrit4>(), ModContent.ItemType<MiscItems.FixCoins.FixCoinDamage4>(), ModContent.ItemType<MiscItems.FixCoins.FixCoinDefense4>()};
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, Projectile.Center);
            for (int h = 0; h < 20; h++)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<TheTusk.Dusts.PurePurple>(), 0, 0, 0, default(Color), 15f * Main.rand.NextFloat(0.4f, 1.1f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3;
            }
            Item.NewItem(null, Projectile.Center, Ty[Main.rand.Next(Ty.Length)]);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(0, 0, 0, 0));
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            Effect ef = MythContent.QuickEffect("Effects/TrailB2");

            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                int width = 30;
                Vector2 v = new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center;
                if (v.Length() < 210)
                {
                    width = (int)(v.Length() / 7f);
                    ;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Purple, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(17, 17), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(17, 17), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<Vertex2D> triangleList = new List<Vertex2D>();

            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + Projectile.ai[0]);
                Texture2D Purple = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapColdPurple").Value;
                Texture2D Shape = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline").Value;
                Texture2D Mask = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/IceTrace").Value;
                Main.graphics.GraphicsDevice.Textures[0] = Purple;
                Main.graphics.GraphicsDevice.Textures[1] = Shape;
                Main.graphics.GraphicsDevice.Textures[2] = Mask;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;

                ef.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}