using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class PlayerMissionViewAdapterTest
{
	private sealed class StubMission : PlayerMissionBase
	{
		public string NameValue { get; set; } = "definition-id";

		public string DisplayNameValue { get; set; } = "Display Name";

		public string DescriptionValue { get; set; } = "Mission description";

		public string HintValue { get; set; } = string.Empty;

		public MissionSourceBase? SourceValue { get; set; } = MissionSourceBase.Default;

		public MissionSourceBase? SubSourceValue { get; set; }

		public MissionType TypeValue { get; set; } = MissionType.None;

		public float ProgressValue { get; set; }

		public long TimeLimitValue { get; set; } = -1;

		public MissionIconGroup? IconValue { get; set; }

		public bool UseDefaultIcons { get; set; }

		public override string Name => NameValue;

		public override string DisplayName => DisplayNameValue;

		public override string Description => DescriptionValue;

		public override string Hint => HintValue;

		public override MissionSourceBase Source => SourceValue!;

		public override MissionSourceBase SubSource => SubSourceValue!;

		public override MissionType Type => TypeValue;

		public override float Progress => ProgressValue;

		public override long TimeLimit => TimeLimitValue;

		public override MissionIconGroup Icon => UseDefaultIcons ? base.Icon : IconValue!;
	}

	private sealed class StubObjective : PlayerObjectiveBase
	{
		private readonly string[] lines;

		public StubObjective(params string[] lines)
		{
			this.lines = lines;
		}

		public bool Ready { get; set; }

		public float ProgressValue { get; set; }

		public MissionIconBase? Icon { get; set; }

		public bool ThrowOnTextRead { get; set; }

		public override float Progress => ProgressValue;

		public override bool CheckCompletion() => Ready;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
			if (Icon is not null)
			{
				iconGroup.Add(Icon);
			}
		}

		public override void GetObjectivesText(List<string> output)
		{
			if (ThrowOnTextRead)
			{
				throw new InvalidOperationException("Hidden objective text must not be read.");
			}

			output.AddRange(lines);
		}
	}

	private sealed class StubSource : MissionSourceBase
	{
		public StubSource(string name)
		{
			Name = name;
		}

		public override Texture2D Texture => null!;

		public override string Name { get; }
	}

	private sealed class StubIcon : MissionIconBase
	{
		public override void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color, float baseScale)
		{
		}
	}

	[TestMethod]
	public void Create_MapsIdentityMetadataSourcesAndVisibility()
	{
		var source = new StubSource("source");
		var subSource = new StubSource("sub-source");
		var mission = new StubMission
		{
			NameValue = "mission-definition",
			DisplayNameValue = "Mission title",
			DescriptionValue = "Visible details",
			SourceValue = source,
			SubSourceValue = subSource,
			TypeValue = MissionType.Legendary,
			IsVisible = false,
			State = PlayerMissionState.Available,
		};

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.AreEqual(MissionSide.Player, view.Identity.Side);
		Assert.AreEqual(mission.Name, view.Identity.DefinitionId);
		Assert.AreEqual(mission.InstanceId, view.Identity.InstanceId);
		Assert.IsTrue(Guid.TryParseExact(view.Identity.InstanceId, "N", out _));
		Assert.AreSame(source, view.Source);
		Assert.AreSame(subSource, view.SubSource);
		Assert.AreEqual(MissionType.Legendary, view.Type);
		Assert.AreEqual("Mission title", view.DisplayName);
		Assert.AreEqual("Visible details", view.Description);
		Assert.AreEqual(string.Empty, view.Hint);
		Assert.IsFalse(view.Visible);

		mission.SourceValue = null;
		MissionView defaultSourceView = PlayerMissionViewAdapter.Create(mission);

		Assert.AreSame(MissionSourceBase.Default, defaultSourceView.Source);
		Assert.AreSame(subSource, defaultSourceView.SubSource);
	}

	[TestMethod]
	[DataRow(PlayerMissionState.Available, MissionViewState.Available)]
	[DataRow(PlayerMissionState.Accepted, MissionViewState.Active)]
	[DataRow(PlayerMissionState.Completed, MissionViewState.Completed)]
	[DataRow(PlayerMissionState.Failed, MissionViewState.Failed)]
	[DataRow(PlayerMissionState.Overdue, MissionViewState.Overdue)]
	public void Create_MapsEveryPlayerState(PlayerMissionState state, MissionViewState expected)
	{
		var mission = new StubMission { State = state };

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.AreEqual(expected, view.State);
	}

	[TestMethod]
	public void Create_ClampsMissionProgressIncludingNaN()
	{
		var belowRange = new StubMission { ProgressValue = -0.25f };
		var aboveRange = new StubMission { ProgressValue = 1.25f };
		var notANumber = new StubMission { ProgressValue = float.NaN };
		var positiveInfinity = new StubMission { ProgressValue = float.PositiveInfinity };
		var negativeInfinity = new StubMission { ProgressValue = float.NegativeInfinity };

		Assert.AreEqual(0f, PlayerMissionViewAdapter.Create(belowRange).Progress);
		Assert.AreEqual(1f, PlayerMissionViewAdapter.Create(aboveRange).Progress);
		Assert.AreEqual(0f, PlayerMissionViewAdapter.Create(notANumber).Progress);
		Assert.AreEqual(1f, PlayerMissionViewAdapter.Create(positiveInfinity).Progress);
		Assert.AreEqual(0f, PlayerMissionViewAdapter.Create(negativeInfinity).Progress);
	}

	[TestMethod]
	public void Create_NormalizesTimeLimitsAndRemainingTime()
	{
		var expired = new StubMission
		{
			Time = 120,
			TimeLimitValue = 100,
		};
		var zeroLimit = new StubMission
		{
			Time = 25,
			TimeLimitValue = 0,
		};
		var negativeLimit = new StubMission
		{
			Time = 30,
			TimeLimitValue = -1,
		};

		MissionView expiredView = PlayerMissionViewAdapter.Create(expired);
		MissionView zeroLimitView = PlayerMissionViewAdapter.Create(zeroLimit);
		MissionView negativeLimitView = PlayerMissionViewAdapter.Create(negativeLimit);

		Assert.AreEqual(120L, expiredView.ElapsedTime);
		Assert.AreEqual(100L, expiredView.TimeLimit);
		Assert.AreEqual(0L, expiredView.RemainingTime);
		Assert.AreEqual(25L, zeroLimitView.ElapsedTime);
		Assert.IsNull(zeroLimitView.TimeLimit);
		Assert.IsNull(zeroLimitView.RemainingTime);
		Assert.AreEqual(30L, negativeLimitView.ElapsedTime);
		Assert.IsNull(negativeLimitView.TimeLimit);
		Assert.IsNull(negativeLimitView.RemainingTime);
	}

	[TestMethod]
	[DataRow("Follow the trail")]
	[DataRow(MissionHintText.Masked)]
	[DataRow(" ")]
	public void Create_AnyNonEmptyHintHidesDetailsButKeepsVisibilityAndIcons(string hint)
	{
		var visibleIcon = new StubIcon();
		var objective = new StubObjective("secret objective")
		{
			ProgressValue = 0.8f,
			ThrowOnTextRead = true,
		};
		var mission = new StubMission
		{
			HintValue = hint,
			DescriptionValue = "secret description",
			ProgressValue = 0.75f,
			Time = 45,
			TimeLimitValue = 120,
			IconValue = new MissionIconGroup(visibleIcon),
			IsVisible = false,
			State = PlayerMissionState.Accepted,
		};
		mission.Objectives.Add(objective);
		mission.RewardItems.Add(new Item());

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.AreEqual(hint, view.Hint);
		Assert.IsFalse(view.Visible);
		Assert.AreEqual(string.Empty, view.Description);
		Assert.IsEmpty(view.ObjectiveNodes);
		Assert.IsEmpty(view.Rewards);
		Assert.AreEqual(0f, view.Progress);
		Assert.AreEqual(0L, view.ElapsedTime);
		Assert.IsNull(view.TimeLimit);
		Assert.IsNull(view.RemainingTime);
		Assert.HasCount(1, view.Icons);
		Assert.AreSame(visibleIcon, view.Icons[0]);
	}

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

	[TestMethod]
	public void Create_FiltersSourceIconsKeepsObjectiveIconsAndSnapshotsTheResult()
	{
		var source = new StubSource("source");
		var subSource = new StubSource("sub-source");
		var sourceIcon = MissionSourceIcon.Create(source, subSource);
		var ordinaryIcon = new StubIcon();
		var addedLater = new StubIcon();
		var iconGroup = new MissionIconGroup(sourceIcon, ordinaryIcon);
		var mission = new StubMission
		{
			SourceValue = source,
			SubSourceValue = subSource,
			IconValue = iconGroup,
		};

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		iconGroup.Add(addedLater);

		Assert.HasCount(1, view.Icons);
		Assert.AreSame(ordinaryIcon, view.Icons[0]);

		var objectiveIcon = new StubIcon();
		var missionWithDefaultIcons = new StubMission
		{
			SourceValue = source,
			SubSourceValue = subSource,
			UseDefaultIcons = true,
		};
		missionWithDefaultIcons.Objectives.Add(new StubObjective("objective") { Icon = objectiveIcon });

		MissionView objectiveIconView = PlayerMissionViewAdapter.Create(missionWithDefaultIcons);

		Assert.HasCount(1, objectiveIconView.Icons);
		Assert.AreSame(objectiveIcon, objectiveIconView.Icons[0]);
	}

	[TestMethod]
	public void Create_NullIconProducesAnEmptySnapshot()
	{
		var mission = new StubMission { IconValue = null };

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.IsNotNull(view.Icons);
		Assert.IsEmpty(view.Icons);
	}

	[TestMethod]
	public void Create_MapsRewardItemsByReferenceAndSnapshotsTheCollection()
	{
		var firstReward = new Item { type = 1, stack = 3 };
		var secondReward = new Item { type = 2, stack = 5 };
		var mission = new StubMission();
		mission.RewardItems.Add(firstReward);

		MissionView view = PlayerMissionViewAdapter.Create(mission);
		mission.RewardItems.Add(secondReward);

		Assert.HasCount(1, view.Rewards);
		Assert.AreSame(firstReward, view.Rewards[0].Item);
		Assert.AreEqual(string.Empty, view.Rewards[0].Description);
	}
}
