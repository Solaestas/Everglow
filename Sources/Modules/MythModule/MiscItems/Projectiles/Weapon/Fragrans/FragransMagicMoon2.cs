using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Fragrans
{
	public class FragransMagicMoon2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fragrans Magic");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "桂花魔法");
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1800;
            Projectile.alpha = 0;
            Projectile.penetrate = 5;
            Projectile.scale = 1f;
        }
        private float K = 10;
        public override void Kill(int timeLeft)
        {

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.Fragrans>()] < 1)
            {
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Fragrans.Fragrans>(), 0, 0, player.whoAmI, 0, 0);
                player.AddBuff(ModContent.BuffType<MiscItems.Buffs.Fragrans.MoonAndFragrans>(), 300);
            }
            else
            {
                MiscItems.Projectiles.Weapon.Fragrans.Fragrans.Reset = 300;
                if (target.type == NPCID.TargetDummy)
                {
                    MiscItems.Projectiles.Weapon.Fragrans.Fragrans.Dummy = true;
                }
            }
            for (int y = 0; y < 4; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926), 0, 0, ModContent.DustType<MiscItems.Dusts.Fragrans.Fragrans3>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 2.7f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.Length() / 8f);
            if (Projectile.timeLeft > 1000f)
            {
                Projectile.timeLeft = Main.rand.Next(60, 90);
                Projectile.rotation = Main.rand.NextFloat(0f, 6.28f);
            }
            if (Projectile.timeLeft < 30)
            {
                Projectile.scale = 0.1f + Projectile.timeLeft / 33.3333f;
            }
            Projectile.velocity *= 0.97f;
        }
        private Effect ef;
        /*public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TrailB").Value;
            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;
                //spriteBatch.Draw(Main.magicPixel, Projectile.oldPos[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                int width = 48;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Blue, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(24, 24) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(24, 24) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 3, Color.White, new Vector3(0, 0.5f, 1));
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
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

                // 把变换和所需信息丢给shader
                ef.Parameters["uTransform"].SetValue(model * projection);
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f + Projectile.ai[0]);
                Texture2D Blue = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapFragrans").Value;
                Texture2D Shape = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline3").Value;
                Texture2D Mask = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/Lightline3").Value;
                Main.graphics.GraphicsDevice.Textures[0] = Blue;
                Main.graphics.GraphicsDevice.Textures[1] = Shape;
                Main.graphics.GraphicsDevice.Textures[2] = Mask;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
                //Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

                ef.CurrentTechnique.Passes[0].Apply();


                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null);
                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
        }*/
    }
}