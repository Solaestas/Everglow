using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.Templates;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class TestMission4 : KillNPCMission
{
	public override List<KillNPCRequirement> DemandNPCs { get; init; } = [
		KillNPCRequirement.Create(
				[
					NPCID.BlueSlime,
					NPCID.IceSlime,
					NPCID.SpikedJungleSlime,
					NPCID.MotherSlime,
				], 10, true),
			KillNPCRequirement.Create(
				[
					NPCID.DemonEye,
				], 3, true),
			];

	public override List<Item> RewardItems => [
		new Item(ItemID.Zenith),
			new Item(ItemID.GoldAxe, 10),
			];

	public override string DisplayName => "击杀10个史莱姆";

	public override MissionType MissionType => MissionType.SideStory;
}