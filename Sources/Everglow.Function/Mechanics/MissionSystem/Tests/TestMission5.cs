using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestMission5 : KillNPCMission
{
	public override List<KillNPCRequirement> DemandNPCs { get; init; } = [
			KillNPCRequirement.Create(
					[
						NPCID.EyeofCthulhu,
					], 1, false)
		];

	public override List<Item> RewardItems => [
			new Item(ItemID.Zenith),
				new Item(ItemID.GoldAxe, 10),
				];

	public override string DisplayName => "击杀克苏鲁之眼";

	public override MissionType MissionType => MissionType.Challenge;
}
