using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Skeleton2D.Renderer.DrawCommands;
public class PipelineStateObject
{
	public Texture2D Texture = null;

	// 如果有需要再做绑定多贴图的
	public Texture2D[] TextureBindings = null;
}
