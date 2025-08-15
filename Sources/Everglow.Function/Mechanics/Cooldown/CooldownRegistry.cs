namespace Everglow.Commons.Mechanics.Cooldown;

public class CooldownRegistry : ModSystem
{
	public const int DefaultCooldownCapacity = 256;
	public static CooldownNet[] registry;
	public static Dictionary<string, ushort> nameToNetID;
	public static Dictionary<string, Type> nameToType;
	private static ushort nextNetID = 0;

	public override void Load()
	{
		registry = new CooldownNet[DefaultCooldownCapacity];
		nameToNetID = [];
		nameToType = [];
	}

	public override void Unload()
	{
		registry = null;

		nameToNetID?.Clear();
		nameToNetID = null;

		nameToType?.Clear();
		nameToType = null;
	}

	public override void PostSetupContent()
	{
		RegisterCooldowns();
	}

	public static void RegisterCooldowns()
	{
		Type baseType = typeof(CooldownBase);
		var dict = ModLoader.Mods.Select(m => (m.Name, m.Code.GetTypes().AsEnumerable()))
			.Concat([(nameof(Everglow), Ins.ModuleManager.Types)]);
		foreach (var (modName, modTypes) in dict)
		{
			foreach (var modType in modTypes.Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract))
			{
				string baseID = (string)modType.GetProperty(nameof(CooldownBase.ID))?.GetValue(null) ?? modName + "_" + modType.Name;
				nameToType.TryAdd(baseID, modType);
				Register(baseID);
			}
		}
	}

	public static CooldownNet GetNet(string id) =>
		nameToNetID.TryGetValue(id, out ushort netID) ? registry[netID] : null;

	/// <summary>
	/// Registers a CooldownHandler for use in netcode, assigning it a Cooldown and thus a netID. Cooldowns are useless until this has been done.
	/// </summary>
	/// <returns>The registered Cooldown.</returns>
	public static CooldownNet Register(string id)
	{
		int currentMaxID = registry.Length;

		// This case only happens when you cap out at 65,536 cooldown registrations (which should never occur).
		// It just stops you from registering more cooldowns.
		if (nextNetID == currentMaxID)
		{
			return default;
		}

		CooldownNet cdNet = new(id, nextNetID);
		nameToNetID[cdNet.ID] = cdNet.NetID;
		registry[cdNet.NetID] = cdNet;
		++nextNetID;

		if (nextNetID == currentMaxID && currentMaxID < ushort.MaxValue)
		{
			CooldownNet[] largerArray = new CooldownNet[currentMaxID * 2];
			for (int i = 0; i < currentMaxID; ++i)
			{
				largerArray[i] = registry[i];
			}

			registry = largerArray;
		}

		return cdNet;
	}
}