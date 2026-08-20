using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.WorldSide.MissionStructure.Nodes;
using Terraria;
using Terraria.ModLoader.IO;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class WorldMissionViewAdapterTest
{
	private sealed class StubMission : WorldMissionBase
	{
		public string NameValue { get; set; } = "world-definition";

		public string DisplayNameValue { get; set; } = "World Mission";

		public string HintValue { get; set; } = string.Empty;

		public bool VisibleValue { get; set; } = true;

		public float ProgressValue { get; set; }

		public int TimeLimitValue { get; set; }

		public bool ThrowOnDetailRead { get; set; }

		public int ProgressReadCount { get; private set; }

		public int TimeLimitReadCount { get; private set; }

		public override string Name => NameValue;

		public override string DisplayName => DisplayNameValue;

		public override string Hint => HintValue;

		public override bool Visible => VisibleValue;

		public override float Progress
		{
			get
			{
				ProgressReadCount++;
				if (ThrowOnDetailRead)
				{
					throw new InvalidOperationException("Hidden mission progress must not be read.");
				}

				return ProgressValue;
			}
		}

		public override int TimeLimit
		{
			get
			{
				TimeLimitReadCount++;
				if (ThrowOnDetailRead)
				{
					throw new InvalidOperationException("Hidden mission time limit must not be read.");
				}

				return TimeLimitValue;
			}
		}

		public void SetState(WorldMissionState state) => State = state;

		public void SetTime(int time) => Time = time;

		public void SetRewards(params Item[] items) => RewardItems = [.. items];

		public void AddReward(Item item)
		{
			RewardItems ??= [];
			RewardItems.Add(item);
		}
	}

	private sealed class StubObjective : WorldObjectiveBase
	{
		public bool Ready { get; set; }

		public float ProgressValue { get; set; }

		public bool ThrowOnProgressRead { get; set; }

		public int ProgressReadCount { get; private set; }

		public int CheckCompletionCalls { get; private set; }

		public int TextReadCalls { get; private set; }

		public int UpdateCalls { get; private set; }

		public int CompleteCalls { get; private set; }

		public int ActivateCalls { get; private set; }

		public int DeactivateCalls { get; private set; }

		public int ResetCalls { get; private set; }

		public int PersistenceCalls { get; private set; }

		public int NetworkCalls { get; private set; }

		public override float Progress
		{
			get
			{
				ProgressReadCount++;
				if (ThrowOnProgressRead)
				{
					throw new InvalidOperationException("Hidden objective progress must not be read.");
				}

				return ProgressValue;
			}
		}

		public override bool CheckCompletion()
		{
			CheckCompletionCalls++;
			return Ready;
		}

		public override void GetObjectivesText()
		{
			TextReadCalls++;
			throw new InvalidOperationException("World objective text is not implemented and must not be read.");
		}

		public override void Update() => UpdateCalls++;

		public override void Complete()
		{
			CompleteCalls++;
			base.Complete();
		}

		public override void Activate(WorldMissionBase sourceMission) => ActivateCalls++;

		public override void Deactivate() => DeactivateCalls++;

		public override void ResetProgress()
		{
			ResetCalls++;
			base.ResetProgress();
		}

		public override void LoadData(TagCompound tag) => PersistenceCalls++;

		public override void SaveData(TagCompound tag) => PersistenceCalls++;

		public override void NetSend(BinaryWriter writer) => NetworkCalls++;

		public override void NetReceive(BinaryReader reader) => NetworkCalls++;

		public override void SendDelta(BinaryWriter writer) => NetworkCalls++;

		public override void ReceiveDelta(BinaryReader reader) => NetworkCalls++;

		public override void SendMain(BinaryWriter writer) => NetworkCalls++;

		public override void ReceiveMain(BinaryReader reader) => NetworkCalls++;
	}

	[TestMethod]
	public void Create_UsesMissionNameForBothWorldIdentityPartsInsteadOfWhoAmI()
	{
		var mission = new StubMission { NameValue = "stable-world-definition" };
		SetWhoAmI(mission, 731);

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreEqual(731, mission.WhoAmI);
		Assert.AreEqual(MissionSide.World, view.Identity.Side);
		Assert.AreEqual("stable-world-definition", view.Identity.DefinitionId);
		Assert.AreEqual("stable-world-definition", view.Identity.InstanceId);
		Assert.AreNotEqual(mission.WhoAmI.ToString(), view.Identity.DefinitionId);
		Assert.AreNotEqual(mission.WhoAmI.ToString(), view.Identity.InstanceId);
	}

	[TestMethod]
	[DataRow(WorldMissionState.Locked, MissionViewState.Locked)]
	[DataRow(WorldMissionState.Active, MissionViewState.Active)]
	[DataRow(WorldMissionState.Completed, MissionViewState.Completed)]
	[DataRow(WorldMissionState.Failed, MissionViewState.Failed)]
	public void Create_MapsEveryWorldState(WorldMissionState state, MissionViewState expected)
	{
		var mission = new StubMission();
		mission.SetState(state);

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreEqual(expected, view.State);
	}

	[TestMethod]
	public void Create_NormalizesWorldMetadataWithoutInventingSubSourceOrIcons()
	{
		var mission = new StubMission
		{
			DisplayNameValue = "Mapped world mission",
			VisibleValue = false,
		};

		Assert.IsNull(mission.Source);
		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreSame(MissionSourceBase.Default, view.Source);
		Assert.IsNull(view.SubSource);
		Assert.AreEqual(MissionType.None, view.Type);
		Assert.AreEqual("Mapped world mission", view.DisplayName);
		Assert.AreEqual(string.Empty, view.Description);
		Assert.AreEqual(string.Empty, view.Hint);
		Assert.IsFalse(view.Visible);
		Assert.IsNotNull(view.Icons);
		Assert.IsEmpty(view.Icons);
	}

	[TestMethod]
	public void Create_ClampsMissionProgressIncludingNaNAndInfinity()
	{
		(float Domain, float Expected)[] cases =
		[
			(-0.25f, 0f),
			(0.4f, 0.4f),
			(1.25f, 1f),
			(float.NaN, 0f),
			(float.PositiveInfinity, 1f),
			(float.NegativeInfinity, 0f),
		];

		foreach ((float domain, float expected) in cases)
		{
			var mission = new StubMission { ProgressValue = domain };

			MissionView view = WorldMissionViewAdapter.Create(mission);

			Assert.AreEqual(expected, view.Progress);
		}
	}

	[TestMethod]
	public void Create_NormalizesWorldTimeLimitsAndRemainingTime()
	{
		var expired = new StubMission { TimeLimitValue = 100 };
		expired.SetTime(120);
		var zeroLimit = new StubMission { TimeLimitValue = 0 };
		zeroLimit.SetTime(25);
		var negativeLimit = new StubMission { TimeLimitValue = -10 };
		negativeLimit.SetTime(30);

		MissionView expiredView = WorldMissionViewAdapter.Create(expired);
		MissionView zeroLimitView = WorldMissionViewAdapter.Create(zeroLimit);
		MissionView negativeLimitView = WorldMissionViewAdapter.Create(negativeLimit);

		Assert.AreEqual(120, expiredView.ElapsedTime);
		Assert.AreEqual(100, expiredView.TimeLimit);
		Assert.AreEqual(0, expiredView.RemainingTime);
		Assert.AreEqual(25, zeroLimitView.ElapsedTime);
		Assert.IsNull(zeroLimitView.TimeLimit);
		Assert.IsNull(zeroLimitView.RemainingTime);
		Assert.AreEqual(30, negativeLimitView.ElapsedTime);
		Assert.IsNull(negativeLimitView.TimeLimit);
		Assert.IsNull(negativeLimitView.RemainingTime);
	}

	[TestMethod]
	[DataRow("Follow the trail")]
	[DataRow(MissionHintText.Masked)]
	public void Create_NonWhitespaceHintShortCircuitsAllHiddenDetailReads(string hint)
	{
		var reward = new Item { type = 1, stack = 2 };
		var objective = new StubObjective
		{
			ProgressValue = 0.8f,
			ThrowOnProgressRead = true,
		};
		var mission = new StubMission
		{
			HintValue = hint,
			VisibleValue = false,
			ProgressValue = 0.75f,
			TimeLimitValue = 120,
			ThrowOnDetailRead = true,
		};
		mission.SetState(WorldMissionState.Active);
		mission.SetTime(45);
		mission.Objectives.Add(objective);
		mission.SetRewards(reward);

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreEqual(hint, view.Hint);
		Assert.IsFalse(view.Visible);
		Assert.AreEqual(MissionViewState.Active, view.State);
		Assert.AreEqual(string.Empty, view.Description);
		Assert.IsEmpty(view.ObjectiveNodes);
		Assert.IsEmpty(view.Rewards);
		Assert.AreEqual(0f, view.Progress);
		Assert.AreEqual(0, view.ElapsedTime);
		Assert.IsNull(view.TimeLimit);
		Assert.IsNull(view.RemainingTime);
		Assert.AreEqual(0, mission.ProgressReadCount);
		Assert.AreEqual(0, mission.TimeLimitReadCount);
		Assert.AreEqual(0, objective.ProgressReadCount);
		Assert.AreEqual(0, objective.TextReadCalls);
		Assert.IsFalse(mission.RewardClaimed);
	}

	[TestMethod]
	[DataRow("")]
	[DataRow(" ")]
	[DataRow("\t")]
	public void Create_BlankHintExportsDetailsAndLeavesWorldObjectiveDescriptionsEmpty(string hint)
	{
		var reward = new Item { type = 2, stack = 3 };
		var objective = new StubObjective { ProgressValue = 0.35f };
		var mission = new StubMission
		{
			HintValue = hint,
			ProgressValue = 0.5f,
			TimeLimitValue = 120,
		};
		mission.SetState(WorldMissionState.Active);
		mission.SetTime(45);
		mission.Objectives.Add(objective);
		mission.SetRewards(reward);

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreEqual(hint, view.Hint);
		Assert.AreEqual(0.5f, view.Progress);
		Assert.AreEqual(45, view.ElapsedTime);
		Assert.AreEqual(120, view.TimeLimit);
		Assert.AreEqual(75, view.RemainingTime);
		Assert.HasCount(1, view.ObjectiveNodes);
		Assert.HasCount(1, view.Rewards);
		var objectiveView = ((LeafObjectiveNodeView)view.ObjectiveNodes[0]).Objective;
		Assert.AreEqual(string.Empty, objectiveView.Description);
		Assert.AreEqual(0, objective.TextReadCalls);
	}

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

	[TestMethod]
	public void Create_MapsRewardItemsByReferenceAndSnapshotsWithoutClaimingThem()
	{
		var firstReward = new Item { type = 1, stack = 3 };
		var secondReward = new Item { type = 2, stack = 5 };
		var mission = new StubMission();
		mission.SetState(WorldMissionState.Completed);
		mission.AddReward(firstReward);

		MissionView view = WorldMissionViewAdapter.Create(mission);
		mission.AddReward(secondReward);

		Assert.HasCount(1, view.Rewards);
		Assert.AreSame(firstReward, view.Rewards[0].Item);
		Assert.AreEqual(string.Empty, view.Rewards[0].Description);
		Assert.IsFalse(mission.RewardClaimed);
		Assert.IsEmpty(mission.RewardClaimedPlayers);
	}

	[TestMethod]
	public void Create_DoesNotTriggerObjectiveBehaviorPersistenceNetworkOrRewardClaims()
	{
		var reward = new Item { type = 1, stack = 1 };
		var objective = new StubObjective { ProgressValue = 0.6f };
		var mission = new StubMission
		{
			ProgressValue = 0.6f,
			TimeLimitValue = 300,
		};
		mission.SetState(WorldMissionState.Active);
		mission.SetTime(90);
		mission.Objectives.Add(objective);
		mission.SetRewards(reward);
		SetWhoAmI(mission, 19);

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreEqual(WorldMissionState.Active, mission.State);
		Assert.AreEqual(90, mission.Time);
		Assert.AreEqual(19, mission.WhoAmI);
		Assert.IsFalse(mission.RewardClaimed);
		Assert.IsEmpty(mission.RewardClaimedPlayers);
		Assert.IsFalse(objective.Completed);
		Assert.AreEqual(0, objective.CheckCompletionCalls);
		Assert.AreEqual(0, objective.TextReadCalls);
		Assert.AreEqual(0, objective.UpdateCalls);
		Assert.AreEqual(0, objective.CompleteCalls);
		Assert.AreEqual(0, objective.ActivateCalls);
		Assert.AreEqual(0, objective.DeactivateCalls);
		Assert.AreEqual(0, objective.ResetCalls);
		Assert.AreEqual(0, objective.PersistenceCalls);
		Assert.AreEqual(0, objective.NetworkCalls);
		Assert.HasCount(1, view.Rewards);
		Assert.AreSame(reward, view.Rewards[0].Item);
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

	private static void SetWhoAmI(WorldMissionBase mission, int value)
	{
		var property = typeof(WorldMissionBase).GetProperty(nameof(WorldMissionBase.WhoAmI));
		Assert.IsNotNull(property);
		var setter = property.GetSetMethod(nonPublic: true);
		Assert.IsNotNull(setter);
		setter.Invoke(mission, [value]);
	}
}
