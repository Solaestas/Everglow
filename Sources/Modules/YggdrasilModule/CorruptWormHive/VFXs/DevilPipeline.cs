﻿using Everglow.Sources.Modules.YggdrasilModule.Common;
using Everglow.Sources.Commons.Core.VFX.Pipelines;

namespace Everglow.Sources.Modules.YggdrasilModule.CorruptWormHive.VFXs
{
    internal class DevilPipeline : PostPipeline
    {

        public override void Render(RenderTarget2D rt2D)
        {
            var sb = Main.spriteBatch;
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.Value.Parameters["tex1"].SetValue(YggdrasilContent.QuickTexture("CorruptWormHive/VFXs/DeathSickle_Color"));
            effect.Value.Parameters["uTransform"].SetValue(
                Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1)
                );
            effect.Value.CurrentTechnique.Passes[0].Apply();
            sb.Draw(rt2D, Vector2.Zero, Color.White);
            sb.End();
        }

        public override void Load()
        {
            effect = ModContent.Request<Effect>("Everglow/Sources/Modules/YggdrasilModule/CorruptWormHive/VFXs/FlameColor");
        }
    }
}
