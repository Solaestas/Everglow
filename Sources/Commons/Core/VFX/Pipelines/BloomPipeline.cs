using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using Everglow.Sources.Commons.Function.ObjectPool;
using Everglow.Sources.Modules.ZYModule.Commons.Function;

namespace Everglow.Sources.Commons.Core.VFX.Pipelines
{
    internal class BloomPipeline : Pipeline
    {
        private RenderTarget2D bloomScreen;
        private RenderTarget2D bloomTempScreen;
        private RenderTarget2D tempScreen;
        private const int Shrink = 1;
        public override void Load()
        {
            Everglow.MainThreadContext.AddTask(() =>
            {
                AllocateRenderTarget();
            });
            Everglow.HookSystem.AddMethod(() =>
            {
                tempScreen.Dispose();
                bloomScreen.Dispose();
                bloomTempScreen.Dispose();
                AllocateRenderTarget();
            }, CallOpportunity.ResolutionChanged, "Realloc RenderTarget");
            effect = ModContent.Request<Effect>("Everglow/Sources/Commons/Core/VFX/Effect/Bloom");
        }
        private void AllocateRenderTarget()
        {
            var gd = Main.instance.GraphicsDevice;
            tempScreen = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth, gd.PresentationParameters.BackBufferHeight,
                false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
            bloomScreen = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth / Shrink, gd.PresentationParameters.BackBufferHeight / Shrink,
                false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
            bloomTempScreen = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth / Shrink, gd.PresentationParameters.BackBufferHeight / Shrink,
                false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
        }
        public override void BeginRender()
        {
            var gd = Main.instance.GraphicsDevice;
            gd.SetRenderTarget(tempScreen);
            gd.Clear(Color.Transparent);
            VFXManager.spriteBatch.Begin(BlendState.AlphaBlend);
        }
        public override void Render(IEnumerable<IVisual> visuals)
        {
            foreach (var visual in visuals)
            {
                visual.Draw(); 
            }
        }
        public override void EndRender()
        {
            VFXManager.spriteBatch.End();
            var gd = Main.instance.GraphicsDevice;
            var sb = Main.spriteBatch;
            var effect = this.effect.Value;

            //将当前rt2D的亮部画到大小为原来1/1大小的bloomScreen上   //先拉伸模糊一遍会很怪，不知道为什么
            gd.SetRenderTarget(bloomScreen);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            effect.Parameters["uIntensity"].SetValue(1);
            effect.Parameters["uTransform"].SetValue(
                Matrix.CreateOrthographicOffCenter(0, bloomScreen.Width, bloomScreen.Height, 0, 0, 1)
                );
            effect.Parameters["uLightLimit"].SetValue(0.3f);
            effect.Parameters["uSize"].SetValue(bloomScreen.Size());
            effect.CurrentTechnique.Passes["GetLight"].Apply();
            sb.Draw(tempScreen, new Rectangle(0, 0, bloomScreen.Width, bloomScreen.Height), Color.White);
            //反复模糊三次
            for (int i = 0; i < 3; i++)
            {
                gd.SetRenderTarget(bloomTempScreen);
                gd.Clear(Color.Transparent);
                effect.CurrentTechnique.Passes["BloomH"].Apply();
                sb.Draw(bloomScreen, Vector2.Zero, Color.White);

                gd.SetRenderTarget(bloomScreen);
                gd.Clear(Color.Transparent);
                effect.CurrentTechnique.Passes["BloomV"].Apply();
                sb.Draw(bloomTempScreen, Vector2.Zero, Color.White);
            }

            //叠加
            sb.End();
            RenderTarget2D nextRenderTarget = VFXManager.Instance.NextRenderTarget;
            gd.SetRenderTarget(nextRenderTarget);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            gd.Clear(Color.Transparent);
            sb.Draw(VFXManager.Instance.CurrentRenderTarget, Vector2.Zero, Color.White);
            sb.Draw(tempScreen, Vector2.Zero, Color.White);

            gd.BlendState = BlendState.Additive;
            sb.Draw(bloomScreen, new Rectangle(0, 0, nextRenderTarget.Width, nextRenderTarget.Height), Color.White);
            sb.End();

            VFXManager.Instance.SwitchRenderTarget();
        }
    }
}
