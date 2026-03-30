using Everglow.Commons.Mechanics.Mission.PlayerMission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Enums;
using Everglow.Commons.Mechanics.Mission.PlayerMission.Shared.Icons;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Tests;

public class MissionIconTest : MissionBase
{
	public MissionIconTest()
	{
		Icon.Add(TextureMissionIcon.Create(ModAsset.AnnaTheGuard.Value, "Anna The Guard"));
		Icon.Add(NPCMissionIcon.Create(NPCID.EyeofCthulhu, nameof(NPCID.EyeofCthulhu)));
	}

	public override string DisplayName => GetType().Name;

	public override MissionType MissionType => MissionType.MainStory;
}