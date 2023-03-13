using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Commons.Function.Curves;
namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Projectiles
{     
    public class AcroporaThumpEff : ModProjectile
    {
        Projectile projectile{get => Projectile;}
        public override string Texture => "Terraria/Images/Projectile_0";
        
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.timeLeft = 40;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 3;
            projectile.hide = true;
            projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] = 100;
            oldPos = new Vector2[35];
        }
        Vector2[] oldPos = new Vector2[35];
        Vector2 vec = Vector2.Zero;
        public override void AI()
        {
            Lighting.AddLight(projectile.Center + projectile.velocity * (40 - Projectile.timeLeft) * 0.2f, 0.24f, 0.36f, 0f);
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] == 0)
            {
                if (projectile.timeLeft > 20)
                {
                    vec = player.Center + (40f - projectile.timeLeft) * projectile.velocity * 0.2f;
                }
                projectile.Center = vec;
            }
            else
            {
                vec = projectile.Center;
                projectile.friendly = true;
            }
            if (projectile.timeLeft > 20)
            {
                projectile.rotation = projectile.velocity.ToRotation();
                for (int i = oldPos.Length - 1; i > 0; --i)
                    oldPos[i] = oldPos[i - 1];
                    oldPos[0] = (40 - projectile.timeLeft) * projectile.velocity;
            }
            if (projectile.timeLeft < 20)
            {
                projectile.friendly = false;
            }
            projectile.position += projectile.velocity * 6;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            List<VertexInfo> bars = new List<VertexInfo>();
            List<VertexInfo> barsII = new List<VertexInfo>();
            List<Vector2> OldPoses = new List<Vector2>();
            OldPoses.Add(oldPos[0] + projectile.velocity);
            for (int i = 1; i < oldPos.Length; ++i)
            {
                if (oldPos[i] == Vector2.Zero) 
                    break;
                else
                {
                    OldPoses.Add(oldPos[i]);
                }
            }
            List<Vector2> SmoothPos = CatmullRom.SmoothPath(OldPoses, Math.Max(32 - OldPoses.Count, 6));
            for (int i = 1; i < SmoothPos.Count; ++i)
            {
                var normalDir = SmoothPos[i - 1] - SmoothPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = (i - 1) / (float)(SmoothPos.Count);
                var w = MathHelper.SmoothStep(1.14514f, 0f, factor);
                float width = MathHelper.SmoothStep(0, 60, Math.Min(1, factor * 3f)) * projectile.scale;
                if (i > SmoothPos.Count * 0.75f)
                {
                    width *= (i - SmoothPos.Count) / (float)(SmoothPos.Count);
                }
                float k1 = 1;
                if (projectile.timeLeft < 20)
                {
                    k1 = (float)projectile.timeLeft / 20f;
                }
                if(projectile.timeLeft < 38)
                {
                    if(projectile.velocity.X < 0)
                    {
                        bars.Add(new VertexInfo((vec + SmoothPos[i]) + normalDir * width * k1, new Vector3((float)Math.Sqrt(factor), 0, w), Color.White));
                        bars.Add(new VertexInfo((vec + SmoothPos[i]) + normalDir * -width * k1, new Vector3((float)Math.Sqrt(factor), 1, w), Color.White));
                    }
                    else
                    {
                        bars.Add(new VertexInfo((vec + SmoothPos[i]) + normalDir * width * k1, new Vector3((float)Math.Sqrt(factor), 1, w), Color.White));
                        bars.Add(new VertexInfo((vec + SmoothPos[i]) + normalDir * -width * k1, new Vector3((float)Math.Sqrt(factor), 0, w), Color.White));
                    }
                }
                if (projectile.timeLeft < 38)
                {
                    barsII.Add(new VertexInfo((vec + SmoothPos[i]) + normalDir * width * 2, new Vector3((float)Math.Sqrt(factor), 1, w), Color.White));
                    barsII.Add(new VertexInfo((vec + SmoothPos[i]) + normalDir * -width * 2, new Vector3((float)Math.Sqrt(factor), 0, w), Color.White));
                }

            }
            Texture2D t0 = YggdrasilContent.QuickTexture("KelpCurtain/Items/Weapons/AcroporaSpear");
            Color c0 = Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16));
            Main.spriteBatch.Draw(t0, projectile.Center - Main.screenPosition, null, c0 * (projectile.timeLeft / 40f), projectile.rotation + (float)(Math.PI / 4f), t0.Size() / 2f, 1, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * (Projectile.ai[0]==0? Main.GameViewMatrix.ZoomMatrix:Main.Transform);
            Effect MeleeTrail = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = YggdrasilContent.QuickTexture("KelpCurtain/Projectiles/texShade");
            MeleeTrail.Parameters["tex1"].SetValue(YggdrasilContent.QuickTexture("KelpCurtain/Projectiles/Acropora_Color"));
            MeleeTrail.CurrentTechnique.Passes[0].Apply();
            if (bars.Count >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = YggdrasilContent.QuickTexture("KelpCurtain/Projectiles/AcroporaLight");
            MeleeTrail.Parameters["tex1"].SetValue(YggdrasilContent.QuickTexture("KelpCurtain/Projectiles/Acropora_Color"));
            MeleeTrail.CurrentTechnique.Passes[0].Apply();
            if (bars.Count >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            MeleeTrail = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/MeleeTrailFade", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            float k0 = (40 - projectile.timeLeft) / 40f;
            MeleeTrail.Parameters["FadeValue"].SetValue(Commons.Core.Utils.MathUtils.Sqrt(k0 * 1.2f));
            Main.graphics.GraphicsDevice.Textures[0] = YggdrasilContent.QuickTexture("KelpCurtain/Projectiles/texBlood");
            MeleeTrail.Parameters["tex1"].SetValue(YggdrasilContent.QuickTexture("KelpCurtain/Projectiles/Acropora_RedColor"));
            MeleeTrail.CurrentTechnique.Passes[0].Apply();
            if (bars.Count >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsII.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


        }


        public struct VertexInfo : IVertexType
        {
            private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color,0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate,0)

            });
            public Vector2 Position;
            public Color Color;
            public Vector3 TexCoord;
            public VertexInfo(Vector2 position, Vector3 texCoord, Color color)
            {
                Position = position;
                TexCoord = texCoord;
                Color = color;
            }
            public VertexDeclaration VertexDeclaration
            {
                get => _vertexDeclaration;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }
    }
}
