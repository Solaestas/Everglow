using System.Reflection;
using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
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

public record HookHandler(CodeLayer Layer, dynamic Hook, string Name) : IHookHandler
{
	public bool Enable { get; set; } = true;
}

public class HookManager : ModSystem, IHookManager
{
	private TrueHookManager _manager;

	public static void Initialize()
	{
		Ins.Set<IHookManager>(new TrueHookManager());
	}

	public HookManager()
	{
		_manager = Ins.HookManager as TrueHookManager;
		Debug.Assert(_manager is not null);
		Ins.Set<IHookManager>(this);
	}

	public static void ClearReflectionCache()
	{
		ReflectionHelper.AssemblyCache.Clear();
		ReflectionHelper.AssembliesCache.Clear();
		ReflectionHelper.ResolveReflectionCache.Clear();
	}

	public IHookHandler AddHook(CodeLayer layer, Delegate hook, string name = null)
	{
		return _manager.AddHook(layer, hook, name);
	}

	public IHookHandler AddHook(MethodInfo target, Delegate hook)
	{
		return _manager.AddHook(target, hook);
	}

	public IHookHandler AddHook(MethodInfo target, ILContext.Manipulator hook)
	{
		return _manager.AddHook(target, hook);
	}

	public void Disable(TerrariaFunction function)
	{
		_manager.Disable(function);
	}

	public void Dispose()
	{
		_manager.Dispose();
		GC.SuppressFinalize(this);
	}

	public void Enable(TerrariaFunction function)
	{
		_manager.Enable(function);
	}

	public void Invoke(CodeLayer layer)
	{
		foreach (var handler in _manager.hooks[layer].Where(h => h.Enable))
		{
			try
			{
				handler.Hook.Invoke();
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
		IL_Main.DoDraw += Main_DoDraw;
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
		Main.OnResolutionChanged += Main_OnResolutionChanged;
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
		IL_Main.DoDraw -= Main_DoDraw;
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
		Dispose();
	}

	private void LegacyPlayerRenderer_DrawPlayers(On_LegacyPlayerRenderer.orig_DrawPlayers orig, LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, IEnumerable<Player> players)
	{
		orig.Invoke(self, camera, players);
		Invoke(CodeLayer.PostDrawPlayers);
	}

	// TODO 不要给这种大函数直接IL了
	private void Main_DoDraw(ILContext il)
	{
		var cursor = new ILCursor(il);
		if (!cursor.TryGotoNext(MoveType.Before, ins => ins.MatchLdcI4(36)))
		{
			throw new Exception("Main_DoDraw_NotFound_1");
		}

		cursor.EmitDelegate(() => Invoke(CodeLayer.PostDrawFilter));
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
				handler.Hook.Invoke(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
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
				handler.Hook.Invoke(obj);
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

	private class TrueHookManager : IHookManager
	{
		public TerrariaFunction disableFlags;

		public Dictionary<CodeLayer, List<HookHandler>> hooks = validLayers.ToDictionary(l => l, l => new List<HookHandler>());

		private static readonly CodeLayer[] validLayers = new CodeLayer[]
		{
			CodeLayer.PostDrawFilter,
			CodeLayer.PostDrawTiles,
			CodeLayer.PostDrawProjectiles,
			CodeLayer.PostDrawDusts,
			CodeLayer.PostDrawNPCs,
			CodeLayer.PostDrawPlayers,
			CodeLayer.PostDrawMapIcons,
			CodeLayer.PostDrawBG,

			CodeLayer.PostUpdateEverything,
			CodeLayer.PostUpdateProjectiles,
			CodeLayer.PostUpdatePlayers,
			CodeLayer.PostUpdateNPCs,
			CodeLayer.PostUpdateDusts,
			CodeLayer.PostUpdateInvasions,

			CodeLayer.PostEnterWorld_Single,
			CodeLayer.PostExitWorld_Single,
			CodeLayer.PostEnterWorld_Server,
			CodeLayer.ResolutionChanged,
		};

		private List<IDisposable> monoHooks = new();

		public IHookHandler AddHook(CodeLayer layer, Delegate hook, string name = default)
		{
			var handler = new HookHandler(layer, hook, name ?? string.Empty);
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