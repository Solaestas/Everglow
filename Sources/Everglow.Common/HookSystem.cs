using Everglow.Common.Enums;
using MonoMod.Cil;
using MonoMod.Utils;
using Terraria.Graphics.Renderers;

namespace Everglow.Common
{
	/// <summary>
	/// 对一个方法的管理，可以用来控制钩子是否启用
	/// </summary>
	[DebuggerDisplay("{name} : {enable ? \"Enable\" : \"Disable\"}")]
	public class ActionHandler
	{
		public Action Action
		{
			get => action;
			set
			{
				enable = value is not null;
				action = value;
			}
		}

		private Action action;
		private string name;
		private bool enable;

		public bool Enable
		{
			get => enable;
			set => enable = value;
		}

		public string Name => name;

		public ActionHandler(Action action)
		{
			this.action = action;
			name = action.ToString();
			enable = true;
		}

		public ActionHandler(Action action, string name)
		{
			this.action = action;
			this.name = name;
			enable = true;
		}

		public ActionHandler()
		{
			action = null;
			name = string.Empty;
			enable = false;
		}

		public void Invoke() => action.Invoke();

		public override string ToString() => name;
	}

	/// <summary>
	/// 钩子统一管理，不用继承一大堆ModSystem或者加很多的On，一些常见的加载时刻都写了
	/// </summary>
	public class HookSystem : ModSystem
	{
		public HookSystem()
		{
			waitToRemove = new List<(CodeLayer op, ActionHandler handler)>();
			methods = new Dictionary<CodeLayer, List<ActionHandler>>();
			foreach (var op in validOpportunity)
			{
				methods.Add(op, new List<ActionHandler>());
			}
		}

		private List<(CodeLayer op, ActionHandler handler)> waitToRemove;

		public static readonly CodeLayer[] validOpportunity = new CodeLayer[]
		{
            //Draw
            CodeLayer.PostDrawFilter,
			CodeLayer.PostDrawTiles,
			CodeLayer.PostDrawProjectiles,
			CodeLayer.PostDrawDusts,
			CodeLayer.PostDrawNPCs,
			CodeLayer.PostDrawPlayers,
			CodeLayer.PostDrawMapIcons,
			CodeLayer.PostDrawBG,
            //Update
            CodeLayer.PostUpdateEverything,
			CodeLayer.PostUpdateProjectiles,
			CodeLayer.PostUpdatePlayers,
			CodeLayer.PostUpdateNPCs,
			CodeLayer.PostUpdateDusts,
			CodeLayer.PostUpdateInvasions,
            //Misc
            CodeLayer.PostEnterWorld_Single,
			CodeLayer.PostExitWorld_Single,
			CodeLayer.PostEnterWorld_Server,
			CodeLayer.ResolutionChanged
		};

		internal bool DisableDrawNPCs { get; set; } = false;
		internal bool DisableDrawSkyAndHell { get; set; } = false;
		internal bool DisableDrawBackground { get; set; } = false;
		internal Dictionary<CodeLayer, List<ActionHandler>> methods;

		/// <summary>
		/// 现在存在的问题就是，这里的method都是无参数的Action，但是如DrawMapIcon这样的方法就需要传参了，只好用这种这种定义字段的方法
		/// </summary>
		public (Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale) MapIconInfomation
		{
			get; internal set;
		}

		/// <summary>
		/// 更新的计时器，PostUpateEverything后加一
		/// </summary>
		public static int UpdateTimer
		{
			get; private set;
		}

		/// <summary>
		/// 绘制的计时器，PostDrawEverything后加一
		/// </summary>
		public static int DrawTimer
		{
			get; private set;
		}

		/// <summary>
		/// 针对UI的计时器，暂停时也会加一
		/// </summary>
		public static int UITimer
		{
			get; private set;
		}

		/// <summary>
		/// 在 <paramref name="op"/> 时执行 <paramref name="action"/>
		/// </summary>
		/// <param name="action"></param>
		/// <param name="op"></param>
		/// <param name="name">Handler的名字，方便查找</param>
		/// <exception cref="ArgumentException"></exception>
		public ActionHandler AddMethod(Action action, CodeLayer op, string name = null)
		{
			if (!validOpportunity.Contains(op))
			{
				//除非搞事不然应该不会执行这行代码
				throw new ArgumentException("Invaild Opportunity");
			}
			var handler = new ActionHandler(action, name ?? action.ToString());
			methods[op].Add(handler);
			return handler;
		}

		public void AddMethod(ActionHandler handler, CodeLayer op)
		{
			if (!validOpportunity.Contains(op))
			{
				//除非搞事不然应该不会执行这行代码
				throw new ArgumentException("Invaild Opportunity");
			}
			methods[op].Add(handler);
		}

		/// <summary>
		/// 根据Name寻找方法
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ActionHandler Find(string name, CodeLayer op = CodeLayer.None)
		{
			if (op == CodeLayer.None)
			{
				foreach (var vs in methods.Values)
				{
					foreach (var handler in vs)
					{
						if (handler.Name == name)
						{
							return handler;
						}
					}
				}
			}
			else
			{
				foreach (var handler in methods[op])
				{
					if (handler.Name == name)
					{
						return handler;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 根据Handler移除一个方法
		/// </summary>
		/// <param name="handler"></param>
		/// <returns>是否成功移除</returns>
		public bool Remove(ActionHandler handler)
		{
			foreach (var op in validOpportunity)
			{
				var handlers = methods[op];
				for (int i = 0; i < handlers.Count; i++)
				{
					if (handler == handlers[i])
					{
						waitToRemove.Add((op, handler));
						handler.Enable = false;
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// 移除所有被Disable的Action
		/// </summary>
		public void RemoveDisabledAction()
		{
			foreach (var op in validOpportunity)
			{
				methods[op].RemoveAll(handler => !handler.Enable);
			}
		}

		public void HookLoad()
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

		public void HookUnload()
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
			ClearReflectionCache();
		}

		public static void ClearReflectionCache()
		{
			ReflectionHelper.AssemblyCache.Clear();
			ReflectionHelper.AssembliesCache.Clear();
			ReflectionHelper.ResolveReflectionCache.Clear();
		}

		internal void Invoke(CodeLayer op)
		{
			foreach (var handler in methods[op])
			{
				if (handler.Enable)
				{
					try
					{
						handler.Invoke();
					}
					catch (Exception ex)
					{
						Debug.Fail($"{handler} 抛出了异常 {ex}");
						Mod.Logger.Error($"{handler} 抛出了异常 {ex}");
						handler.Enable = false;
					}
				}
			}
		}

		public override void PostUpdateInvasions()
		{
			Invoke(CodeLayer.PostUpdateInvasions);
		}

		public override void PostUpdateEverything()
		{
			Invoke(CodeLayer.PostUpdateEverything);
			UpdateTimer++;
			foreach (var (op, handler) in waitToRemove)
			{
				methods[op].Remove(handler);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			UITimer++;
		}

		public override void PostUpdateNPCs()
		{
			Invoke(CodeLayer.PostUpdateNPCs);
		}

		public override void PostUpdateProjectiles()
		{
			Invoke(CodeLayer.PostUpdateProjectiles);
		}

		public override void PostUpdateDusts()
		{
			Invoke(CodeLayer.PostUpdateDusts);
		}

		public override void PostUpdatePlayers()
		{
			Invoke(CodeLayer.PostUpdatePlayers);
		}

		public override void PostDrawTiles()
		{
			Invoke(CodeLayer.PostDrawTiles);
		}

		private void Main_DrawBackground(On_Main.orig_DrawBackground orig, Main self)
		{
			if (DisableDrawBackground)
			{
				return;
			}
			orig(self);
		}

		private void Main_DrawBG(On_Main.orig_DrawBG orig, Main self)
		{
			if (DisableDrawSkyAndHell)
			{
				return;
			}
			orig(self);
		}

		private void Main_DoDraw_WallsTilesNPCs(On_Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
		{
			Invoke(CodeLayer.PostDrawBG);
			orig(self);
		}

		private void WorldGen_serverLoadWorldCallBack(On_WorldGen.orig_serverLoadWorldCallBack orig)
		{
			orig();
			Invoke(CodeLayer.PostEnterWorld_Server);
		}

		private void Main_DrawMiscMapIcons(On_Main.orig_DrawMiscMapIcons orig, Main self, SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string mouseTextString)
		{
			orig(self, spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, ref mouseTextString);
			MapIconInfomation = (mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
			Invoke(CodeLayer.PostDrawMapIcons);
		}

		private void WorldGen_SaveAndQuit(On_WorldGen.orig_SaveAndQuit orig, Action callback)
		{
			orig(callback);
			Invoke(CodeLayer.PostExitWorld_Single);
		}

		private void WorldGen_playWorldCallBack(On_WorldGen.orig_playWorldCallBack orig, object threadContext)
		{
			orig(threadContext);
			Invoke(CodeLayer.PostEnterWorld_Single);
		}

		private void Main_OnResolutionChanged(Vector2 obj)
		{
			Invoke(CodeLayer.ResolutionChanged);
		}

		private void LegacyPlayerRenderer_DrawPlayers(On_LegacyPlayerRenderer.orig_DrawPlayers orig, LegacyPlayerRenderer self, Terraria.Graphics.Camera camera, IEnumerable<Player> players)
		{
			orig.Invoke(self, camera, players);
			Invoke(CodeLayer.PostDrawPlayers);
		}

		private void Main_DrawNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			orig.Invoke(self, behindTiles);
			if (!behindTiles)
			{
				Invoke(CodeLayer.PostDrawNPCs);
			}
		}

		private void Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
		{
			orig.Invoke(self);
			Invoke(CodeLayer.PostDrawDusts);
		}

		private void Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			orig.Invoke(self);
			Invoke(CodeLayer.PostDrawProjectiles);
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
	}
}