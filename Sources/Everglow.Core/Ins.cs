using Everglow.Common.Enums;
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
	public static ILog Logger { get; private set; }
	public static GraphicsDevice Device { get; private set; }
	public static IVisualQualityController VisualQuality { get; private set; }
	public static IHookManager HookManager { get; private set; }
	public static ModuleManager ModuleManager { get; private set; }
	public static IMainThreadContext MainThread { get; private set; }
	public static RenderTargetPool RenderTargetPool { get; private set; }
	public static VFXBatch Batch { get; private set; }
	public static IVFXManager VFXManager { get; private set; }

	public static void SetInstance(
		ILog logger,
		GraphicsDevice device,
		IVisualQualityController visualQuality,
		IHookManager hookManager,
		ModuleManager moduleManager,
		IMainThreadContext mainThread,
		RenderTargetPool renderTargetPool,
		VFXBatch batch,
		IVFXManager manager)
	{
		Logger = logger;
		Device = device;
		VisualQuality = visualQuality;
		HookManager = hookManager;
		ModuleManager = moduleManager;
		MainThread = mainThread;
		RenderTargetPool = renderTargetPool;
		Batch = batch;
		VFXManager = manager;
	}

	public static void DisposeAll()
	{
		Logger = null;
		Device = null;
		VisualQuality = null;
		HookManager?.Dispose();
		HookManager = null;
		ModuleManager = null;
		MainThread = null;
		RenderTargetPool = null;
		Batch?.Dispose();
		Batch = null;
		VFXManager?.Dispose();
		VFXManager = null;
	}
}