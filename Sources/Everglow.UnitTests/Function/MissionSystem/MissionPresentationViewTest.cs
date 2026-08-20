using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class MissionPresentationViewTest
{
	[TestMethod]
	public void RemainingTime_IsClampedToZero()
	{
		var active = new MissionView { ElapsedTime = 40, TimeLimit = 100 };
		var expired = new MissionView { ElapsedTime = 120, TimeLimit = 100 };

		Assert.AreEqual(60, active.RemainingTime);
		Assert.AreEqual(0, expired.RemainingTime);
	}
}
