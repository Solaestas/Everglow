using System.Reflection;
using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class MissionPresentationSystemTest
{
	[TestMethod]
	public void PendingEvents_AreDistinctAndReentrantEventsWaitForNextUpdate()
	{
		const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
		const BindingFlags NonPublicNested = BindingFlags.NonPublic;
		var system = new MissionPresentationSystem();
		var identity = new MissionIdentity(MissionSide.Player, "TestMission", "instance");
		Type eventType = typeof(MissionPresentationSystem).GetNestedType("MissionEventType", NonPublicNested);
		MethodInfo queueEvent = typeof(MissionPresentationSystem).GetMethod("QueueEvent", NonPublicInstance);
		Assert.IsNotNull(eventType);
		Assert.IsNotNull(queueEvent);
		object statusUpdated = Enum.Parse(eventType, "StatusUpdated");
		int publishCount = 0;
		system.MissionStatusUpdated += publishedIdentity =>
		{
			Assert.AreEqual(identity, publishedIdentity);
			publishCount++;
			if (publishCount == 1)
			{
				queueEvent.Invoke(system, [statusUpdated, identity]);
			}
		};

		queueEvent.Invoke(system, [statusUpdated, identity]);
		queueEvent.Invoke(system, [statusUpdated, identity]);
		system.PostUpdateEverything();

		Assert.AreEqual(1, publishCount);

		system.PostUpdateEverything();

		Assert.AreEqual(2, publishCount);
	}

}
