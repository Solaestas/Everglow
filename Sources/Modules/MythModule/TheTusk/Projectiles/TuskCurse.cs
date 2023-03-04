using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.TheTusk.Projectiles
{
    public class TuskCurse : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1080;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void AI()
        {
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
            Projectile.velocity *= 0.98f;
            Projectile.velocity.Y += 0.4f;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, Projectile.Center);
            for (int h = 0; h < 20; h++)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, DustID.VampireHeal, 0, 0, 0, default, 15f * Main.rand.NextFloat(0.4f, 1.1f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3;
            }
            NPC.NewNPC(null, (int)Projectile.Center.X, (int)Projectile.Bottom.Y, ModContent.NPCType<NPCs.Bosses.BloodTusk.TuskPoolWave>());
            NPC.NewNPC(null, (int)Projectile.Center.X, (int)Projectile.Bottom.Y, ModContent.NPCType<NPCs.Bosses.BloodTusk.TuskRedLight>());
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Weapon.ToothMagicHit>(), 0, Projectile.knockBack, Projectile.owner, 0f, 0f);
        }
        private Effect ef;
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Projectiles/TuskCurseGlow").Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(15f, 15f), Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            List<VertexBase.CustomVertexInfo> bars = new List<VertexBase.CustomVertexInfo>();
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TuskFlame").Value;

            int width = 60;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);

                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(15, 15), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new VertexBase.CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(15, 15), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<VertexBase.CustomVertexInfo> triangleList = new List<VertexBase.CustomVertexInfo>();

            if (bars.Count > 2)
            {
                triangleList.Add(bars[0]);
                var vertex = new VertexBase.CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 5, Color.White, new Vector3(0, 0.5f, 1));
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
                ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapBloodTusk").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTrace").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Projectiles/Metero").Value;
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