namespace Everglow.Commons.Mechanics.ElementalDebuff;

public readonly struct ElementalPenetration
{
	/// <summary>
	/// 0 ~ last - 1 : types
	/// <br/> last : generic
	/// </summary>
	private readonly StatModifier[] elementalPenetration;

	public ElementalPenetration()
	{
		if (ElementalDebuffRegistry.Initialized)
		{
			var types = ElementalDebuffRegistry.GetTypes();
			elementalPenetration = new StatModifier[types.Count() + 1];
			foreach (var type in types)
			{
				elementalPenetration[ElementalDebuffRegistry.NameToNetID[type]] = StatModifier.Default;
			}
			elementalPenetration[types.Count()] = StatModifier.Default;
		}
	}

	public ref StatModifier Generic => ref elementalPenetration[^1];

	public ref StatModifier this[string type] => ref type != ElementalDebuffHandler.Generic
		? ref elementalPenetration[ElementalDebuffRegistry.GetNet(type).NetID]
		: ref Generic;

	public ref StatModifier this[int type] => ref elementalPenetration[type];

	public void ResetEffects()
	{
		for (int i = 0; i < elementalPenetration.Length; i++)
		{
			elementalPenetration[i] = StatModifier.Default;
		}
	}
}