using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Base;

namespace Everglow.Sources.Commons.Core.VFX.Pipelines
{
    internal class RedPipeline : Pipeline
    {
        public override void BeginRender()
        {
            VFXManager.spriteBatch.Begin();
            effect.Value.Parameters["uColor"].SetValue(Color.Red.ToVector4());
            effect.Value.Parameters["uTransform"].SetValue(
                Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1)
                );
            effect.Value.CurrentTechnique.Passes[0].Apply();
        }
         
        public override void EndRender()
        {
            VFXManager.spriteBatch.End();
        }
        public override void Load()
        {
            effect = ModContent.Request<Effect>("Everglow/Sources/Commons/Core/VFX/Effect/PureColor");
        }
    }
}
