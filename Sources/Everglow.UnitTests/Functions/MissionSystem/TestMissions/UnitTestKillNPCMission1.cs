using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;
using Terraria;

namespace Everglow.UnitTests.Functions.MissionSystem.TestMissions;

public class UnitTestKillNPCMission1 : KillNPCMission
{
	public UnitTestKillNPCMission1()
	{
		DemandNPCs = [];
	}

	public UnitTestKillNPCMission1(List<KillNPCRequirement> requires)
	{
		DemandNPCs = requires;
	}

	public override List<KillNPCRequirement> DemandNPCs { get; init; }

	public override List<Item> RewardItems => [];

	public override string DisplayName => nameof(UnitTestCollectItemMission1);

	public override string Description => nameof(UnitTestCollectItemMission1);
}