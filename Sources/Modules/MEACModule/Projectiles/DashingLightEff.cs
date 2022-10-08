using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{     
    public class DashingLightEff : ModProjectile
    {
        Projectile projectile{get => Projectile;}
        public override string Texture => "Terraria/Images/Projectile_0";
        
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.timeLeft = 40;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 3;
            projectile.hide = true;
            projectile.DamageType = DamageClass.Melee;
            ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] = 100;
            oldPos = new Vector2[25];
        }
        Vector2[] oldPos = new Vector2[25];
        Vector2 vec = Vector2.Zero;
        public override void AI()
        {
            Lighting.AddLight(projectile.Center,0.9f, 0.6f, 0);
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
                projectile.velocity = Vector2.Zero;
                projectile.friendly = false;
            }
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
            int TrueLength = 1;
            for (int i = 1; i < oldPos.Length; ++i)
            {
                TrueLength++;
                if (oldPos[i] == Vector2.Zero) break;
            }
            for (int i = 1; i < oldPos.Length; ++i)
            {
                if (oldPos[i] == Vector2.Zero) break;

                var normalDir = oldPos[i - 1] - oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)TrueLength;
                var w = MathHelper.SmoothStep(1.5f, 0.1f, factor);
                float width = MathHelper.SmoothStep(0, 50, Math.Min(1, 3 * factor)) * projectile.scale;
                if (i > 15)
                {
                    width *= (float)(25 - i) / 10;
                }
                if (projectile.timeLeft < 20)
                {
                    width *= (float)projectile.timeLeft / 20;
                }
                Vector2 d = oldPos[i - 1] - oldPos[i];
                    bars.Add(new VertexInfo((vec + oldPos[i] - d * i * 0.6f) + normalDir * width, new Vector3((float)Math.Sqrt(factor), 1, w),Color.White));
                    bars.Add(new VertexInfo((vec + oldPos[i] - d * i * 0.45f) + normalDir * -width, new Vector3((float)Math.Sqrt(factor), 0, w), Color.White));

            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * (Projectile.ai[0]==0? Main.GameViewMatrix.ZoomMatrix:Main.Transform);
            Effect MeleeTrail = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/MeleeTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/tex4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MEACModule/Images/img_color", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MeleeTrail.CurrentTechnique.Passes[0].Apply();
            if (bars.Count >= 3)
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
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
    }
}
