using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.MissionSystem.Templates;

namespace Everglow.Commons.MissionSystem.Tests;

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

	public override string Description => "测试介绍: \n" + "[ItemDrawer,Type='2',Stack='9-11',StackColor='196,241,255']";

	public override MissionType MissionType => MissionType.SideStory;
}