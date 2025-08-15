namespace Everglow.Commons.Utilities;

public static class BuffUtils
{
	public static readonly IReadOnlyCollection<int> VanillaFlameDebuffs =
	[
		BuffID.OnFire,
		BuffID.OnFire3,
		BuffID.Burning,
		BuffID.Frostburn,
		BuffID.Frostburn2,
		BuffID.ShadowFlame,
		BuffID.CursedInferno,
	];

	/// <summary>
	/// The value is life regen, use <see cref="Player.lifeRegenCount"/> to apply damage
	/// </summary>
	public static readonly IReadOnlyDictionary<int, int> VanillaDotDebuffDamageOnPlayer = new Dictionary<int, int>()
	{
		{ BuffID.OnFire, 8 },
		{ BuffID.OnFire3, 8 },
		{ BuffID.Burning, 60 },
		{ BuffID.Frostburn, 16 },
		{ BuffID.Frostburn2, 16 },
		{ BuffID.CursedInferno, 24 },
	};

	/// <summary>
	/// The value is life regen, use <see cref="NPC.lifeRegenCount"/> to apply damage
	/// </summary>
	public static readonly IReadOnlyDictionary<int, int> VanillaDotDebuffDamageOnNPC = new Dictionary<int, int>()
	{
		{ BuffID.OnFire, 8 },
		{ BuffID.OnFire3, 30 },
		{ BuffID.Frostburn, 16 },
		{ BuffID.Frostburn2, 50 },
		{ BuffID.ShadowFlame, 30 },
		{ BuffID.CursedInferno, 48 },
		{ BuffID.Oiled, 50 },
	};
}