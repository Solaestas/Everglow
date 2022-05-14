using Everglow.Sources.Commons.Function.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.Test
{
    public class TestSystem : ModSystem
    {
        private static string PathPrefix
        {
            get
            {
                return "Everglow/Sources/Modules/Test/";
            }
        }

        private RenderTargetPool m_renderTargetPool;
        private Asset<Effect> m_screenBloomEffect;

        public static Asset<Effect> GetEffect(string path)
        {
            return ModContent.Request<Effect>(PathPrefix + path);
        }
        public override void PostSetupContent()
        {
            m_screenBloomEffect = GetEffect("Effects/BloomTest");
            base.PostSetupContent();
        }

        public override void PostDrawTiles()
        {
            if (!m_screenBloomEffect.IsLoaded)
            {
                m_screenBloomEffect.Wait();
            }

            var graphicsDevice = Main.graphics.GraphicsDevice;
            var sb = Main.spriteBatch;

            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            graphicsDevice.SetRenderTarget(Main.screenTarget);
            graphicsDevice.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            var effect = m_screenBloomEffect.Value;

            effect.Parameters["uScreenResolution"].SetValue(new Vector2(graphicsDevice.Viewport.Width, 
                graphicsDevice.Viewport.Height));
            effect.Parameters["m"].SetValue(0.1f);
            effect.Parameters["uIntensity"].SetValue(0.1f);
            effect.Parameters["uRange"].SetValue(1.0f);

            effect.CurrentTechnique.Passes["GlurH"].Apply();
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            base.PostDrawTiles();
        }
    }
}
