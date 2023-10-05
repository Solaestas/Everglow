using System.Reflection;
using System.Runtime.CompilerServices;
using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Renderers;

namespace Everglow.Commons.Hooks;

public class ILHookHandler : IHookHandler
{
	public bool Enable
	{
		get => Hook.IsApplied;
		set
		{
			if (value)
			{
				Hook.Apply();
			}
			else
			{
				Hook.Undo();
			}
		}
	}

	public ILHook Hook { get; init; }

	public string Name => Hook.Method.Name;
}

public class OnHookHandler : IHookHandler
{
	public bool Enable
	{
		get => Hook.IsApplied;
		set
		{
			if (value)
			{
				Hook.Apply();
			}
			else
			{
				Hook.Undo();
			}
		}
	}

	public Hook Hook { get; init; }

	public string Name => Hook.Target.Name;
}

public record HookHandler(CodeLayer Layer, Delegate Hook, string Name) : IHookHandler
{
	public bool Enable { get; set; } = true;
}

public class HookManager : ModSystem
{
	private TrueHookManager _manager;

	public HookManager()
	{
		_manager = Ins.HookManager as TrueHookManager;
		Debug.Assert(_manager is not null);
	}

	public static void ClearReflectionCache()
	{
		ReflectionHelper.AssemblyCache.Clear();
		ReflectionHelper.AssembliesCache.Clear();
		ReflectionHelper.ResolveReflectionCache.Clear();
	}

	public void Invoke(CodeLayer layer)
	{
		foreach (var handler in _manager.hooks[layer].Where(h => h.Enable))
		{
			try
			{
				var hook = (Action)handler.Hook;
				hook.Invoke();
			}
			catch (Exception ex)
			{
				Debug.Fail(ex.ToString());
				Ins.Logger.Error($"{handler} 抛出了异常 {ex}");
				handler.Enable = false;
			}
		}
	}

	public override void Load()
	{
		On_Main.DrawDust += Main_DrawDust;
		On_Main.DrawProjectiles += Main_DrawProjectiles;
		On_Main.DrawNPCs += Main_DrawNPCs;
		On_LegacyPlayerRenderer.DrawPlayers += LegacyPlayerRenderer_DrawPlayers;
		On_WorldGen.playWorldCallBack += WorldGen_playWorldCallBack;

		On_WorldGen.SaveAndQuit += WorldGen_SaveAndQuit;
		On_Main.DrawMiscMapIcons += Main_DrawMiscMapIcons;
		On_WorldGen.serverLoadWorldCallBack += WorldGen_serverLoadWorldCallBack;
		On_Main.DrawBG += Main_DrawBG;
		On_Main.DrawBackground += Main_DrawBackground;
		On_Main.DoDraw_WallsTilesNPCs += Main_DoDraw_WallsTilesNPCs;
		On_FilterManager.EndCapture += On_FilterManager_EndCapture;
		Main.OnResolutionChanged += Main_OnResolutionChanged;
	}

	private void On_FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
	{
		Invoke(CodeLayer.PreDrawFilter);
		orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
	}

	public override void PostDrawTiles()
	{
		Invoke(CodeLayer.PostDrawTiles);
	}

	public override void PostUpdateDusts()
	{
		Invoke(CodeLayer.PostUpdateDusts);
	}

	public override void PostUpdateEverything()
	{
		Invoke(CodeLayer.PostUpdateEverything);
	}

	public override void PostUpdateInvasions()
	{
		Invoke(CodeLayer.PostUpdateInvasions);
	}

	public override void PostUpdateNPCs()
	{
		Invoke(CodeLayer.PostUpdateNPCs);
	}

	public override void PostUpdatePlayers()
	{
		Invoke(CodeLayer.PostUpdatePlayers);
	}

	public override void PostUpdateProjectiles()
	{
		Invoke(CodeLayer.PostUpdateProjectiles);
	}

	public void RemoveHook(IHookHandler handler)
	{
		_manager.RemoveHook(handler);
	}

	public override void Unload()
	{
		On_Main.DrawDust -= Main_DrawDust;
		On_Main.DrawProjectiles -= Main_DrawProjectiles;
		On_Main.DrawNPCs -= Main_DrawNPCs;
		On_LegacyPlayerRenderer.DrawPlayers -= LegacyPlayerRenderer_DrawPlayers;
		On_WorldGen.playWorldCallBack -= WorldGen_playWorldCallBack;

		On_WorldGen.SaveAndQuit -= WorldGen_SaveAndQuit;
		On_Main.DrawMiscMapIcons -= Main_DrawMiscMapIcons;
		On_WorldGen.serverLoadWorldCallBack -= WorldGen_serverLoadWorldCallBack;
		On_Main.DrawBG -= Main_DrawBG;
		On_Main.DrawBackground -= Main_DrawBackground;
		On_Main.DoDraw_WallsTilesNPCs -= Main_DoDraw_WallsTilesNPCs;
		Main.OnResolutionChanged -= Main_OnResolutionChanged;
	}

	private void LegacyPlayerRenderer_DrawPlayers(On_LegacyPlayerRenderer.orig_DrawPlayers orig, LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, IEnumerable<Player> players)
	{
		orig.Invoke(self, camera, players);
		Invoke(CodeLayer.PostDrawPlayers);
	}

	private void Main_DoDraw_WallsTilesNPCs(On_Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
	{
		Invoke(CodeLayer.PostDrawBG);
		orig(self);
	}

	private void Main_DrawBackground(On_Main.orig_DrawBackground orig, Main self)
	{
		if (_manager.disableFlags.Has(TerrariaFunction.DrawBackground))
		{
			return;
		}

		orig(self);
	}

	private void Main_DrawBG(On_Main.orig_DrawBG orig, Main self)
	{
		if (_manager.disableFlags.Has(TerrariaFunction.DrawSkyAndHell))
		{
			return;
		}

		orig(self);
	}

	private void Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
	{
		orig.Invoke(self);
		Invoke(CodeLayer.PostDrawDusts);
	}

	private void Main_DrawMiscMapIcons(On_Main.orig_DrawMiscMapIcons orig, Main self, SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string mouseTextString)
	{
		orig(self, spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, ref mouseTextString);
		foreach (var handler in _manager.hooks[CodeLayer.PostDrawMapIcons].Where(h => h.Enable))
		{
			try
			{
				var hook = (Action<Vector2, Vector2, Rectangle?, float>)handler.Hook;
				hook.Invoke(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
			}
			catch (Exception ex)
			{
				Debug.Fail(ex.ToString());
				Ins.Logger.Error($"{handler} 抛出了异常 {ex}");
				handler.Enable = false;
			}
		}
	}

	private void Main_DrawNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
	{
		orig.Invoke(self, behindTiles);
		if (!behindTiles)
		{
			Invoke(CodeLayer.PostDrawNPCs);
		}
	}

	private void Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
	{
		orig.Invoke(self);
		Invoke(CodeLayer.PostDrawProjectiles);
	}

	private void Main_OnResolutionChanged(Vector2 obj)
	{
		foreach (var handler in _manager.hooks[CodeLayer.ResolutionChanged].Where(h => h.Enable))
		{
			try
			{
				var hook = (Action<Vector2>)handler.Hook;
				hook.Invoke(obj);
			}
			catch (Exception ex)
			{
				Debug.Fail(ex.ToString());
				Ins.Logger.Error($"{handler} 抛出了异常 {ex}");
				handler.Enable = false;
			}
		}
	}

	private void WorldGen_playWorldCallBack(On_WorldGen.orig_playWorldCallBack orig, object threadContext)
	{
		orig(threadContext);
		Invoke(CodeLayer.PostEnterWorld_Single);
	}

	private void WorldGen_SaveAndQuit(On_WorldGen.orig_SaveAndQuit orig, Action callback)
	{
		orig(callback);
		Invoke(CodeLayer.PostExitWorld_Single);
	}

	private void WorldGen_serverLoadWorldCallBack(On_WorldGen.orig_serverLoadWorldCallBack orig)
	{
		orig();
		Invoke(CodeLayer.PostEnterWorld_Server);
	}

	public class TrueHookManager : IHookManager
	{
		public TerrariaFunction disableFlags;

		public Dictionary<CodeLayer, List<HookHandler>> hooks = requiredHookType.Keys.ToDictionary(l => l, l => new List<HookHandler>());

		private static readonly Dictionary<CodeLayer, Type> requiredHookType = new Dictionary<CodeLayer, Type>
		{
			[CodeLayer.PreDrawFilter] = typeof(Action),
			[CodeLayer.PostDrawTiles] = typeof(Action),
			[CodeLayer.PostDrawProjectiles] = typeof(Action),
			[CodeLayer.PostDrawDusts] = typeof(Action),
			[CodeLayer.PostDrawNPCs] = typeof(Action),
			[CodeLayer.PostDrawPlayers] = typeof(Action),
			[CodeLayer.PostDrawMapIcons] = typeof(Action<Vector2, Vector2, Rectangle?, float>),
			[CodeLayer.PostDrawBG] = typeof(Action),
			[CodeLayer.PostUpdateEverything] = typeof(Action),
			[CodeLayer.PostUpdateProjectiles] = typeof(Action),
			[CodeLayer.PostUpdatePlayers] = typeof(Action),
			[CodeLayer.PostUpdateNPCs] = typeof(Action),
			[CodeLayer.PostUpdateDusts] = typeof(Action),
			[CodeLayer.PostUpdateInvasions] = typeof(Action),
			[CodeLayer.PostEnterWorld_Single] = typeof(Action),
			[CodeLayer.PostExitWorld_Single] = typeof(Action),
			[CodeLayer.PostEnterWorld_Server] = typeof(Action),
			[CodeLayer.ResolutionChanged] = typeof(Action<Vector2>),
		};

		private List<IDisposable> monoHooks = new();

		public IHookHandler AddHook<T>(CodeLayer layer, T hook, [CallerMemberName]string name = default, [CallerFilePath]string file = default) where T : Delegate
		{
			Debug.Assert(typeof(T).IsAssignableTo(requiredHookType[layer]), $"Hook type not match, {Wrapper.Create(requiredHookType[layer])} != {Wrapper.Create(typeof(T))}");
			var handler = new HookHandler(layer, hook, $"{Path.GetFileNameWithoutExtension(file)}.{name}.{layer}");
			hooks[layer].Add(handler);
			return handler;
		}

		public IHookHandler AddHook(MethodInfo target, Delegate hook)
		{
			var on = new Hook(target, hook);
			monoHooks.Add(on);
			return new OnHookHandler
			{
				Hook = on,
			};
		}

		public IHookHandler AddHook(MethodInfo target, ILContext.Manipulator hook)
		{
			var il = new ILHook(target, hook);
			monoHooks.Add(il);
			return new ILHookHandler
			{
				Hook = il,
			};
		}

		public void Disable(TerrariaFunction function)
		{
			disableFlags |= function;
		}

		public void Dispose()
		{
			foreach (var hook in monoHooks)
			{
				hook.Dispose();
			}
			ClearReflectionCache();
			GC.SuppressFinalize(this);
		}

		public void Enable(TerrariaFunction function)
		{
			disableFlags &= ~function;
		}

		public void RemoveHook(IHookHandler handler)
		{
			switch (handler)
			{
				case ILHookHandler il:
					il.Hook.Dispose();
					monoHooks.Remove(il.Hook);
					break;

				case OnHookHandler on:
					on.Hook.Dispose();
					monoHooks.Remove(on.Hook);
					break;

				case HookHandler hook:
					hooks[hook.Layer].Remove(hook);
					break;
			}
		}
	}
}