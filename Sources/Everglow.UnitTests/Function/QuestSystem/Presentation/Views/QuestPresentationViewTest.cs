using Everglow.Commons.Mechanics.Quest.Presentation.Views;

namespace Everglow.UnitTests.Function.QuestSystem;

[TestClass]
public class QuestPresentationViewTest
{
	[TestMethod]
	public void RemainingTime_IsClampedToZero()
	{
		var active = new QuestView { ElapsedTime = 40, TimeLimit = 100 };
		var expired = new QuestView { ElapsedTime = 120, TimeLimit = 100 };

		Assert.AreEqual(60, active.RemainingTime);
		Assert.AreEqual(0, expired.RemainingTime);
	}

	[TestMethod]
	public void PrimaryObjectiveTimer_ChoosesLeastRemainingActiveOrTimedOutTimerAcrossNodes()
	{
		var ignoredPending = new TimerView { TimeLimit = 10, ElapsedTime = 10 };
		var active = new TimerView { TimeLimit = 100, ElapsedTime = 40 };
		var timedOut = new TimerView { TimeLimit = 50, ElapsedTime = 50 };
		var quest = new QuestView
		{
			ObjectiveNodes =
			[
				new LeafObjectiveNodeView(new ObjectiveView
				{
					State = ObjectiveViewState.Pending,
					Timer = ignoredPending,
				}),
				new ParallelObjectiveNodeView(
				[
					new ObjectiveView { State = ObjectiveViewState.Active, Timer = active },
					new ObjectiveView { State = ObjectiveViewState.TimedOut, Timer = timedOut },
				]),
			],
		};

		Assert.AreSame(timedOut, quest.PrimaryObjectiveTimer);
	}

	[TestMethod]
	public void PrimaryObjectiveTimer_ReturnsNullWithoutRelevantTimer()
	{
		var quest = new QuestView
		{
			ObjectiveNodes =
			[
				new LeafObjectiveNodeView(new ObjectiveView
				{
					State = ObjectiveViewState.Pending,
					Timer = new TimerView { TimeLimit = 60 },
				}),
			],
		};

		Assert.IsNull(quest.PrimaryObjectiveTimer);
	}
}
