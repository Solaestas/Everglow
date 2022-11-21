using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Function.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.MEACModule
{
    internal class MEACModule : IModule
    {
        string IModule.Name => "MEAC";
        private RenderTarget2D screen = null, bloomTarget1=null,bloomTarget2=null;
        private Effect ScreenWarp;
        void IModule.Load()
        {
            if (!Main.dedServ)
            {
                Main.OnResolutionChanged += Main_OnResolutionChanged;
                On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
                ScreenWarp = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/ScreenWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
        }

        void CreateRender(Vector2 v)
        {
            bloomTarget1 = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)v.X / 3, (int)v.Y / 3);
            bloomTarget2 = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)v.X / 3, (int)v.Y / 3);
        }
        private void Main_OnResolutionChanged(Vector2 obj)
        {
            CreateRender(obj);
        }
        private bool HasBloom()
        {
            bool flag = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.ModProjectile is IBloomProjectile ModProj)
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }
        private void UseBloom(GraphicsDevice graphicsDevice)
        {
            if (HasBloom())
            {
                Effect Bloom = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/Bloom1",ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                //保存原图
                graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();

                //在screen上绘制发光部分
                graphicsDevice.SetRenderTarget(screen);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                DrawBloom(Main.spriteBatch);
                Main.spriteBatch.End();

                //取样
                
                graphicsDevice.SetRenderTarget(bloomTarget2);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Bloom.CurrentTechnique.Passes[0].Apply();//取亮度超过m值的部分
                Bloom.Parameters["m"].SetValue(0.5f);
                Main.spriteBatch.Draw(screen, new Rectangle(0, 0, Main.screenWidth / 3, Main.screenHeight / 3), Color.White);
                Main.spriteBatch.End();

                //处理
                
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Bloom.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / 3f);
                Bloom.Parameters["uRange"].SetValue(1.5f);//范围
                Bloom.Parameters["uIntensity"].SetValue(0.97f);//发光强度
                for (int i = 0; i < 2; i++)//交替使用两个RenderTarget2D，进行多次模糊
                {
                    Bloom.CurrentTechnique.Passes["GlurV"].Apply();//横向
                    graphicsDevice.SetRenderTarget(bloomTarget1);
                    graphicsDevice.Clear(Color.Transparent);
                    Main.spriteBatch.Draw(bloomTarget2, Vector2.Zero, Color.White);

                    Bloom.CurrentTechnique.Passes["GlurH"].Apply();//纵向
                    graphicsDevice.SetRenderTarget(bloomTarget2);
                    graphicsDevice.Clear(Color.Transparent);
                    Main.spriteBatch.Draw(bloomTarget1, Vector2.Zero, Color.White);
                }
                Main.spriteBatch.End();
                
                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                //叠加
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.Draw(bloomTarget2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                Main.spriteBatch.End();
            }
        }
        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            // 直接从RT池子里取
            var renderTargets = Everglow.RenderTargetPool.GetRenderTarget2DArray(1);
            screen = renderTargets.Resource[0];

            if (bloomTarget1 == null)
            {
                CreateRender(new Vector2(Main.screenWidth, Main.screenHeight));
            }
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
            
            GetOrig(graphicsDevice);
            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            bool flag = DrawWarp(Main.spriteBatch);

            if (flag)
            {
                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                graphicsDevice.Textures[1] = Main.screenTargetSwap;
                graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                ScreenWarp.CurrentTechnique.Passes[0].Apply();
                ScreenWarp.Parameters["i"].SetValue(0.025f);//扭曲程度
                Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }
            UseBloom(graphicsDevice);
            screen = null;
            renderTargets.Release();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private void DrawBloom(SpriteBatch sb)//发光层
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.ModProjectile is IBloomProjectile ModProj)
                    {
                        ModProj.DrawBloom();
                    }
                }
            }
        }
        private bool DrawWarp(SpriteBatch sb)//扭曲层
        {
            bool flag = false;
            VFXManager.spriteBatch.Begin();
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (proj.ModProjectile is IWarpProjectile ModProj2)
                    {
                        flag = true;
                        ModProj2.DrawWarp(VFXManager.spriteBatch);
                    }
                }
            }
            VFXManager.spriteBatch.End();
            return flag;
        }
        /// <summary>
        /// 往screen上保存原图
        /// </summary>
        /// <param name="graphicsDevice"></param>
        private void GetOrig(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(screen);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }

        void IModule.Unload()
        {
            bool wait = true;
            Main.QueueMainThreadAction(() =>
            {
                bloomTarget1?.Dispose();
                bloomTarget2?.Dispose();
                wait = false;
            });
            while (wait) ;
        }
    }
}
