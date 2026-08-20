using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class PlayerMissionViewAdapterTest
{
	[TestMethod]
	public void Create_MapsAllNodeShapesInDefinitionOrderAndSnapshotsNodes()
	{
		var leaf = new StubObjective("leaf line one", "leaf line two") { ProgressValue = 0.1f };
		var parallelFirst = new StubObjective("parallel first") { ProgressValue = 0.2f };
		var parallelSecond = new StubObjective("parallel second") { ProgressValue = 0.3f };
		var anyOfFirst = new StubObjective("any-of first") { ProgressValue = 0.4f };
		var anyOfSecond = new StubObjective("any-of second") { ProgressValue = 0.5f };
		var branchAFirst = new StubObjective("branch A first") { ProgressValue = 0.6f };
		var branchASecond = new StubObjective("branch A second") { ProgressValue = 0.7f };
		var branchBFirst = new StubObjective("branch B first") { ProgressValue = 0.8f };
		var mission = new StubMission { State = PlayerMissionState.Available };
		mission.Objectives
			.Add(leaf)
			.AddParallel(parallelFirst, parallelSecond)
			.AddOptional(anyOfFirst, anyOfSecond)
			.AddBranch([branchAFirst, branchASecond], [branchBFirst]);

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.HasCount(4, view.ObjectiveNodes);
		Assert.IsInstanceOfType<LeafObjectiveNodeView>(view.ObjectiveNodes[0]);
		Assert.IsInstanceOfType<ParallelObjectiveNodeView>(view.ObjectiveNodes[1]);
		Assert.IsInstanceOfType<AnyOfObjectiveNodeView>(view.ObjectiveNodes[2]);
		Assert.IsInstanceOfType<BranchObjectiveNodeView>(view.ObjectiveNodes[3]);

		var leafView = (LeafObjectiveNodeView)view.ObjectiveNodes[0];
		var parallelView = (ParallelObjectiveNodeView)view.ObjectiveNodes[1];
		var anyOfView = (AnyOfObjectiveNodeView)view.ObjectiveNodes[2];
		var branchView = (BranchObjectiveNodeView)view.ObjectiveNodes[3];
		Assert.AreEqual(0, leafView.Objective.Id);
		Assert.AreEqual("leaf line one\nleaf line two", leafView.Objective.Description);
		Assert.AreEqual(ObjectiveViewState.Pending, leafView.Objective.State);
		CollectionAssert.AreEqual(
			new[] { "parallel first", "parallel second" },
			parallelView.Objectives.Select(objective => objective.Description).ToArray());
		CollectionAssert.AreEqual(
			new[] { "any-of first", "any-of second" },
			anyOfView.Objectives.Select(objective => objective.Description).ToArray());
		Assert.AreEqual(ObjectiveBranchState.Candidate, branchView.Branches[0].State);
		Assert.AreEqual(ObjectiveBranchState.Candidate, branchView.Branches[1].State);
		CollectionAssert.AreEqual(
			new[] { "branch A first", "branch A second" },
			branchView.Branches[0].Objectives.Select(objective => objective.Description).ToArray());
		CollectionAssert.AreEqual(
			new[] { "branch B first" },
			branchView.Branches[1].Objectives.Select(objective => objective.Description).ToArray());
		CollectionAssert.AreEqual(new[] { 1, 2 }, parallelView.Objectives.Select(objective => objective.Id).ToArray());
		CollectionAssert.AreEqual(new[] { 3, 4 }, anyOfView.Objectives.Select(objective => objective.Id).ToArray());
		CollectionAssert.AreEqual(new[] { 5, 6 }, branchView.Branches[0].Objectives.Select(objective => objective.Id).ToArray());
		CollectionAssert.AreEqual(new[] { 7 }, branchView.Branches[1].Objectives.Select(objective => objective.Id).ToArray());

		mission.Objectives.Add(new StubObjective("added later"));
		leaf.ProgressValue = 0.9f;
		Assert.HasCount(4, view.ObjectiveNodes);
		Assert.AreEqual(0.1f, leafView.Objective.Progress);
	}

	[TestMethod]
	public void Create_DerivesCompletedActiveAndPendingObjectiveStates()
	{
		var completed = new StubObjective("completed") { ProgressValue = -1f };
		var active = new StubObjective("active") { ProgressValue = 1.5f };
		var pending = new StubObjective("pending") { ProgressValue = 0.25f };
		var mission = new StubMission { State = PlayerMissionState.Accepted };
		mission.Objectives.Add(completed).Add(active).Add(pending);
		completed.Complete();

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		var completedView = ((LeafObjectiveNodeView)view.ObjectiveNodes[0]).Objective;
		var activeView = ((LeafObjectiveNodeView)view.ObjectiveNodes[1]).Objective;
		var pendingView = ((LeafObjectiveNodeView)view.ObjectiveNodes[2]).Objective;

		Assert.AreEqual(ObjectiveViewState.Completed, completedView.State);
		Assert.AreEqual(1f, completedView.Progress);
		Assert.AreEqual(ObjectiveViewState.Active, activeView.State);
		Assert.AreEqual(1f, activeView.Progress);
		Assert.AreEqual(ObjectiveViewState.Pending, pendingView.State);
		Assert.AreEqual(0.25f, pendingView.Progress);
	}

	[TestMethod]
	public void Create_MapsCandidateBranchesAndOnlyTheirHeadsAsActive()
	{
		var firstHead = new StubObjective("first head");
		var firstContinuation = new StubObjective("first continuation");
		var secondHead = new StubObjective("second head");
		var mission = new StubMission { State = PlayerMissionState.Accepted };
		mission.Objectives.AddBranch([firstHead, firstContinuation], [secondHead]);

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		var branch = (BranchObjectiveNodeView)view.ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Candidate, branch.Branches[0].State);
		Assert.AreEqual(ObjectiveBranchState.Candidate, branch.Branches[1].State);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[0].Objectives[0].State);
		Assert.AreEqual(ObjectiveViewState.Pending, branch.Branches[0].Objectives[1].State);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[1].Objectives[0].State);
	}

	[TestMethod]
	public void Create_MapsSelectedAndSkippedBranchesWithSkippedStateTakingPriority()
	{
		var skippedCompleted = new StubObjective("skipped completed") { ProgressValue = 0.9f };
		var selectedHead = new StubObjective("selected head") { Ready = true, ProgressValue = 0.7f };
		var selectedContinuation = new StubObjective("selected continuation") { ProgressValue = 0.4f };
		var mission = new StubMission { State = PlayerMissionState.Accepted };
		mission.Objectives.AddBranch([skippedCompleted], [selectedHead, selectedContinuation]);
		skippedCompleted.Complete();
		mission.Objectives.Activate(mission);
		mission.Objectives.Update(mission);

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		var branch = (BranchObjectiveNodeView)view.ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Skipped, branch.Branches[0].State);
		Assert.AreEqual(ObjectiveViewState.Skipped, branch.Branches[0].Objectives[0].State);
		Assert.AreEqual(0f, branch.Branches[0].Objectives[0].Progress);
		Assert.AreEqual(ObjectiveBranchState.Selected, branch.Branches[1].State);
		Assert.AreEqual(ObjectiveViewState.Completed, branch.Branches[1].Objectives[0].State);
		Assert.AreEqual(1f, branch.Branches[1].Objectives[0].Progress);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[1].Objectives[1].State);
		Assert.AreEqual(0.4f, branch.Branches[1].Objectives[1].Progress);

		selectedContinuation.Ready = true;
		mission.Objectives.Update(mission);
		var completedBranch = (BranchObjectiveNodeView)PlayerMissionViewAdapter.Create(mission).ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Selected, completedBranch.Branches[1].State);
		Assert.IsTrue(completedBranch.Branches[1].Objectives.All(objective => objective.State == ObjectiveViewState.Completed));
	}
}
