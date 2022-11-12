using Everglow.Sources.Modules.MEACModule.Projectiles;
using Everglow.Sources.Modules.MythModule;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.Curves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Everglow.Sources.Modules.MEACModule;
using System.Linq.Expressions;
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using static Terraria.ModLoader.PlayerDrawLayer;
using Everglow.Sources.Commons.Core.Utils;

namespace Everglow.Sources.Modules.FoodModule.Projectiles
{
    public class FryingPanNonMelee : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 3000;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        private bool Hit = true;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
            if (Projectile.timeLeft > 2850)
            {
                if (Projectile.ai[1] != 0)
                {
                    return true;
                }
                Projectile.soundDelay = 10;
                if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                {
                    Projectile.velocity.X = oldVelocity.X * -0.9f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                {
                    Projectile.velocity.Y = oldVelocity.Y * -0.9f;
                }
            }
            return false;
        }
        float[] OldRo = new float[12];
        bool CanHitProj = true;
        public override void AI()
        {
            if (CanHitProj)
            {
                foreach (Projectile proj in Main.projectile)
                {

                    if (proj.active && proj != Projectile)
                    {
                        if (Projectile.Hitbox.Intersects(proj.Hitbox))
                        {
                            Vector2 v1 = proj.velocity;
                            Vector2 v2 = Projectile.velocity;


                            float m1 = proj.width * proj.height * proj.knockBack * proj.scale;
                            float m2 = Projectile.width * Projectile.height * Projectile.knockBack * Projectile.scale / 100;

                            Vector2 newvelocity1 = (v1 * (m1 - m2) + 2 * m2 * v2) / (m1 + m2);
                            Vector2 newvelocity2 = (v2 * (m2 - m1) + 2 * m1 * v1) / (m1 + m2);

                            proj.velocity = Vector2.Normalize(newvelocity1) * v1.Length();
                            Projectile.velocity = newvelocity2;//这里是质心动量守恒的弹性碰撞，效果极其奇怪

                            CanHitProj = false;
                        }
                    }
                }
            }
            else
            {
                CanHitProj = true;
            }
            OldRo[0] = Projectile.rotation;
            for (int i = 11; i > 0; i--)
            {
                OldRo[i] = OldRo[i - 1];
            }
            float num7 = Projectile.velocity.Length();
            Player p = Main.player[Projectile.owner];
            Projectile.rotation += 0.15f * num7;
            float num6 = (float)Math.Sqrt((p.Center.X - Projectile.Center.X) * (p.Center.X - Projectile.Center.X) + (p.Center.Y - Projectile.Center.Y) * (p.Center.Y - Projectile.Center.Y));
            if (Projectile.timeLeft <= 2850)
            {
                int num3 = (int)Player.FindClosest(Projectile.Center, 1, 1);
                Projectile.velocity = Projectile.velocity * 0.95f + (p.Center - Projectile.Center) / num6 * 0.5f;
                Projectile.tileCollide = false;
            }
            else
            {
                if (num7 > 27.5f)
                {
                    Projectile.velocity *= 0.98f;
                }

            }
            /*if (num6 < 40 && num7 < 10)
            {
                Projectile.timeLeft = 0;
            }*/
            if (DelOme < 120)
            {
                DelOme += 3;
            }
        }

        public override void Kill(int timeLeft)
        {
        }
        private int DelOme = 0;
        private Effect ef;


        /* public override void PostDraw(Color lightColor)
         {
             Texture2D tg = ModContent.Request<Texture2D>("Everglow/Sources/Modules/FoodModule/Projectiles/FryingPanNonMelee").Value;
             Main.spriteBatch.Draw(tg, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(34f, 39f), Projectile.scale, SpriteEffects.None, 0);
             for (int i = 1; i < Projectile.oldPos.Length; ++i)
             {
                 if (Projectile.oldPos[i] == Vector2.Zero) break;
                 Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
                 Vector2 DrawPos = Projectile.oldPos[i] + new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                 Color c0 = Lighting.GetColor((int)(DrawPos.X / 16f), (int)(DrawPos.Y / 16f));
                 if (i % 3 == 0)
                 {
                     Main.spriteBatch.Draw(texture, DrawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(c0.R * (10 - i) / 10f), (int)(c0.G * (10 - i) / 10f), (int)(c0.B * (10 - i) / 10f), (int)(255 * (10 - i) / 10f)), OldRo[i], new Vector2(34f, 39f), Projectile.scale, SpriteEffects.None, 0);
                     Main.spriteBatch.Draw(tg, DrawPos - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color((int)(255 * (10 - i) / 10f), (int)(255 * (10 - i) / 10f), (int)(255 * (10 - i) / 10f), 0), OldRo[i], new Vector2(34f, 39f), Projectile.scale, SpriteEffects.None, 0);
                 }
                 Lighting.AddLight(DrawPos, 0, (float)(255 - Projectile.alpha) * 0.4f / 500f * (10 - i), 0);
             }
            /* Main.spriteBatch.End();
             Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
             List<Vertex2D> bars = new List<Vertex2D>();
             ef = (Effect)ModContent.Request<Effect>("MythMod/Effects/Trail2").Value;
             // 把所有的点都生成出来，按照顺序
             double o1 = Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
             double o2 = Math.Atan2(Projectile.oldVelocity.Y, Projectile.oldVelocity.X);
             double omega = Math.Abs(o2 - o1) % MathHelper.TwoPi;
             int width = (int)(120 - omega * 480);
             if (width <= 10)
             {
                 width = 10;
             }
             if (width <= 30)
             {
                 DelOme = width;
             }
             if (width > DelOme)
             {
                 width = DelOme;
             }
             for (int i = 1; i < Projectile.oldPos.Length; ++i)
             {
                 if (Projectile.oldPos[i] == Vector2.Zero) break;
                 //spriteBatch.Draw(Main.magicPixel, Projectile.oldPos[i] - Main.screenPosition,
                 //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);


                 var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                 normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                 var factor = i / (float)Projectile.oldPos.Length;
                 var color = Color.Lerp(Color.White, Color.Red, factor);
                 var w = MathHelper.Lerp(1f, 0.05f, factor);



                 bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(34, 34), color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                 bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(34, 34), color, new Vector3((float)Math.Sqrt(factor), 0, w)));
             }

             List<Vertex2D> triangleList = new List<Vertex2D>();

             if (bars.Count > 2)
             {

                 // 按照顺序连接三角形
                 triangleList.Add(bars[0]);
                 var vertex = new Vertex2D((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, Color.White, new Vector3(0, 0.5f, 1));
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
                 ef.Parameters["uTime"].SetValue(-(float)Main.time * 0.06f);
                 Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("MythMod/UIImages/heatmapCurseGreen").Value;
                 Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("MythMod/UIImages/Lightline").Value;
                 Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("MythMod/UIImages/FlameTrace").Value;
                 Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                 Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                 Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;


                 ef.CurrentTechnique.Passes[0].Apply();


                 Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                 Main.graphics.GraphicsDevice.RasterizerState = originalState;
                 Main.spriteBatch.End();
                 Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
             }*/


    }
}
