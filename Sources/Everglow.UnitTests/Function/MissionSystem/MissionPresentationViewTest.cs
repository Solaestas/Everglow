using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class MissionPresentationViewTest
{
	[TestMethod]
	public void MissionView_Defaults_AreSafeForPresentation()
	{
		var view = new MissionView();

		Assert.AreEqual(string.Empty, view.DisplayName);
		Assert.AreEqual(string.Empty, view.Description);
		Assert.AreEqual(string.Empty, view.Hint);
		Assert.IsNotNull(view.Source);
		Assert.IsNull(view.SubSource);
		Assert.IsNotNull(view.Icons);
		Assert.IsEmpty(view.Icons);
		Assert.IsNotNull(view.ObjectiveNodes);
		Assert.IsEmpty(view.ObjectiveNodes);
		Assert.IsNotNull(view.Rewards);
		Assert.IsEmpty(view.Rewards);
		Assert.IsNull(view.TimeLimit);
		Assert.IsNull(view.RemainingTime);
	}

	[TestMethod]
	public void RemainingTime_IsClampedToZero()
	{
		var active = new MissionView { ElapsedTime = 40, TimeLimit = 100 };
		var expired = new MissionView { ElapsedTime = 120, TimeLimit = 100 };

		Assert.AreEqual(60, active.RemainingTime);
		Assert.AreEqual(0, expired.RemainingTime);
	}

	[TestMethod]
	public void ObjectiveNodes_PreserveTypedStructure()
	{
		var first = new ObjectiveView { Id = 1, Description = "first" };
		var second = new ObjectiveView { Id = 2, Description = "second" };
		ObjectiveNodeView[] nodes =
		[
			new LeafObjectiveNodeView(first),
			new ParallelObjectiveNodeView([first, second]),
			new AnyOfObjectiveNodeView([second, first]),
			new BranchObjectiveNodeView(
			[
				new ObjectiveBranchView(ObjectiveBranchState.Candidate, [first]),
				new ObjectiveBranchView(ObjectiveBranchState.Selected, [second]),
			]),
		];

		Assert.IsInstanceOfType<LeafObjectiveNodeView>(nodes[0]);
		Assert.IsInstanceOfType<ParallelObjectiveNodeView>(nodes[1]);
		Assert.IsInstanceOfType<AnyOfObjectiveNodeView>(nodes[2]);
		Assert.IsInstanceOfType<BranchObjectiveNodeView>(nodes[3]);
		var branchNode = (BranchObjectiveNodeView)nodes[3];
		Assert.AreEqual(ObjectiveBranchState.Candidate, branchNode.Branches[0].State);
		Assert.AreSame(second, branchNode.Branches[1].Objectives[0]);
	}

	[TestMethod]
	public void RewardView_DefaultDescription_IsEmpty()
	{
		var reward = new RewardView();

		Assert.IsNull(reward.Item);
		Assert.AreEqual(string.Empty, reward.Description);
	}
}
