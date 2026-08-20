using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.MissionStructure.Nodes;

namespace Everglow.UnitTests.Function.MissionSystem;

public partial class WorldMissionViewAdapterTest
{
	[TestMethod]
	public void Create_MapsAllNodeShapesInDefinitionOrderAndSnapshotsNodes()
	{
		var leaf = new StubObjective { ProgressValue = 0.1f };
		var parallelFirst = new StubObjective { ProgressValue = 0.2f };
		var parallelSecond = new StubObjective { ProgressValue = 0.3f };
		var anyOfFirst = new StubObjective { ProgressValue = 0.4f };
		var anyOfSecond = new StubObjective { ProgressValue = 0.5f };
		var branchAFirst = new StubObjective { ProgressValue = 0.6f };
		var branchASecond = new StubObjective { ProgressValue = 0.7f };
		var branchBFirst = new StubObjective { ProgressValue = 0.8f };
		var mission = new StubMission();
		mission.Objectives
			.Add(leaf)
			.AddParallel(parallelFirst, parallelSecond)
			.AddOptional(anyOfFirst, anyOfSecond)
			.AddBranch([branchAFirst, branchASecond], [branchBFirst]);

		MissionView view = WorldMissionViewAdapter.Create(mission);

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
		Assert.AreEqual(string.Empty, leafView.Objective.Description);
		CollectionAssert.AreEqual(new[] { 1, 2 }, parallelView.Objectives.Select(objective => objective.Id).ToArray());
		CollectionAssert.AreEqual(new[] { 3, 4 }, anyOfView.Objectives.Select(objective => objective.Id).ToArray());
		CollectionAssert.AreEqual(new[] { 5, 6 }, branchView.Branches[0].Objectives.Select(objective => objective.Id).ToArray());
		CollectionAssert.AreEqual(new[] { 7 }, branchView.Branches[1].Objectives.Select(objective => objective.Id).ToArray());
		Assert.AreEqual(ObjectiveBranchState.Candidate, branchView.Branches[0].State);
		Assert.AreEqual(ObjectiveBranchState.Candidate, branchView.Branches[1].State);
		Assert.IsTrue(view.ObjectiveNodes
			.SelectMany(GetObjectives)
			.All(objective => objective.Description == string.Empty));

		mission.Objectives.Add(new StubObjective());
		leaf.ProgressValue = 0.9f;
		Assert.HasCount(4, view.ObjectiveNodes);
		Assert.AreEqual(0.1f, leafView.Objective.Progress);
	}

	[TestMethod]
	public void Create_DerivesCompletedActiveAndPendingObjectiveStates()
	{
		var completed = new StubObjective { ProgressValue = -1f };
		var active = new StubObjective { ProgressValue = 1.5f };
		var pending = new StubObjective { ProgressValue = 0.25f };
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Active);
		mission.Objectives.Add(completed).Add(active).Add(pending);
		completed.Complete();

		MissionView view = WorldMissionViewAdapter.Create(mission);
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
	public void Create_ClampsActiveObjectiveProgressIncludingNaN()
	{
		var belowRange = new StubObjective { ProgressValue = -0.5f };
		var notANumber = new StubObjective { ProgressValue = float.NaN };
		var aboveRange = new StubObjective { ProgressValue = 1.5f };
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Active);
		mission.Objectives.AddParallel(belowRange, notANumber, aboveRange);

		MissionView view = WorldMissionViewAdapter.Create(mission);
		var parallel = (ParallelObjectiveNodeView)view.ObjectiveNodes.Single();

		Assert.IsTrue(parallel.Objectives.All(objective => objective.State == ObjectiveViewState.Active));
		CollectionAssert.AreEqual(new[] { 0f, 0f, 1f }, parallel.Objectives.Select(objective => objective.Progress).ToArray());
	}

	[TestMethod]
	public void Create_MapsCandidateBranchesAndOnlyTheirHeadsAsActive()
	{
		var firstHead = new StubObjective();
		var firstContinuation = new StubObjective();
		var secondHead = new StubObjective();
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Active);
		mission.Objectives.AddBranch([firstHead, firstContinuation], [secondHead]);

		MissionView view = WorldMissionViewAdapter.Create(mission);
		var branch = (BranchObjectiveNodeView)view.ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Candidate, branch.Branches[0].State);
		Assert.AreEqual(ObjectiveBranchState.Candidate, branch.Branches[1].State);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[0].Objectives[0].State);
		Assert.AreEqual(ObjectiveViewState.Pending, branch.Branches[0].Objectives[1].State);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[1].Objectives[0].State);
	}

	[TestMethod]
	public void Create_CompletedStateTakesPriorityOverCandidateHeadBeingActive()
	{
		var completedHead = new StubObjective { ProgressValue = -1f };
		var activeHead = new StubObjective();
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Active);
		mission.Objectives.AddBranch([completedHead], [activeHead]);
		completedHead.Complete();

		MissionView view = WorldMissionViewAdapter.Create(mission);
		var branch = (BranchObjectiveNodeView)view.ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Candidate, branch.Branches[0].State);
		Assert.AreEqual(ObjectiveViewState.Completed, branch.Branches[0].Objectives[0].State);
		Assert.AreEqual(1f, branch.Branches[0].Objectives[0].Progress);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[1].Objectives[0].State);
	}

	[TestMethod]
	public void Create_MapsSelectedAndSkippedBranchesWithSkippedStateTakingPriority()
	{
		var skippedCompleted = new StubObjective { ProgressValue = 0.9f };
		var skippedPending = new StubObjective { ProgressValue = 1.4f };
		var selectedHead = new StubObjective { Ready = true, ProgressValue = 0.7f };
		var selectedContinuation = new StubObjective { ProgressValue = 0.4f };
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Active);
		mission.Objectives.AddBranch(
			[skippedCompleted, skippedPending],
			[selectedHead, selectedContinuation]);
		skippedCompleted.Complete();
		var node = (WorldBranchNode)mission.Objectives.AllNodes.Single();
		node.Complete();

		MissionView view = WorldMissionViewAdapter.Create(mission);
		var branch = (BranchObjectiveNodeView)view.ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Skipped, branch.Branches[0].State);
		Assert.IsTrue(branch.Branches[0].Objectives.All(objective => objective.State == ObjectiveViewState.Skipped));
		Assert.IsTrue(branch.Branches[0].Objectives.All(objective => objective.Progress == 0f));
		Assert.AreEqual(ObjectiveBranchState.Selected, branch.Branches[1].State);
		Assert.AreEqual(ObjectiveViewState.Completed, branch.Branches[1].Objectives[0].State);
		Assert.AreEqual(1f, branch.Branches[1].Objectives[0].Progress);
		Assert.AreEqual(ObjectiveViewState.Active, branch.Branches[1].Objectives[1].State);
		Assert.AreEqual(0.4f, branch.Branches[1].Objectives[1].Progress);

		selectedContinuation.Ready = true;
		node.Complete();
		var completedBranch = (BranchObjectiveNodeView)WorldMissionViewAdapter.Create(mission).ObjectiveNodes.Single();

		Assert.AreEqual(ObjectiveBranchState.Selected, completedBranch.Branches[1].State);
		Assert.IsTrue(completedBranch.Branches[1].Objectives.All(objective => objective.State == ObjectiveViewState.Completed));
	}

	[TestMethod]
	public void Create_NonActiveMissionNeverMarksCurrentObjectiveActive()
	{
		var objective = new StubObjective { ProgressValue = 0.3f };
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Locked);
		mission.Objectives.Add(objective);

		MissionView view = WorldMissionViewAdapter.Create(mission);
		var objectiveView = ((LeafObjectiveNodeView)view.ObjectiveNodes.Single()).Objective;

		Assert.AreEqual(ObjectiveViewState.Pending, objectiveView.State);
		Assert.AreEqual(0.3f, objectiveView.Progress);
	}

	private static IEnumerable<ObjectiveView> GetObjectives(ObjectiveNodeView node)
	{
		return node switch
		{
			LeafObjectiveNodeView leaf => [leaf.Objective],
			ParallelObjectiveNodeView parallel => parallel.Objectives,
			AnyOfObjectiveNodeView anyOf => anyOf.Objectives,
			BranchObjectiveNodeView branch => branch.Branches.SelectMany(value => value.Objectives),
			_ => throw new InvalidDataException($"Unknown objective node view {node.GetType().FullName}."),
		};
	}
}
