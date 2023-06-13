using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Everglow.Commons.Events
{
	public class EventManager : ModSystem
	{
		const string LayerName = "Everglow:DrawModEvents";
		static LegacyGameInterfaceLayer DrawLayer_HasInvasion= new LegacyGameInterfaceLayer(LayerName, delegate
		{
			foreach (ModEvent e in activeEvents)
			{
				if (e.IsBackground)
				{
					continue;
				}
				e.Draw(Main.spriteBatch);
				return true;
			}
			Main.DrawInvasionProgress();
			return true;
		});
		static LegacyGameInterfaceLayer DrawLayer_HasNotInvasion= new LegacyGameInterfaceLayer(LayerName, delegate
		{
			foreach (ModEvent e in activeEvents)
			{
				if (e.IsBackground)
				{
					continue;
				}
				e.Draw(Main.spriteBatch);
				return true;
			}
			return true;
		});
		internal static List<ModEvent> activeEvents = new();
		public override void Unload()
		{
			activeEvents.Clear();
		}
		public override void PostUpdateEverything()
		{
			foreach(ModEvent e in activeEvents)
			{
				e.Update();
			}
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int index = layers.FindIndex(layer => layer.Name == "Vanilla: Invasion Progress Bars");
			if (index != -1)
			{
				layers.RemoveAt(index);
				layers.Insert(index, DrawLayer_HasInvasion);
				return;
			}
			index = layers.FindIndex(layer => layer.Name == "Vanilla: Map / Minimap");
			if (index != -1)
			{
				layers.Insert(index, DrawLayer_HasNotInvasion);
			}
		}
		private static int SortEvents(ModEvent e1, ModEvent e2)
		{
			return -e1.SortRank.CompareTo(e2.SortRank);
		}
		internal static void Reigster(ModEvent @event)
		{
			ModTypeLookup<ModEvent>.Register(@event);
		}
		/// <summary>
		/// 尝试启动指定的事件类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns>null:未找到此类型事件<br>false:此类型事件已处于启动状态</br><br>true:成功启动此类型事件</br></returns>
		public static bool? Start<T>(params object[] args) where T : ModEvent
		{
			T e = ModContent.GetInstance<T>();
			if (e is null)
			{
				return null;
			}
			if (e.Active)
			{
				return false;
			}
			e.Active = true;
			activeEvents.Add(e);
			activeEvents.Sort(SortEvents);
			e.OnStart(args);
			return true;
		}
		/// <summary>
		/// 通常情况<see cref="ModEvent"/>应为单实例
		/// <br>但是此方法允许你启动复数个类似的<see cref="ModEvent"/></br>
		/// <br>注意:只有通过<see cref="Start{T}(object[])"/>能够启动的事件才会被自动保存</br>
		/// </summary>
		/// <param name="event"></param>
		/// <param name="args"></param>
		public static void Start(ModEvent @event, params object[] args)
		{
			@event.Active = true;
			activeEvents.Add(@event);
			activeEvents.Sort(SortEvents);
			@event.OnStart(args);
		}
		/// <summary>
		/// 尝试停止指定的事件类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <returns>null:未找到此类型事件<br>false:此类型事件未处于启动状态</br><br>true:成功停止此类型事件</br></returns>
		public static bool? Stop<T>(params object[] args) where T : ModEvent
		{
			T e = ModContent.GetInstance<T>();
			if (e is null)
			{
				return null;
			}
			if (!e.Active)
			{
				return false;
			}
			e.Active = false;
			e.OnStop(args);
			activeEvents.Remove(e);
			return true;
		}
		/// <summary>
		/// 停止特定的事件
		/// </summary>
		/// <param name="event"></param>
		/// <param name="args"></param>
		/// <returns>仅当目标受管理且处于启动状态时成功关闭返回true</returns>
		public static bool Stop(ModEvent @event, params object[] args)
		{
			if (activeEvents.Remove(@event))
			{
				if(!@event.Active)
				{
					return false;
				}
				@event.Active = false;
				@event.OnStop(args);
				return true;
			}
			return false;
		}
		/// <summary>
		/// 停止满足某些条件的所有事件
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="args"></param>
		public static void Stop(Predicate<ModEvent> predicate, params object[] args)
		{
			List<ModEvent> removed = new();
			activeEvents.RemoveAll(e =>
			{
				if(predicate(e))
				{
					removed.Add(e);
					return true;
				}
				return false;
			});
			foreach(ModEvent e in removed)
			{
				e.Active = false;
				e.OnStop(predicate, args);
			}
		}
		public override void SaveWorldData(TagCompound tag)
		{
			List<string> aes = new();
			foreach(ModEvent e in ModContent.GetContent<ModEvent>())
			{
				TagCompound subtag = new();
				e.SaveData(subtag);
				tag[e.FullName] = subtag;
				if(e.Active)
				{
					aes.Add(e.FullName);
				}
			}
			tag["ActiveInfos"] = aes;
		}
		public override void LoadWorldData(TagCompound tag)
		{
			tag.TryGet("ActiveInfos",out List<string> aes);
			aes ??= new();
			foreach(ModEvent e in ModContent.GetContent<ModEvent>())
			{
				e.Active = aes.Contains(e.FullName);
				if(tag.TryGet(e.FullName,out TagCompound subtag))
				{
					e.LoadData(subtag);
				}
			}
		}
	}
}
