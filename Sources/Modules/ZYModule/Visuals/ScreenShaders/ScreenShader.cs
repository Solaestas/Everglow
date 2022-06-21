
using Everglow.Sources.Modules.ZYModule.Commons.Function;

using Extensions;

using ReLogic.Content;

namespace Everglow.Sources.Modules.ZYModule.Visuals.ScreenShaders
{
    internal class ScreenShader
    {
        public int time;
        public float opacity;
        public ScreenParameter parameters;
        public bool active;
        public EffectParameterCollection effectParameters;
        public EffectPass effectPass;
        public ScreenShader(EffectType effectType, ScreenParameter parameters)
        {
            this.parameters = parameters;
            var effect = effectType.GetValue();
            effectPass = effect.CurrentTechnique.Passes[0];
            effectParameters = effect.Parameters;
            if(parameters.HasFlag(ScreenParameter.uNoise))
            {
                effectParameters["uNoise"].SetValue(TextureType.Noise.GetValue(false));
            }
        }
        public void AutoSetParameters()
        {
            if (parameters.HasFlag(ScreenParameter.uResolution))
            {
                effectParameters["uResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            }
            if (parameters.HasFlag(ScreenParameter.uInvResolution))
            {
                effectParameters["uResolution"].SetValue(new Vector2(1f / Main.screenWidth, 1f / Main.screenHeight));
            }
            if (parameters.HasFlag(ScreenParameter.uOpacity))
            {
                effectParameters["uOpacity"].SetValue(opacity);
            }
            if (parameters.HasFlag(ScreenParameter.uTime))
            {
                effectParameters["uTime"].SetValue(time);
            }
            if (parameters.HasFlag(ScreenParameter.uInvZoom))
            {
                effectParameters["uTime"].SetValue(new Vector2(1f / Main.GameViewMatrix.Zoom.X, 1f / Main.GameViewMatrix.Zoom.Y));
            }
            if (parameters.HasFlag(ScreenParameter.uZoomMatrix))
            {
                effectParameters["uZoomMatrix"].SetValue(Main.GameViewMatrix.ZoomMatrix);
            }
        }
        public virtual void SetParameters() { }
        public virtual void PreDraw() { }
        public virtual void PostDraw() { }
    }
}
