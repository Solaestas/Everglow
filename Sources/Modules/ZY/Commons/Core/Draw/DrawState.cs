namespace Everglow.Sources.Modules.ZY.Commons.Core.Draw;

internal class DrawState
{
    public readonly BlendState blendState;
    public readonly SamplerState samplerState;
    public readonly DepthStencilState depthStencilState;
    public readonly RasterizerState rasterizerState;

    public DrawState(BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
    {
        this.blendState = blendState;
        this.samplerState = samplerState;
        this.depthStencilState = depthStencilState;
        this.rasterizerState = rasterizerState;
    }

    public void SetState(GraphicsDevice graphicsDevice)
    {
        graphicsDevice.BlendState = blendState;
        graphicsDevice.SamplerStates[0] = samplerState;
        graphicsDevice.DepthStencilState = depthStencilState;
        graphicsDevice.RasterizerState = rasterizerState;
    }

    public DrawState Reset(GraphicsDevice graphicsDevice)
    {
        DrawState oldState = graphicsDevice.GetState();
        SetState(graphicsDevice);
        return oldState;
    }

    public readonly DrawState Default = new DrawState(BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
    public readonly DrawState Additive = new DrawState(BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
}
