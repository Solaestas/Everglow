using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.UIMissionObjectives;
using Everglow.UnitTests.Functions.MissionSystem.TestMissions;

namespace Everglow.UnitTests.Functions.MissionSystem;

[TestClass]
public class MissionObjectiveRendererTest
{
	[TestMethod]
	public void Test()
	{
		// Contain Test
		Assert.IsTrue(MissionObjectiveRendererFactory
			.GetRenderer(new UnitTestCollectItemMission1())
			.Any(x => x.GetType() == typeof(UICollectItemMissionObjectiveRenderer)));
		Assert.IsTrue(MissionObjectiveRendererFactory
			.GetRenderer(new UnitTestKillNPCMission1())
			.Any(x => x.GetType() == typeof(UIKillNPCMissionObjectiveRenderer)));

		// Dosen't Contain Test
		Assert.IsFalse(MissionObjectiveRendererFactory
			.GetRenderer(new UnitTestCollectItemMission1())
			.Any(x => x.GetType() == typeof(UIKillNPCMissionObjectiveRenderer)));
		Assert.IsFalse(MissionObjectiveRendererFactory
			.GetRenderer(new UnitTestKillNPCMission1())
			.Any(x => x.GetType() == typeof(UICollectItemMissionObjectiveRenderer)));
	}
}