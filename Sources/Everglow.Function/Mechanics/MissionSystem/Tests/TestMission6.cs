using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestMission6 : KillNPCMission
{
	public override List<KillNPCRequirement> DemandNPCs { get; init; } = [
			KillNPCRequirement.Create(
					[
						NPCID.MoonLordHead
					], 12, true),
				KillNPCRequirement.Create(
					[
						NPCID.DemonEye,
					], 3, true),
				];

	public override List<Item> RewardItems => [
			new Item(ItemID.Zenith),
				new Item(ItemID.GoldAxe, 10),
				];

	public override string DisplayName => "击杀月亮领主";

	public override MissionType MissionType => MissionType.Legendary;
}