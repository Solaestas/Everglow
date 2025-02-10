namespace Everglow.Commons.Mechanics.ElementalDebuff;

public static class ElementalDebuffInfoRegistry
{
	private static readonly Dictionary<(int, ElementalDebuffType), ElementalDebuffInfo> _registry = [];

	static ElementalDebuffInfoRegistry()
	{
		
	}

	public static void Register(int npcType, ElementalDebuffType debuffType, ElementalDebuffInfo info) =>
		_registry[(npcType, debuffType)] = info;

	public static ElementalDebuffInfo GetInfo(int npcType, ElementalDebuffType debuffType) =>
		_registry.TryGetValue((npcType, debuffType), out ElementalDebuffInfo info) ? info : new ElementalDebuffInfo();
}