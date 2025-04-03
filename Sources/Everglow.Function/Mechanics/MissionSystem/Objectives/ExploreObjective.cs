using Everglow.Commons.Mechanics.MissionSystem.Core;
using Terraria.GameContent.Personalities;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class ExploreObjective : MissionObjectiveBase
{
	public ExploreObjective()
	{
	}

	public ExploreObjective(IShoppingBiome biome)
	{
		Biome = biome;
	}

	public IShoppingBiome Biome { get; set; }

	public override bool CheckCompletion() => Biome.IsInBiome(Main.LocalPlayer);

	public override void GetObjectivesText(List<string> lines) =>
		lines.Add("探索" + (Biome is ModBiome modBiome
			? modBiome.DisplayName.ToString()
			: Biome.NameKey));
}