using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

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