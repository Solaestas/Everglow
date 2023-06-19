using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Everglow.Myth.Common;
using Terraria.GameContent;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class DarkFlame2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("恶魔火");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.scale = 1f;
            this.CooldownSlot = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
        }
        private bool initialization = true;
        private double X;
        private float Y;
        private float b;
        private float rg = 0;
        public override void AI()
        {
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) - (float)Math.PI * 0.5f;
            /*if (projectile.timeLeft < 3595)
            {
                Vector2 vector = base.projectile.Center;
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, mod.DustType("DarkF"), 50f, 50f, 0, default(Color), (float)projectile.scale * 2.4f);
                Main.dust[num].velocity *= 0.0f;
                Main.dust[num].noGravity = true;
                Main.dust[num].alpha = 150;
            }*/
            if(Projectile.timeLeft == 179)
            {
                Y = Main.rand.NextFloat(0, 100f);
            }

                Projectile.velocity *= 0.985f;

            
            Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.25f,0.25f) / Projectile.velocity.Length());
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 150));
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            base.Projectile.penetrate--;
            if (base.Projectile.penetrate <= 0)
            {
                base.Projectile.Kill();
            }
            else
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f));
                Projectile.velocity.Y *= 0.2f;
            }
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
			Effect DefaultEffectB2 = ModContent.Request<Effect>("Everglow/Myth/Effects/TrailB2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;
                //spriteBatch.Draw(TextureAssets.MagicPixel.Value, projectile.oldPos[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                int width = 0;
                float x = 180 - Projectile.timeLeft;
                width = (int)(1200 / (x - 203) + (x + 3000) * (x - 600) / 10000f + 185.8f) * 2;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var alpha = (float)0;
                if (Projectile.timeLeft > 120)
                {
                    alpha = 0 + i / (float)Projectile.oldPos.Length;
                }
                else
                {
                    if(1 - Projectile.timeLeft / 120f + i / (float)Projectile.oldPos.Length > 1)
                    {
                        alpha = 1;
                    }
                    else
                    {
                        alpha = 1 - Projectile.timeLeft / 120f + i / (float)Projectile.oldPos.Length;
                    }
                }
                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Blue, factor);
                var w = MathHelper.Lerp(1f, 0.05f, alpha);

                bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(13, 13) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(13, 13) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

                // 把变换和所需信息丢给shader
                DefaultEffectB2.Parameters["uTransform"].SetValue(model * projection);
                DefaultEffectB2.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + Y);
				Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/VisualTextures/heatmapColdPurple");
				Main.graphics.GraphicsDevice.Textures[1] = MythContent.QuickTexture("UIImages/VisualTextures/Lightline");
				Main.graphics.GraphicsDevice.Textures[2] = MythContent.QuickTexture("UIImages/VisualTextures/FogTrace");
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
                //Main.graphics.GraphicsDevice.Textures[1] = TextureAssets.MagicPixel.Value;
                //Main.graphics.GraphicsDevice.Textures[2] = TextureAssets.MagicPixel.Value;

                DefaultEffectB2.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(); //Ins.Batch.Begin(); ??? but errors were caused
            }
        }


        // 自定义顶点数据结构，注意这个结构体里面的顺序需要和shader里面的数据相同
        private struct CustomVertexInfo : IVertexType
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

            public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
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