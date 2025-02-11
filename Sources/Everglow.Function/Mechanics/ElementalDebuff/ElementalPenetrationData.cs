namespace Everglow.Commons.Mechanics.ElementalDebuff;

public readonly struct ElementalPenetrationData
{
	private readonly StatModifier[] elementalPenetration;

	public ElementalPenetrationData()
	{
		var types = ElementalDebuffRegistry.GetAllTypes().ToList();
		types.Insert(0, ElementalDebuffType.Generic);

		elementalPenetration = new StatModifier[types.Count];
		foreach (var type in types)
		{
			elementalPenetration[(int)type] = StatModifier.Default;
		}
	}

	public ref StatModifier this[int type] => ref elementalPenetration[type];

	public ref StatModifier this[ElementalDebuffType type] => ref elementalPenetration[(int)type];

	public void ResetEffects() => Array.ForEach(elementalPenetration, stat => stat = StatModifier.Default);
}