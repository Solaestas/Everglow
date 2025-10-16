using System.Diagnostics.CodeAnalysis;
using Everglow.Commons.Utilities;
using Terraria.Localization;

namespace Everglow.Commons.Mechanics.BiomesText;

public static class VanillaBiomes
{
	public const string Ocean = nameof(Ocean);
	public const string Forest = nameof(Forest);
	public const string Dungeon = nameof(Dungeon);
	public const string Glowshroom = nameof(Glowshroom);
	public const string LihzhardTemple = nameof(LihzhardTemple);
	public const string Cavern = nameof(Cavern);
	public const string Space = nameof(Space);
	public const string Underworld = nameof(Underworld);
	public const string Desert = nameof(Desert);
	public const string Corruption = nameof(Corruption);
	public const string Crimson = nameof(Crimson);
	public const string Hallow = nameof(Hallow);
	public const string Jungle = nameof(Jungle);
	public const string Snow = nameof(Snow);
	public const string UndergroundDesert = nameof(UndergroundDesert);
	public const string Underground = nameof(Underground);
	public const string UndergroundCrimson = nameof(UndergroundCrimson);
	public const string UndergroundCorruption = nameof(UndergroundCorruption);
	public const string UndergroundHallow = nameof(UndergroundHallow);
	public const string UndergroundJungle = nameof(UndergroundJungle);
	public const string UndergroundSnow = nameof(UndergroundSnow);
	public const string UndergroundGlowshroom = nameof(UndergroundGlowshroom);
	public const string Granite = nameof(Granite);
	public const string Hive = nameof(Hive);
	public const string Marble = nameof(Marble);
	public const string Meteor = nameof(Meteor);
	public const string Graveyard = nameof(Graveyard);
	public const string Shimmer = nameof(Shimmer);
	public const string GemCave = nameof(GemCave);
	public const string WaterCandle = nameof(WaterCandle);
	public const string PeaceCandle = nameof(PeaceCandle);
	public const string ShadowCandle = nameof(ShadowCandle);
	public const string Rain = nameof(Rain);
	public const string Sandstorm = nameof(Sandstorm);
	public const string TowerNebula = nameof(TowerNebula);
	public const string TowerSolar = nameof(TowerSolar);
	public const string TowerStardust = nameof(TowerStardust);
	public const string TowerVortex = nameof(TowerVortex);
	public const string OldOneArmy = nameof(OldOneArmy);

	public static readonly IReadOnlyCollection<string> BiomeKeys =
	[
		Forest, // Surface biomes
        Snow,
		Desert,
		Corruption,
		Crimson,
		Jungle,
		Hallow,
		Dungeon,
		Ocean,
		Glowshroom,

        // Underground biomes
        Underground,
		UndergroundDesert,

        // Cavern biomes
        Cavern,
		UndergroundCrimson,
		UndergroundCorruption,
		UndergroundHallow,
		UndergroundJungle,
		UndergroundSnow,
		UndergroundGlowshroom,

        // Space and Underworld biomes
        Space,
		Underworld,

        // Mini-biomes
        Granite,
		Hive,
		Marble,
		LihzhardTemple,
		Meteor,
		Graveyard,
		Shimmer,

        // Micro-biomes
        GemCave,

        // Candles
        // WaterCandle,
        // PeaceCandle,
        // ShadowCandle,

        // Weather and event
        // Rain,
        // Sandstorm,
        // TowerNebula,
        // TowerSolar,
        // TowerStardust,
        // TowerVortex,
        // OldOneArmy,
    ];

	public struct BiomeData
	{
		public string Key { get; }

		public string DisplayName => Language.GetTextValue($"Mods.Everglow.Biomes.VanillaBiomes.{Key}");

		public Func<Player, bool> Condition { get; }

		[MaybeNull]
		public Texture2D Icon { get; }

		public BiomeData(string key, Func<Player, bool> condition = null, Texture2D icon = null)
		{
			Key = key;
			Condition = condition ?? (p => false);
			Icon = icon ?? ModAsset.Forest.Value;
		}
	}

	public static readonly Dictionary<string, BiomeData> VanillaBiomeIndex = [];

	static VanillaBiomes()
	{
		VanillaBiomeIndex = new Dictionary<string, BiomeData>
		{
            // Surface biomes
			{ Forest, new BiomeData(Forest, p => p.ZoneForest, ModAsset.Forest.Value) },
			{ Snow, new BiomeData(Snow, p => p.InSurfaceAndUndergroundBiome(p.ZoneSnow), ModAsset.Snow.Value) },
			{ Desert, new BiomeData(Desert, p => p.ZoneDesert && !p.ZoneUndergroundDesert, ModAsset.Desert.Value) },
			{ Corruption, new BiomeData(Corruption, p => p.InSurfaceAndUndergroundBiome(p.ZoneCorrupt), ModAsset.Corruption.Value) },
			{ Crimson, new BiomeData(Crimson, p => p.InSurfaceAndUndergroundBiome(p.ZoneCrimson), ModAsset.Crimson.Value) },
			{ Jungle, new BiomeData(Jungle, p => p.InSurfaceAndUndergroundBiome(p.ZoneJungle), ModAsset.Jungle.Value) },
			{ Hallow, new BiomeData(Hallow, p => p.InSurfaceAndUndergroundBiome(p.ZoneHallow), ModAsset.Hallow.Value) },
			{ Dungeon, new BiomeData(Dungeon, p => p.ZoneDungeon, ModAsset.Dungeon.Value) },
			{ Ocean, new BiomeData(Ocean, p => p.ZoneBeach, ModAsset.Beach.Value) },
			{ Glowshroom, new BiomeData(Glowshroom, p => p.InSurfaceAndUndergroundBiome(p.ZoneGlowshroom), ModAsset.Glowshroom.Value) },

			// Underground biomes
			{ Underground, new BiomeData(Underground, p => p.ZoneNormalUnderground, ModAsset.Underground.Value) },
			{ UndergroundDesert, new BiomeData(UndergroundDesert, p => p.ZoneUndergroundDesert, ModAsset.Underground_Desert.Value) },

			// Cavern biomes
			{ Cavern, new BiomeData(Cavern, p => p.ZoneNormalCaverns, ModAsset.Cavern.Value) },
			{ UndergroundCrimson, new BiomeData(UndergroundCrimson, p => p.InCavernBiome(p.ZoneCrimson), ModAsset.Underground_Crimson.Value) },
			{ UndergroundCorruption, new BiomeData(UndergroundCorruption, p => p.InCavernBiome(p.ZoneCorrupt), ModAsset.Underground_Corruption.Value) },
			{ UndergroundHallow, new BiomeData(UndergroundHallow, p => p.InCavernBiome(p.ZoneHallow), ModAsset.Underground_Hallow.Value) },
			{ UndergroundJungle, new BiomeData(UndergroundJungle, p => p.InCavernBiome(p.ZoneJungle), ModAsset.Underground_Jungle.Value) },
			{ UndergroundSnow, new BiomeData(UndergroundSnow, p => p.InCavernBiome(p.ZoneSnow), ModAsset.Underground_Snowland.Value) },
			{ UndergroundGlowshroom, new BiomeData(UndergroundGlowshroom, p => p.InCavernBiome(p.ZoneGlowshroom), ModAsset.Glowshroom.Value) },

			// Space and Underworld biomes
			{ Space, new BiomeData(Space, p => p.ZoneNormalSpace, ModAsset.Space.Value) },
			{ Underworld, new BiomeData(Underworld, p => p.ZoneUnderworldHeight, ModAsset.Underworld.Value) },

			// Mini-biomes
			{ Granite, new BiomeData(Granite, p => p.ZoneGranite) },
			{ Hive, new BiomeData(Hive, p => p.ZoneHive) },
			{ Marble, new BiomeData(Marble, p => p.ZoneMarble) },
			{ LihzhardTemple, new BiomeData(LihzhardTemple, p => p.ZoneLihzhardTemple, ModAsset.LihzhardTemple.Value) },
			{ Meteor, new BiomeData(Meteor, p => p.ZoneMeteor) },
			{ Graveyard, new BiomeData(Graveyard, p => p.ZoneGraveyard) },
			{ Shimmer, new BiomeData(Shimmer, p => p.ZoneShimmer) },

			// Micro-biomes
			{ GemCave, new BiomeData(GemCave, p => p.ZoneGemCave) },

			// Candles
			{ WaterCandle, new BiomeData(WaterCandle, p => p.ZoneWaterCandle) },
			{ PeaceCandle, new BiomeData(PeaceCandle, p => p.ZonePeaceCandle) },
			{ ShadowCandle, new BiomeData(ShadowCandle, p => p.ZoneShadowCandle) },

			// Weather and event
			{ Rain, new BiomeData(Rain, p => p.ZoneRain) },
			{ Sandstorm, new BiomeData(Sandstorm, p => p.ZoneSandstorm) },
			{ TowerNebula, new BiomeData(TowerNebula, p => p.ZoneTowerNebula) },
			{ TowerSolar, new BiomeData(TowerSolar, p => p.ZoneTowerSolar) },
			{ TowerStardust, new BiomeData(TowerStardust, p => p.ZoneTowerStardust) },
			{ TowerVortex, new BiomeData(TowerVortex, p => p.ZoneTowerVortex) },
			{ OldOneArmy, new BiomeData(OldOneArmy, p => p.ZoneOldOneArmy) },
		};
	}
}