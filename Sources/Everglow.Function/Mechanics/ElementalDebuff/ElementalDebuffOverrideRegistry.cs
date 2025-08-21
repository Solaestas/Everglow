namespace Everglow.Commons.Mechanics.ElementalDebuff;

public static class ElementalDebuffOverrideRegistry
{
	private static readonly Dictionary<(int, string), ElementalDebuffOverride> _registry = [];

	public static void Register(int npcType, string debuffType, ElementalDebuffOverride info) =>
		_registry[(npcType, debuffType)] = info;

	public static ElementalDebuffOverride GetOverride(NPC npc, string debuffType) =>
		GetOverride(npc.type, debuffType);

	public static ElementalDebuffOverride GetOverride(int npcType, string debuffType) =>
		_registry.TryGetValue((npcType, debuffType), out ElementalDebuffOverride info) ? info : null;
}