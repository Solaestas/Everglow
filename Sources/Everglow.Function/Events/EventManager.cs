using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Everglow.Commons.Events
{
	public class EventManager : ModSystem
	{
		const string LayerName = "Everglow/ModEvent";
		static List<ModEvent> actives = new();
		static LegacyGameInterfaceLayer Layer_HasInvasion = new(LayerName, delegate
		{
			foreach (ModEvent e in actives)
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
		static LegacyGameInterfaceLayer Layer_NoInvasion = new(LayerName, delegate
		{
			foreach (ModEvent e in actives)
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
		private static void ReSortActives()
		{
			actives.Sort((e1, e2) => -e1.SortRank.CompareTo(e2.SortRank));
		}
		internal static void Register(ModEvent e)
		{
			ModTypeLookup<ModEvent>.Register(e);
		}
		public static bool Activate(ModEvent e, params object[] args)
		{
			if (e.CanActivate(args))
			{
				actives.Add(e);
				ReSortActives();
				e.Active = true;
				e.OnActivate(args);
				return true;
			}
			return false;
		}
		public static bool Activate<T>(params object[] args) where T : ModEvent
		{
			ModEvent e;
			return (e = ModContent.GetInstance<T>()) is not null && Activate(e, args);
		}
		public static bool Deactivate(ModEvent e, params object[] args)
		{
			if (e.CanDeactivate(args))
			{
				if (actives.Remove(e))
				{
					e.Active = false;
					e.OnDeactivate(args);
					return true;
				}
				return false;
			}
			return false;
		}
		public static bool Deactivate<T>(params object[] args) where T : ModEvent
		{
			ModEvent e;
			return (e = ModContent.GetInstance<T>()) is not null && Deactivate(e, args);
		}

		public override void PostUpdateEverything()
		{
			foreach (ModEvent e in actives)
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
				layers.Insert(index, Layer_HasInvasion);
				return;
			}
			index = layers.FindIndex(layer => layer.Name == "Vanilla: Map / Minimap");
			if (index != -1)
			{
				layers.Insert(index, Layer_NoInvasion);
			}
		}
		public override void SaveWorldData(TagCompound tag)
		{
			foreach (ModEvent e in ModContent.GetContent<ModEvent>())
			{
				TagCompound subtag = new();
				e.SaveData(subtag);
				tag[e.FullName] = subtag;
			}
			int Count = actives.Count;
			tag[nameof(Count)] = Count;
			for (int i = 0; i < Count; i++)
			{
				ModEvent e = actives[i];
				tag[$"{nameof(actives)}_{i}.FullName"] = e.FullName;
				tag[$"{nameof(actives)}_{i}.DefName"] = e.DefName;
				TagCompound subtag = new();
				if (ModContent.TryFind(e.FullName, out ModEvent oe) && oe != e)
				{
					e.SaveData(subtag);
				}
				tag[$"{nameof(actives)}_{i}.Tag"] = subtag;
			}
		}
		public override void LoadWorldData(TagCompound tag)
		{
			foreach (ModEvent e in ModContent.GetContent<ModEvent>())
			{
				if (tag.TryGet(e.FullName, out TagCompound subtag))
				{
					e.LoadData(e.FullName, subtag);
				}
			}
			int Count;
			if (tag.TryGet(nameof(Count), out Count) && Count > 0)
			{
				for (int i = 0; i < Count; i++)
				{
					if (tag.TryGet($"{nameof(actives)}_{i}.FullName", out string fullName) && tag.TryGet($"{nameof(actives)}_{i}.DefName", out string defName))
					{
						if (ModContent.TryFind(fullName, out ModEvent e))
						{
							if (e.FullName != defName)
							{
								e = e.Clone();
								tag.TryGet($"{nameof(actives)}_{i}.Tag", out TagCompound subtag);
								subtag ??= new();
								e.LoadData(defName, subtag);
							}
							e.Active = true;
							actives.Add(e);
						}
					}
				}
				ReSortActives();
			}
		}
	}
}
