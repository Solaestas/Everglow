using Everglow.Sources.Commons.Core.ModuleSystem;
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
        private RenderTarget2D screen = null, render = null;
        private Effect ScreenWarp;
        void IModule.Load()
        {
            if (!Main.dedServ)
            {
                On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
                ScreenWarp = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/ScreenWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
        }

        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            // 直接从RT池子里取
            var renderTargets = Everglow.RenderTargetPool.GetRenderTarget2DArray(2);
            screen = renderTargets.Resource[0];
            render = renderTargets.Resource[1];

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
                ScreenWarp.Parameters["i"].SetValue(0.02f);//扭曲程度
                Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }

            screen = null;
            renderTargets.Release();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private bool DrawWarp(SpriteBatch sb)//扭曲层
        {
            bool flag = false;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if(proj.ModProjectile is IWarpProjectile ModProj)
                    {
                        flag = true;
                        ModProj.DrawWarp();
                    }
                }
            }
            sb.End();
            return flag;
        }
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
        }
    }
}
