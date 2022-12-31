using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Function.Vertex;
using ReLogic.Content;

namespace Everglow.Sources.Modules.MythModule.Common.VFXPipelines
{
    internal class BasePipeline : Pipeline
    {
        public override void BeginRender()
        {
            VFXManager.spriteBatch.Begin();
            effect.Value.Parameters["uTransform"].SetValue(Main.Transform*
                Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
            effect.Value.CurrentTechnique.Passes[0].Apply();
        }

        public override void EndRender()
        {
            VFXManager.spriteBatch.End();
        }

        public override void Load()
        {
            effect = VFXManager.DefaultEffect;
        }
    }
}
