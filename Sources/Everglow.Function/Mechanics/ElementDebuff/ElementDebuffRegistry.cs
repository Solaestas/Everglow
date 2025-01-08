using Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

namespace Everglow.Commons.Mechanics.ElementDebuff;

public static class ElementDebuffRegistry
{
	private static readonly Dictionary<ElementDebuffType, Func<ElementDebuff>> _registry = [];

	static ElementDebuffRegistry()
	{
		Register(ElementDebuffType.NervousImpairment, () => new NervousImpairmentDebuff());
		Register(ElementDebuffType.Corrosion, () => new CorrosionDebuff());
		Register(ElementDebuffType.Burn, () => new BurnDebuff());
		Register(ElementDebuffType.Necrosis, () => new NecrosisDebuff());
	}

	public static void Register(ElementDebuffType type, Func<ElementDebuff> factory)
	{
		_registry[type] = factory;
	}

	public static ElementDebuff CreateDebuff(ElementDebuffType type)
	{
		if (_registry.TryGetValue(type, out var factory))
		{
			return factory();
		}
		throw new ArgumentException($"No debuff registered for type: {type}");
	}

	public static IEnumerable<ElementDebuffType> GetAllTypes()
	{
		return _registry.Keys;
	}
}