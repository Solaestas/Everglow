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
using Everglow.Myth.Common;
using Terraria.GameContent;
//using TemplateMod2.Utils;


namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class DarkFlow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("DarkFlow");
        }
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.scale = 1;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        private bool initialization = true;
        private double X;
        private float Omega;
        private float b;
        private float K = 10;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int y = 0; y < 40; y++)
            {
                int num90 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 4, 4, 64, 0f, 0f, 100, default(Color), Main.rand.NextFloat(2.8f, 4.5f));
                Main.dust[num90].noGravity = true;
                Main.dust[num90].velocity = new Vector2(Main.rand.NextFloat(2.0f, 2.5f), Main.rand.NextFloat(1.8f, 2.5f)).RotatedByRandom(Math.PI * 2d);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(1f, 1f, 1f, 0));
        }
        public override void AI()
        {
            /*if (Main.myPlayer == projectile.owner)
            {
                var projectileToMouse = Vector2.Normalize(Main.MouseWorld - projectile.Center) * 16;
                projectile.velocity = projectileToMouse;

                for (int i = projectile.oldPos.Length - 1; i > 0; --i)
                    projectile.oldRot[i] = projectile.oldRot[i - 1];
                projectile.oldRot[0] = projectile.rotation;
            }*/
            base.Projectile.rotation = (float)(Math.Atan2(base.Projectile.velocity.Y, base.Projectile.velocity.X) + Math.PI * 0.25);
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC(base.Projectile.damage, base.Projectile.knockBack, base.Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        base.Projectile.penetrate--;
                    }
                }
            }
            Projectile.velocity *= 0.96f;
            if (K >= 40)
            {
                K *= 0.96f;
            }
            if (K <= 6)
            {
                K *= 1.05f;
            }
            if(Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            K += Main.rand.NextFloat(-0.025f, 0.025f);
            float num2 = Projectile.Center.X;
            float num3 = Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num2 = num5;
                        num3 = num6;
                        flag = true;
                    }
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC(base.Projectile.damage, base.Projectile.knockBack, base.Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        base.Projectile.penetrate--;
                        NPC target = Main.npc[j];
                    }
                }
            }
            if (flag)
            {
                float num8 = 50f;
                Vector2 vector1 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num9 = num2 - vector1.X;
                float num10 = num3 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + num9) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num10) / 21f;
                if(Projectile.velocity.Length() > 0.005f)
                {
                    Projectile.velocity *= 0.95f;
                }

            }
        }
        public override void PostDraw(Color lightColor)
        {
			Effect DefaultEffectDarkRedGold2 = ModContent.Request<Effect>("Everglow/Myth/Effects/TrailDarkRedGold2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            // 0„30Š00100·350106¦09¡§0…70100·370100·34010¡­80106¦02010¡­80100”70010¡­90106¥940106¦07¡§0…5010¡­60106¦07010¡­60106§820106¥98010¡­70106¥75010¡­10„30Š0010¡­70100·390100·390106¦09010¡­60100·34¡§¡ã
            for (int i = 2; i < Projectile.oldPos.Length - 1; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) break;
                //spriteBatch.Draw(TextureAssets.MagicPixel.Value, projectile.oldPos[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                int width = 30;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var alpha = i / (float)Projectile.oldPos.Length * (float)Math.Log10((60 - Projectile.timeLeft) + 1);
                var factor = i / (float)Projectile.oldPos.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, alpha);

                bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * width + new Vector2(13, 13) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(Projectile.oldPos[i] + normalDir * -width + new Vector2(13, 13) - Projectile.velocity * 1.5f, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 0„30Š0010¡­70100·390100·390106¦09010¡­60100·34¡§¡ã0106¥99010¡­10106¥950100·370106¦060106§850106¥950106¦050100·340100·32
                triangleList.Add(bars[0]);
                var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Projectile.velocity * 2, Color.White, new Vector3(0, 0.5f, 1));
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
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                // 0106¥900106¦07010¡­80106§800„30†90106¥740106¦080100·31010¡­80106§800106¥960100·310106¥970106¦070100·360100·38010¡ë00106¥930100·330100·380106¦080106¥960106¦060106§850106¥950106¦050100·340100·320100·390„3¨¨0106¥900100”79
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

                // 0„30Š00100·350„30†80100”710106¥930106¥930106¥920100·310106¦09¡§0…70100·34¡§¡§0100·340106¦030100·330106¥74010¡­90106¥790106¥900106§83shader
                DefaultEffectDarkRedGold2.Parameters["uTransform"].SetValue(model * projection);
                DefaultEffectDarkRedGold2.Parameters["uTime"].SetValue(-(float)Main.time * 0.03f);
                Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/VisualTextures/heatmapDarkRedGold");
				Main.graphics.GraphicsDevice.Textures[1] = MythContent.QuickTexture("UIImages/VisualTextures/Lightline");
				Main.graphics.GraphicsDevice.Textures[2] = MythContent.QuickTexture("UIImages/VisualTextures/FogTrace");
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
                //Main.graphics.GraphicsDevice.Textures[1] = TextureAssets.MagicPixel.Value;
                //Main.graphics.GraphicsDevice.Textures[2] = TextureAssets.MagicPixel.Value;

                DefaultEffectDarkRedGold2.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(); //Ins.Batch.Begin(); ??? but errors were caused
            }
        }


        // 0„30†90100·38010¡­90„3¡ì0100·360100”72010¡­90106¥76010¡­80100”700106¦080106§850106¥96010¡ë60106¥95¡§0„40106¥910106¥910106¥75010¡­10„30†90106¥740100·36010¡ë90100·39010¡ë90106¥900106§820106¥95¡§0„40106¥910106¥910100·300100”720106¥980100”770106¦010100”73010¡­80106¦020106¦09010¡­60100·34¡§¡ã0100·34¡§¡§0100·360106¥790106¥920100·31shader0106¥980100”770106¦010100”73010¡­80106¦020106¦080106§850106¥96010¡ë60100·33¡§¡è0100·31010¡­1
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