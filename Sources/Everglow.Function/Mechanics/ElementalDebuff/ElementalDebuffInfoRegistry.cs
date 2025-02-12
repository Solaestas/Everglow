namespace Everglow.Commons.Mechanics.ElementalDebuff;

public static class ElementalDebuffInfoRegistry
{
	private static readonly Dictionary<(int, ElementalDebuffType), ElementalDebuffInfo> _registry = [];

	public static void Register(int npcType, ElementalDebuffType debuffType, ElementalDebuffInfo info) =>
		_registry[(npcType, debuffType)] = debuffType != ElementalDebuffType.Generic
			? info
			: throw new InvalidOperationException(GenericError);

	public static ElementalDebuffInfo GetInfo(NPC npc, ElementalDebuffType debuffType) =>
		GetInfo(npc.type, debuffType);

	public static ElementalDebuffInfo GetInfo(int npcType, ElementalDebuffType debuffType) =>
		debuffType != ElementalDebuffType.Generic
			? _registry.TryGetValue((npcType, debuffType), out ElementalDebuffInfo info) ? info : new()
			: throw new InvalidOperationException(GenericError);

	private static string GenericError => "Generic is invalid.";
}