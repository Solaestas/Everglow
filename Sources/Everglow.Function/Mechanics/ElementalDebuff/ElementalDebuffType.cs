namespace Everglow.Commons.Mechanics.ElementalDebuff;

/// <summary>
/// Element debuff types. (For more details, please go to <a href="https://prts.wiki/w/%E5%85%83%E7%B4%A0">Arknights Wiki</a>)
/// <list type="number">
///     <item>Nervous Impairment</item>
///     <item>Corrosion</item>
///     <item>Burn</item>
///     <item>Necrosis</item>
/// </list>
/// </summary>
public enum ElementalDebuffType
{
	/// <summary>
	/// Used to apply bonus for all elemental debuff type. Not registered in <see cref="ElementalDebuffRegistry"/> and other registeries.
	/// </summary>
	Generic,

	NervousImpairment,
	Corrosion,
	Burn,
	Necrosis,

	BloodRot,
	Frost,
	Salination,
}