using Everglow.Commons.Interfaces;

namespace Everglow.Commons.Utilities;

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
		Ins.VFXManager.Add(visual);
	}
}
