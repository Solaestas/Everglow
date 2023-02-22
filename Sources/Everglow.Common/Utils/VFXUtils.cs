using Everglow.Common.Interfaces;
using Everglow.Common.VFX;

namespace Everglow.Common.Utils;

public static class VFXUtils
{
	/// <summary>
	/// 添加一个Visual实例
	/// </summary>
	/// <param name="visual"></param>
	public static void Add(IVisual visual)
	{
		if (NetUtils.IsServer)
		{
			return;
		}
		VFXManager.Instance.Add(visual);
	}
}
