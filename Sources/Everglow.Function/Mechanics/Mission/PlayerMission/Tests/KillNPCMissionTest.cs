using Everglow.Commons.Mechanics.Mission.PlayerMission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Enums;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Primitives;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Shared.Icons;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Objectives;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Tests;

public class KillNPCMissionTest : MissionBase
{
	public KillNPCMissionTest()
	{
		var objective = new KillNPCObjective(KillNPCRequirement.Create([NPCID.CursedSkull, NPCID.DemonEye], 10, true));
		Objectives.Add(objective);

		Icon.AddRange(objective.DemandNPC.NPCs.Select(i => NPCMissionIcon.Create(i)));

		RewardItems.Add(new(ItemID.DirtBlock, 10));
	}

	public override MissionSourceBase Source => MissionSourceTest1.Instance;

	public override MissionSourceBase SubSource => MissionSourceTest2.Instance;

	public override string DisplayName => nameof(KillNPCMissionTest);

	public override MissionType MissionType => MissionType.MainStory;
}