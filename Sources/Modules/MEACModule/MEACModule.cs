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
        private RenderTarget2D screen,render;
        private Effect ScreenWarp;
        void IModule.Load()
        {
            if (!Main.dedServ)
            {
                On.Terraria.Main.LoadWorlds += Main_LoadWorlds;
                On.Terraria.Graphics.Effects.FilterManager.EndCapture += FilterManager_EndCapture;
                Main.OnResolutionChanged += Main_OnResolutionChanged;
                ScreenWarp = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/ScreenWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
        }

        private void Main_LoadWorlds(On.Terraria.Main.orig_LoadWorlds orig)
        {
            if(screen==null)
            {
                GraphicsDevice gd = Main.instance.GraphicsDevice;
                screen = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
                render = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
            }
            orig();
        }

        private void Main_OnResolutionChanged(Vector2 obj)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            screen = new RenderTarget2D(gd, (int)obj.X, (int)obj.Y);
            render = new RenderTarget2D(gd, (int)obj.X, (int)obj.Y);
        }
        private void FilterManager_EndCapture(On.Terraria.Graphics.Effects.FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
            GetOrig(graphicsDevice);
            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            bool flag = DrawWarp(Main.spriteBatch);
            if (flag)
            {
                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                ScreenWarp.CurrentTechnique.Passes[0].Apply();
                ScreenWarp.Parameters["tex0"].SetValue(Main.screenTargetSwap);
                ScreenWarp.Parameters["i"].SetValue(0.02f);//扭曲程度
                Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }
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
            screen.Dispose();
            screen = null;

            render.Dispose();
            render = null;
        }
    }
}
