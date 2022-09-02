using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using Everglow.Sources.Commons.Function.ObjectPool;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using ReLogic.Content;

namespace Everglow.Sources.Commons.Core.VFX.Pipelines
{
    internal class BloomPipeline : PostPipeline
    {
        private RenderTarget2D bloomScreen;
        private RenderTarget2D bloomTempScreen;
        //拉伸模糊的倍率
        private const int ReverseScale = 2;
        public override void Load()
        {
            Everglow.MainThreadContext.AddTask(() =>
            {
                AllocateRenderTarget();
            });
            Everglow.HookSystem.AddMethod(() =>
            {
                bloomScreen.Dispose();
                bloomTempScreen.Dispose();
                AllocateRenderTarget();
            }, CallOpportunity.ResolutionChanged, "Realloc RenderTarget");
            effect = ModContent.Request<Effect>("Everglow/Sources/Commons/Core/VFX/Effect/Bloom");
        }
        private void AllocateRenderTarget()
        {
            var gd = Main.instance.GraphicsDevice;
            bloomScreen = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth / ReverseScale, gd.PresentationParameters.BackBufferHeight / ReverseScale,
                false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
            bloomTempScreen = new RenderTarget2D(gd, gd.PresentationParameters.BackBufferWidth / ReverseScale, gd.PresentationParameters.BackBufferHeight / ReverseScale,
                false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
        }
        public override void Render(RenderTarget2D rt2D)
        {
            var sb = Main.spriteBatch;
            var gd = Main.instance.GraphicsDevice;
            var effect = this.effect.Value;
            //将当前rt2D的亮部画到大小为原来1/ReverseScale大小的bloomScreen上 
            //先拉伸模糊一遍貌似会导致一像素的光点不会被采样到？？
            gd.SetRenderTarget(bloomScreen);
            gd.Clear(Color.Transparent);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
            effect.Parameters["uIntensity"].SetValue(1);
            effect.Parameters["uTransform"].SetValue(
                Matrix.CreateOrthographicOffCenter(0, bloomScreen.Width, bloomScreen.Height, 0, 0, 1)
                );
            effect.Parameters["uLightLimit"].SetValue(0.3f);
            effect.Parameters["uSize"].SetValue(bloomScreen.Size());
            effect.CurrentTechnique.Passes["GetLight"].Apply();
            sb.Draw(rt2D, new Rectangle(0, 0, bloomScreen.Width, bloomScreen.Height), Color.White);

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
            sb.End();


            //叠加
            RenderTarget2D oldRt2D = VFXManager.Instance.CurrentRenderTarget;//先获得旧的Rt2D
            VFXManager.Instance.SwapRenderTarget();//交换Rt2D
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            sb.Draw(oldRt2D, Vector2.Zero, Color.White);//先把旧的画上去
            sb.Draw(rt2D, Vector2.Zero, Color.White);//绘制特效
            //绘制发光
            gd.BlendState = BlendState.Additive;
            sb.Draw(bloomScreen, new Rectangle(0, 0, oldRt2D.Width, oldRt2D.Height), Color.White);
            sb.End();
        }
    }
}
