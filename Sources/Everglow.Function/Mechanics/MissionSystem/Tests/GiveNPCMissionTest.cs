using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Objectives;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class GiveNPCMissionTest : MissionBase
{
	public override string DisplayName => GetType().Name;

	public GiveNPCMissionTest()
	{
		Objectives.AddParallel([
			new TalkNPCObjective(NPCID.Guide, "Hi! Player."),
			new GiveItemObjective(new([ItemID.DirtBlock], 10))]);
	}
}
