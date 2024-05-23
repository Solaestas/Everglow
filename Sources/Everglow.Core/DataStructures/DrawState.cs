namespace Everglow.Commons.DataStructures;

/// <summary>
/// 绘制的基本状态
/// </summary>
/// <param name="BlendState"> </param>
/// <param name="SamplerState"> </param>
/// <param name="DepthStencilState"> </param>
/// <param name="RasterizerState"> </param>
public record struct DrawState(
	BlendState BlendState,
	SamplerState SamplerState,
	DepthStencilState DepthStencilState,
	RasterizerState RasterizerState)
{
}
