using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Skeleton2D.Renderer.DrawCommands;

/// <summary>
/// Stores pipeline state data, maybe critical to batching
/// </summary>
public class PipelineStateObject
{
	public Texture2D Texture = null;

	// 如果有需要再做绑定多贴图的
	public Texture2D[] TextureBindings = null;

	public Matrix Model;

	public Matrix View;

	public Matrix Projection;

	public RasterizerState RasterizerState = RasterizerState.CullNone;

	public BlendState BlendState = BlendState.AlphaBlend;
}