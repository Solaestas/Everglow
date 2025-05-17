namespace Everglow.Commons.Mechanics.ElementalDebuff;

/// <summary>
/// Use this type to store the base infomations of <see cref="ElementalDebuff"/>
/// </summary>
/// <param name="BuildUpMax"></param>
/// <param name="DurationMax"></param>
/// <param name="DotDamage"></param>
/// <param name="ProcDamage"></param>
/// <param name="ElementalResistance"></param>
public record ElementalDebuffInfo(int? BuildUpMax = null, int? DurationMax = null, int? DotDamage = null, int? ProcDamage = null, float? ElementalResistance = null);