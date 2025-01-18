using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionTemplates;
using Terraria;

namespace Everglow.UnitTests.Functions.MissionSystem.TestMissions;

public class UnitTestGainItemMission1 : GainItemMission
{
	public UnitTestGainItemMission1(List<GainItemRequirement> requires)
	{
		DemandGainItems = requires;
	}

	public override List<GainItemRequirement> DemandGainItems { get; init; }

	public override List<Item> RewardItems => [];

	public override string DisplayName => nameof(UnitTestGainItemMission1);

	public override string Description => nameof(UnitTestGainItemMission1);
}
