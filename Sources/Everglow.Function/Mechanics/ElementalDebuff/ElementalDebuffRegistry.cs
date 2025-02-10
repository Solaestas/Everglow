using Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

namespace Everglow.Commons.Mechanics.ElementalDebuff;

public static class ElementalDebuffRegistry
{
	private static readonly Dictionary<ElementalDebuffType, Func<ElementalDebuff>> _registry = [];

	static ElementalDebuffRegistry()
	{
		Register(ElementalDebuffType.NervousImpairment, () => new NervousImpairmentDebuff());
		Register(ElementalDebuffType.Corrosion, () => new CorrosionDebuff());
		Register(ElementalDebuffType.Burn, () => new BurnDebuff());
		Register(ElementalDebuffType.Necrosis, () => new NecrosisDebuff());
	}

	public static void Register(ElementalDebuffType type, Func<ElementalDebuff> factory)
	{
		_registry[type] = factory;
	}

	public static ElementalDebuff CreateDebuff(ElementalDebuffType type)
	{
		if (_registry.TryGetValue(type, out var factory))
		{
			return factory();
		}
		throw new ArgumentException($"No elemental debuff registered for type: {type}");
	}

	public static IEnumerable<ElementalDebuffType> GetAllTypes()
	{
		return _registry.Keys;
	}
}