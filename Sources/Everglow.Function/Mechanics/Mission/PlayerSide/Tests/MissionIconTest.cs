using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Shared.Icons;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Tests;

public class MissionIconTest : PlayerMissionBase
{
	public MissionIconTest()
	{
		Icon.Add(TextureMissionIcon.Create(ModAsset.AnnaTheGuard.Value, "Anna The Guard"));
		Icon.Add(NPCMissionIcon.Create(NPCID.EyeofCthulhu, nameof(NPCID.EyeofCthulhu)));
	}

	public override string DisplayName => GetType().Name;

	public override MissionType MissionType => MissionType.MainStory;
}