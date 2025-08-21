using Everglow.Commons.Mechanics.Cooldown;

namespace Everglow.Commons.Mechanics.ElementalDebuff;

public class ElementalDebuffRegistry : ModSystem
{
	private const int DefaultCooldownCapacity = 256;

	private static bool initialized = false;
	private static ElementalDebuffNet[] registry;
	private static Dictionary<string, ushort> nameToNetID;
	private static Dictionary<string, Type> nameToType;
	private static ushort nextNetID;

	public static IReadOnlyList<ElementalDebuffNet> Registry => registry;

	public static IReadOnlyDictionary<string, ushort> NameToNetID => nameToNetID;

	public static IReadOnlyDictionary<string, Type> NameToType => nameToType;

	public static bool Initialized => initialized;

	public override void Load()
	{
		registry = new ElementalDebuffNet[DefaultCooldownCapacity];
		nameToType = [];
		nameToNetID = [];
		nextNetID = 0;
	}

	public override void Unload()
	{
		initialized = false;

		registry = null;

		nameToType.Clear();
		nameToType = null;

		nameToNetID.Clear();
		nameToNetID = null;

		nextNetID = 0;
	}

	public override void PostSetupContent()
	{
		RegisterAllElementalDebuffs();
		initialized = true;
	}

	private static void RegisterAllElementalDebuffs()
	{
		Type baseType = typeof(ElementalDebuffHandler);
		foreach (var (modName, modTypes) in ModLoader.Mods
			.Select(m => (m.Name, m.Code.GetTypes().AsEnumerable()))
			.Concat([(nameof(Everglow), Ins.ModuleManager.Types)]))
		{
			foreach (var modType in modTypes.Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract))
			{
				string baseID = (string)modType.GetProperty(nameof(CooldownBase.ID))?.GetValue(null) ?? modName + "_" + modType.Name;
				nameToType.TryAdd(baseID, modType);
				Register(baseID);
			}
		}
	}

	public static ElementalDebuffNet Register(string id)
	{
		int currentMaxID = registry.Length;

		// This case only happens when you cap out at 65,536 cooldown registrations (which should never occur).
		// It just stops you from registering more cooldowns.
		if (nextNetID == currentMaxID)
		{
			return default;
		}

		ElementalDebuffNet cdNet = new(id, nextNetID);
		nameToNetID[cdNet.ID] = cdNet.NetID;
		registry[cdNet.NetID] = cdNet;
		++nextNetID;

		if (nextNetID == currentMaxID && currentMaxID < ushort.MaxValue)
		{
			ElementalDebuffNet[] largerArray = new ElementalDebuffNet[currentMaxID * 2];
			for (int i = 0; i < currentMaxID; ++i)
			{
				largerArray[i] = registry[i];
			}

			registry = largerArray;
		}

		return cdNet;
	}

	public static IEnumerable<string> GetTypes() => nameToType.Keys;

	/// <summary>
	/// Return id record of specified id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns>Returns <c>null</c> if no id record registered.</returns>
	public static ElementalDebuffNet GetNet(string id) =>
		nameToNetID.TryGetValue(id, out ushort netID) ? registry[netID] : null;

	public static ElementalDebuffInstance GetInstance(string id, NPC npc) =>
		nameToType.TryGetValue(id, out var type)
			? new ElementalDebuffInstance(npc, GetNet(id))
			: throw new ArgumentException($"No elemental debuff registered for type: {type}");
}