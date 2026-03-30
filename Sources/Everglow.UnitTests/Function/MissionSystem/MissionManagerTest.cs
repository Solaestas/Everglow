using Everglow.Commons.Mechanics.Mission.WorldMission;
using Everglow.Commons.Mechanics.MissionSystem_New;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class MissionManagerTest
{
	private class TestStateProvider : IGameStateProvider
	{
		public double TimeForVisualEffects { get; set; }

		public bool GameMenu => false;

		public bool GameInactive => false;

		public bool GamePaused => false;
	}

	private class TestMission1 : MissionBase_New
	{
		public override string Name => nameof(TestMission1);
	}

	[TestMethod]
	public void GetMissionTest()
	{
		var manager = new MissionManager_New(new TestStateProvider());
		manager.AddMission(new TestMission1());
		var m = manager.GetMission<TestMission1>();
		Assert.IsNotNull(m);
	}

	private class TestMission2 : MissionBase_New
	{
		public TestMission2()
		{
			MissionState = WorldMissionState.Active;
		}

		public override string Name => nameof(TestMission2);

		public override int TimeLimit => 20;
	}

	[TestMethod]
	public void TimeLimitTest()
	{
		var provider = new TestStateProvider();
		provider.TimeForVisualEffects = 60;
		var manager = new MissionManager_New(provider);
		manager.AddMission(new TestMission2());
		var m = manager.GetMission<TestMission2>();
		Assert.IsNotNull(m);

		for (int i = 0; i < 30; i++)
		{
			manager.Update();

			if (i < 20)
			{
				Assert.AreEqual(WorldMissionState.Active, m.MissionState);
			}
			else
			{
				Assert.AreEqual(WorldMissionState.Failed, m.MissionState);
			}
		}
	}
}