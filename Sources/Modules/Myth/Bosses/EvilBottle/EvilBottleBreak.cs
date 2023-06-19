using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Everglow.Myth.Bosses.EvilBottle;
using Terraria.ModLoader;
using System.IO;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent;
using Everglow.Myth.Common;

namespace Everglow.Myth.Bosses.EvilBottle
{
	public class EvilBottleBreak : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("封魔石瓶");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "封魔石瓶");
        }
        private bool X = true;
        private int num10;
        private int ADDnum = 0;
        private float num13;
        private bool T = false;
        public override void SetDefaults()
        {
            NPC.behindTiles = true;
            base.NPC.damage = 0;
            base.NPC.width = 120;
            base.NPC.height = 160;
            base.NPC.defense = 0;
            base.NPC.lifeMax = 2000;
            base.NPC.knockBackResist = 0f;
            base.NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
            base.NPC.color = new Color(0, 0, 0, 0);
            base.NPC.aiStyle = -1;
            this.AIType = -1;
            base.NPC.lavaImmune = true;
            base.NPC.noGravity = false;
            base.NPC.noTileCollide = false;
            base.NPC.HitSound = SoundID.NPCHit1;
            base.NPC.DeathSound = SoundID.NPCDeath1;
        }
        Vector2[,] Pm = new Vector2[20,50];
        Vector2 O = Vector2.Zero;
        public override void AI()
        {
            NPC.dontTakeDamage = true;
            for(int i = 0;i < 40;i++)
            {
                if (EvilBottle.Blife < i * 25f && Pm[i, 0] == Vector2.Zero)
                {
                    Pm[i, 0] = NPC.Center + new Vector2(Main.rand.Next(-23, 23), Main.rand.Next(-50, 50));
                }
            }
            O = NPC.Center;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			//Effect DefaultEffectDarkRedGold = ModContent.Request<Effect>("Everglow/Myth/Effects/TrailDarkRedGold", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			for (int i = 1; i < 40; ++i)
            {
                List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
                for (int j = 1; j < 15; ++j)
                {
                    if (Pm[i, j] == Vector2.Zero) break;
                    //spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/Star"), projectile.position + projectile.velocity.RotatedBy(Math.PI / 2d * Dir) * 20 - Main.screenPosition, null, new Color(0.2f, 0f, 0f, 0f), 0f, new Vector2(36f, 36f), (float)Math.Sin(projectile.timeLeft / 30d * Math.PI) * 0.2f, SpriteEffects.None, 0f);
                    //spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/Star"), projectile.position + projectile.velocity.RotatedBy(Math.PI / 2d * Dir) * 20 - Main.screenPosition, null, new Color(0.2f, 0f, 0f, 0f), (float)(Math.PI / 2d), new Vector2(36f, 36f), (float)Math.Sin(projectile.timeLeft / 30d * Math.PI) * 0.6f, SpriteEffects.None, 0f);

                    int width = 0;
                    width = (int)(j * 4f + 20);
                    var normalDir = Pm[i, 1] - O;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var alpha = (float)0;
                    alpha = j / 15f;
                    var factor = j / 15f;
                    var color = Color.Lerp(Color.White, Color.Blue, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, alpha);

                    /*if(i > 2)
                    {
                        for (int j = 1; j < 9; ++j)
                        {
                            float t = j / 10f;
                            float ti = t - 0.1f;
                            Vector2 vk0 = (projectile.oldPos[i - 2] * (1 - t) * (1 - t) + projectile.oldPos[i - 1] * t * 2 * (1 - t) + projectile.oldPos[i] * t * t);
                            Vector2 vk1 = (projectile.oldPos[i - 2] * (1 - ti) * (1 - ti) + projectile.oldPos[i - 1] * ti * 2 * (1 - ti) + projectile.oldPos[i] * ti * ti);
                            normalDir = vk1 - vk0;
                            normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                            bars.Add(new CustomVertexInfo(vk0 + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                            bars.Add(new CustomVertexInfo(vk0 + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                        }
                    }*/

                    bars.Add(new CustomVertexInfo(Pm[i, j] + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new CustomVertexInfo(Pm[i, j] + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }

                List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

                if (bars.Count > 2)
                {

                    // 按照顺序连接三角形
                    triangleList.Add(bars[0]);
                    var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f, Color.White, new Vector3(0, 0.5f, 1));
                    triangleList.Add(bars[1]);
                    triangleList.Add(vertex);
                    for (int z = 0; z < bars.Count - 2; z += 2)
                    {
                        triangleList.Add(bars[z]);
                        triangleList.Add(bars[z + 2]);
                        triangleList.Add(bars[z + 1]);

                        triangleList.Add(bars[z + 1]);
                        triangleList.Add(bars[z + 2]);
                        triangleList.Add(bars[z + 3]);
                    }


                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                    RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                    // 干掉注释掉就可以只显示三角形栅格
                    //RasterizerState rasterizerState = new RasterizerState();
                    //rasterizerState.CullMode = CullMode.None;
                    //rasterizerState.FillMode = FillMode.WireFrame;
                    //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                    var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                    var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

                    // 把变换和所需信息丢给shader
                    //DefaultEffectDarkRedGold.Parameters["uTransform"].SetValue(model * projection);
                    //DefaultEffectDarkRedGold.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f);
					Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/VisualTextures/heatmapDarkRedGold");
					Main.graphics.GraphicsDevice.Textures[1] = MythContent.QuickTexture("UIImages/VisualTextures/Lightline");
					Main.graphics.GraphicsDevice.Textures[2] = MythContent.QuickTexture("UIImages/VisualTextures/FogTrace");
					Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                    Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                    Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;

                    //Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
                    //Main.graphics.GraphicsDevice.Textures[1] = TextureAssets.MagicPixel.Value;
                    //Main.graphics.GraphicsDevice.Textures[2] = TextureAssets.MagicPixel.Value;

                    //DefaultEffectDarkRedGold.CurrentTechnique.Passes[0].Apply();


                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                    Main.graphics.GraphicsDevice.RasterizerState = originalState;
                    spriteBatch.End();
                    spriteBatch.Begin();
                }
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
