using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
//using TemplateMod2.Utils;
using Terraria.Localization;
using Terraria.Graphics.Capture;
using Everglow.Myth.Common;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class EvilLightingbolt2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("魔咒闪电");
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 11;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
        }
        private bool initialization = true;
        private double X;
        private float Omega;
        private float b;
        private float K = 10;
        private Vector2[] vL = new Vector2[100];

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f, 1f, 1f, 0));
        }
        public override void AI()
        {
            if (Projectile.timeLeft == 11)
            {
                Vector2 vo = new Vector2(0, 5).RotatedByRandom(Math.PI * 2);
                b = Main.rand.NextFloat(0, 100f);
                vL[0] = Projectile.Center;
                for (int i = 1; i < Projectile.ai[0]; i++)
                {
                    vL[i] = vL[i - 1] + vo + new Vector2(0, Main.rand.NextFloat(0, 10f)).RotatedByRandom(Math.PI * 2);
                    vo = vo.RotatedBy(Main.rand.NextFloat(-0.15f,0.15f));
                    if (Main.rand.Next(100) < 4)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), vL[i].X, vL[i].Y, 0, 0, ModContent.ProjectileType<EvilLightingbolt2>(), 0, 0f, Main.myPlayer, Projectile.ai[0] - i, 0f);
                    }
                }
            }
            if (Projectile.timeLeft == 10)
            {
                int I = 0;
                for (int i = 15; i < 99; i++)
                {
                    Projectile.position = vL[i];
                    if (Main.tile[(int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)].TileType == 367 || Main.tile[(int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)].TileType == 357)
                    {
                        I = i;
                        break;
                    }
                }
                if (I != 0)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0f, 3f)).RotatedByRandom(Math.PI * 2);
                        int num = Dust.NewDust(vL[I] - new Vector2(4, 4) + new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, 112, v0.X, v0.Y, 0, default(Color), Main.rand.NextFloat(0.5f, 1.2f));
                        v0 = new Vector2(0, Main.rand.NextFloat(0f, 6f)).RotatedByRandom(Math.PI * 2);
                        int num1 = Dust.NewDust(vL[I] - new Vector2(4, 4) + new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, 112, v0.X, v0.Y, 0, default(Color), Main.rand.NextFloat(0.5f, 1.2f));
                    }
                    for (int i = I; i < 99; i++)
                    {
                        vL[i] = Vector2.Zero;
                    }
                }
            }
            if (Projectile.timeLeft < 10)
            {
                for (int i = 1; i < 99; i++)
                {
                    if (vL[i] != Vector2.Zero)
                    {
                        vL[i] += new Vector2(0,Main.rand.NextFloat(0,2f)).RotatedByRandom(Math.PI * 2);
                        Lighting.AddLight(vL[i],new Vector3(0.2f, 0, 0.9f) * 0.1f * Projectile.timeLeft);
                    }
                }
            }
            if (Projectile.timeLeft == 5)
            {
                for (int i = 1; i < 99; i++)
                {
                    if (vL[i] != Vector2.Zero)
                    {
                        Terraria.Player player = Terraria.Main.LocalPlayer;
                        if ((player.Center - vL[i]).Length() < 24)
                        {
                            Projectile.hostile = true;
                            Projectile.damage = 120;
                            Projectile.position = player.Center;
                            break;
                        }
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
        }
        public override void PostDraw(Color lightColor)
        {
			Effect DefaultEffectB2 = ModContent.Request<Effect>("Everglow/Myth/Effects/TrailB2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < 99; ++i)
            {
                if(vL[i] == Vector2.Zero)
                {
                    break;
                }
                int width = 10;
                if (Projectile.timeLeft < 10)
                {
                    if (Projectile.timeLeft > 5)
                    {
                        width = 10;
                    }
                    else
                    {
                        width = Projectile.timeLeft * 2;
                    }
                }
                else
                {
                    width = 0;
                }
                Vector2 vLr = (vL[i] - vL[i - 1]).RotatedBy(Math.PI / 2d) / (vL[i] - vL[i - 1]).Length();
                var factor = 0.2f;
                var color = Color.Lerp(Color.White, Color.Blue, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new CustomVertexInfo(vL[i] + vLr * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(vL[i] + vLr * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
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
                DefaultEffectB2.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + b);
                Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/VisualTextures/heatmapColdPurple");
				Main.graphics.GraphicsDevice.Textures[1] = MythContent.QuickTexture("UIImages/VisualTextures/Lightline");
				Main.graphics.GraphicsDevice.Textures[2] = MythContent.QuickTexture("UIImages/VisualTextures/Lightline2");
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

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