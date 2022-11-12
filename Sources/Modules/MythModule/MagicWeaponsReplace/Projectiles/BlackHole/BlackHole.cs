using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core;
using Terraria.DataStructures;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BlackHole.Dust;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BlackHole
{
    //[Pipeline(typeof(BlackHolePipeline))]
    internal class BlackHole : ModProjectile
    {
        public static Projectile proj;//只能存在一个
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 1;
            Projectile.scale = 0;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type]=5000;
        }
        public static bool ProjActive()
        {
            if (proj != null)
                return proj.active && Main.projectile[proj.whoAmI].active && Main.projectile[proj.whoAmI].type == ModContent.ProjectileType<BlackHole>();
            else
                return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (ProjActive()&&proj!=Projectile)
            {
                Projectile.Kill();
            }
            else
            {
                proj = Projectile;
            }

        }
        public override void AI()
        {
            if(Projectile.timeLeft>20)
                proj.scale = MathHelper.Lerp(proj.scale,280,0.1f);
            else
                proj.scale = MathHelper.Lerp(proj.scale, 0, 0.25f);

            if (Main.rand.NextBool(6))//暗色粒子
            {
                DarkDust d = new DarkDust() { position = Projectile.Center + Main.rand.NextVector2Unit() * 30, velocity = Main.rand.NextVector2Unit() * 6, scale = 0.8f, time_max = 30 };
                VFXManager.Add(d);
            }
            if (Main.rand.NextBool(4))//光亮粒子
            {
                Color c = new Color(0.2f, 0.7f, 1f);//颜色
                LightDust d = new LightDust() {drawColor= c, position = Projectile.Center + Main.rand.NextVector2Unit() * 30, velocity = Main.rand.NextVector2Unit() * 6, scale = 0.2f, time_max = 30 };
                VFXManager.Add(d);
            }

        }
        public static Vector2 Projection(Vector3 vec, Vector2 center)
        {
            float k1 = -1200 / (vec.Z - 1200);
            Vector2 v = new Vector2(vec.X, vec.Y);
            return v + (k1 - 1) * (v - center);
        }
        public static void DrawRing(Projectile Projectile,bool front = false)//分前后两段(由front参数决定)绘制环
        {
            
            Color c = new Color(0.2f, 0.7f, 1f);//环的颜色

            float time = (float)Main.timeForVisualEffects * 0.005f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            List<Vertex2D> vertices = new();
            (int, int) r = front ? (50, 100) : (0, 50);
            for (int i = r.Item1; i <= r.Item2; i++)
            {
                Vector3 v3 = Vector3.Transform(Vector3.UnitX * Projectile.scale * 0.8f, Matrix.CreateRotationY(i * MathHelper.TwoPi / 100));

                Vector2 v2 = Projection(new Vector3(Projectile.Center.X + v3.X - Main.screenPosition.X, Projectile.Center.Y + v3.Y - Main.screenPosition.Y, v3.Z), new Vector2(Main.screenWidth, Main.screenHeight) / 2);
                vertices.Add(new Vertex2D(v2, c * 0.8f, new Vector3(time + i / 50f, 0, 0)));

                v3 *= 2.7f;
                v2 = Projection(new Vector3(Projectile.Center.X + v3.X - Main.screenPosition.X, Projectile.Center.Y + v3.Y - Main.screenPosition.Y, v3.Z), new Vector2(Main.screenWidth, Main.screenHeight) / 2);
                vertices.Add(new Vertex2D(v2, c, new Vector3(time + i / 50f, 1, 0)));
            }
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Projectiles/BlackHole/tex3").Value;
            ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Projectiles/BlackHole/Colorize", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value.CurrentTechnique.Passes[0].Apply();
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Main.drawToScreen)
            {
                DrawRing(Projectile);
                
            }
           else//低特效
            {
                DrawRing(Projectile);
                Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
                Main.spriteBatch.Draw(tex, proj.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, proj.scale / 255f, 0, 0);
                BlackHole.DrawRing(proj, true);

            }
            return false;
        }
    }
    public class TemporarySys : ModSystem//暂时用一个ModSystem上滤镜
    {
        public override void Load()
        {
            On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
        }

        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            if(BlackHole.ProjActive())
            {
                Projectile proj = BlackHole.proj;
                var sb = Main.spriteBatch;
                var gd = Main.instance.GraphicsDevice;

                gd.SetRenderTarget(Main.screenTargetSwap);
                gd.Clear(Color.Transparent);
                sb.Begin();
                sb.Draw(Main.screenTarget,new Rectangle(0,0,Main.screenWidth,Main.screenHeight),Color.White);
                sb.End();

                gd.SetRenderTarget(Main.screenTarget);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);
                Effect eff = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlackHole",ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                Vector2 scRes = new Vector2(Main.screenWidth,Main.screenHeight);
                Vector2 pos = Vector2.Transform(proj.Center - Main.screenPosition, Main.Transform);
                eff.Parameters["uPosition"].SetValue(pos/scRes);
                eff.Parameters["uRatio"].SetValue(scRes.X/scRes.Y);
                eff.Parameters["uRadius"].SetValue(0.001f*proj.scale*Main.Transform.M11);//乘了一个总缩放系数
                eff.Parameters["uIntensity"].SetValue(3f);//扭曲程度，可以调节这个值来实现不同效果
                eff.CurrentTechnique.Passes[0].Apply();
                sb.Draw(Main.screenTargetSwap, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,Main.DefaultSamplerState,DepthStencilState.Default,RasterizerState.CullNone,null,Main.Transform);
                //绘制黑洞以及前半环
                Texture2D tex = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Projectiles/BlackHole/BlackHole").Value;
                sb.Draw(tex,proj.Center-Main.screenPosition,null,Color.White,0,tex.Size()/2,proj.scale/255f,0,0);
                BlackHole.DrawRing(proj,true);
                sb.End();
            }
            orig(self,finalTexture,screenTarget1,screenTarget2,clearColor);
        }
    }
    
    
}
