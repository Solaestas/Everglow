using Everglow.Common.Interfaces;
using Everglow.Common.ModuleSystem;
using Everglow.Common.ObjectPool;
using Everglow.Common.VFX;
using log4net;

namespace Everglow.Common;

/// <summary>
/// 手动（伪）单例（？），代替Everglow的作用，存在一些依赖关系
/// </summary>
public static class Ins
{
	public static VFXBatch Batch => Get<VFXBatch>();
	public static GraphicsDevice Device => Get<GraphicsDevice>();
	public static IHookManager HookManager => Get<IHookManager>();
	public static ILog Logger => Get<ILog>();
	public static IMainThreadContext MainThread => Get<IMainThreadContext>();
	public static ModuleManager ModuleManager => Get<ModuleManager>();
	public static RenderTargetPool RenderTargetPool => Get<RenderTargetPool>();
	public static IVFXManager VFXManager => Get<IVFXManager>();
	public static IVisualQualityController VisualQuality => Get<IVisualQualityController>();
	public static void Set<T>(T instance) where T : class
	{
		instances.Add(instance);
		Reference<T>.reference = new WeakReference<T>(instance);
	}

	public static void Clear()
	{
		foreach (var instance in instances.OfType<IDisposable>())
		{
			instance.Dispose();
		}
		instances.Clear();
	}

	public static T Get<T>() where T : class
	{
		if (Reference<T>.reference == null)
		{
			return null;
		}

		if (Reference<T>.reference.TryGetTarget(out var target))
		{
			return target;
		}

		return null;
	}

	private static List<object> instances = new();

	/// <summary>
	/// 我也不清楚用泛型弱引用快还是直接用字典存实例强制转换快，建议来个人测一下，我摸了（）
	/// </summary>
	/// <typeparam name="T"></typeparam>
	private static class Reference<T> where T : class
	{
		public static WeakReference<T> reference;
	}
}