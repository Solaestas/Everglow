using Everglow.Commons.Interfaces;
using Everglow.Commons.VFX;

namespace Everglow.Commons.Utils;

public static class VFXUtils
{
	/// <summary>
	/// 添加一个Visual实例
	/// </summary>
	/// <param name="visual"></param>
	public static void Add(this IVisual visual)
	{
		if (NetUtils.IsServer)
			return;
		VFXManager.Instance.Add(visual);
	}
}
