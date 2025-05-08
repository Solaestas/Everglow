using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Everglow.Commons.Events
{
	public class EventManager : ModSystem
	{
		private const string LayerName = "Everglow/ModEvent";
		private const string VanillaInvasionLayerName = "Vanilla: Invasion Progress Bars";
		private const string VanillaMinimapLayerName = "Vanilla: Map / Minimap";

		private static List<ModEvent> Actives = [];

		private static LegacyGameInterfaceLayer Layer_HasInvasion = new(LayerName, delegate
		{
			foreach (ModEvent e in Actives)
			{
				if (e.IsBackground)
				{
					continue;
				}
				e.Draw(Main.spriteBatch);
				return true;
			}

			var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin();

			Main.DrawInvasionProgress();
			if (Main.HealthBarDrawSettings != 0)
			{
				Main.BigBossProgressBar.Draw(Main.spriteBatch);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);

			return true;
		});

		private static LegacyGameInterfaceLayer Layer_NoInvasion = new(LayerName, delegate
		{
			foreach (ModEvent e in Actives)
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
			Actives.Sort((e1, e2) => -e1.SortRank.CompareTo(e2.SortRank));
		}

		internal static void Register(ModEvent e)
		{
			ModTypeLookup<ModEvent>.Register(e);
		}

		public static bool Activate(ModEvent e, params object[] args)
		{
			if (e.CanActivate(args))
			{
				Actives.Add(e);
				ReSortActives();
				e.Active = true;
				e.OnActivate(args);
				return true;
			}
			return false;
		}

		public static bool Activate<T>(params object[] args)
			where T : ModEvent
		{
			ModEvent e;
			return (e = ModContent.GetInstance<T>()) is not null && Activate(e, args);
		}

		public static bool Deactivate(ModEvent e, params object[] args)
		{
			if (e.CanDeactivate(args))
			{
				if (Actives.Remove(e))
				{
					e.Active = false;
					e.OnDeactivate(args);
					return true;
				}
				return false;
			}
			return false;
		}

		public static bool Deactivate<T>(params object[] args)
			where T : ModEvent
		{
			ModEvent e;
			return (e = ModContent.GetInstance<T>()) is not null && Deactivate(e, args);
		}

		public override void PostUpdateEverything()
		{
			foreach (ModEvent e in Actives)
			{
				e.Update();
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int index = layers.FindIndex(layer => layer.Name == VanillaInvasionLayerName);
			if (index != -1)
			{
				layers.RemoveAt(index);
				layers.Insert(index, Layer_HasInvasion);
				return;
			}
			index = layers.FindIndex(layer => layer.Name == VanillaMinimapLayerName);
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
			int Count = Actives.Count;
			tag[nameof(Count)] = Count;
			for (int i = 0; i < Count; i++)
			{
				ModEvent e = Actives[i];
				tag[$"{nameof(Actives)}_{i}.FullName"] = e.FullName;
				tag[$"{nameof(Actives)}_{i}.DefName"] = e.DefName;
				TagCompound subtag = new();
				if (ModContent.TryFind(e.FullName, out ModEvent oe) && oe != e)
				{
					e.SaveData(subtag);
				}
				tag[$"{nameof(Actives)}_{i}.Tag"] = subtag;
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
					if (tag.TryGet($"{nameof(Actives)}_{i}.FullName", out string fullName) && tag.TryGet($"{nameof(Actives)}_{i}.DefName", out string defName))
					{
						if (ModContent.TryFind(fullName, out ModEvent e))
						{
							if (e.FullName != defName)
							{
								e = e.Clone();
								tag.TryGet($"{nameof(Actives)}_{i}.Tag", out TagCompound subtag);
								subtag ??= new();
								e.LoadData(defName, subtag);
							}
							e.Active = true;
							Actives.Add(e);
						}
					}
				}
				ReSortActives();
			}
		}
	}
}