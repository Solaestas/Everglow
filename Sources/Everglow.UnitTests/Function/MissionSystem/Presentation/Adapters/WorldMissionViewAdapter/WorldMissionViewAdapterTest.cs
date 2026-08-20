using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Adapters;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public partial class WorldMissionViewAdapterTest
{
	private sealed class StubMission : WorldMissionBase
	{
		public string NameValue { get; set; } = "world-definition";

		public string DisplayNameValue { get; set; } = "World Mission";

		public string DescriptionValue { get; set; } = string.Empty;

		public string HintValue { get; set; } = string.Empty;

		public MissionSourceBase SourceValue { get; set; } = MissionSourceBase.Default;

		public MissionType TypeValue { get; set; } = MissionType.None;

		public bool VisibleValue { get; set; } = true;

		public float ProgressValue { get; set; }

		public int TimeLimitValue { get; set; }

		public bool ThrowOnDetailRead { get; set; }

		public int ProgressReadCount { get; private set; }

		public int TimeLimitReadCount { get; private set; }

		public override string Name => NameValue;

		public override string DisplayName => DisplayNameValue;

		public override string Description => DescriptionValue;

		public override string Hint => HintValue;

		public override MissionSourceBase Source => SourceValue;

		public override MissionType Type => TypeValue;

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

	private sealed class StubSource : MissionSourceBase
	{
		public StubSource(string name)
		{
			Name = name;
		}

		public override Texture2D Texture => null;

		public override string Name { get; }
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
	public void Create_MapsConfigurableWorldMetadata()
	{
		var source = new StubSource("world-source");
		var mission = new StubMission
		{
			DisplayNameValue = "Mapped world mission",
			DescriptionValue = "World mission description",
			SourceValue = source,
			TypeValue = MissionType.Legendary,
			VisibleValue = false,
		};

		MissionView view = WorldMissionViewAdapter.Create(mission);

		Assert.AreSame(source, view.Source);
		Assert.IsNull(view.SubSource);
		Assert.AreEqual(MissionType.Legendary, view.Type);
		Assert.AreEqual("Mapped world mission", view.DisplayName);
		Assert.AreEqual("World mission description", view.Description);
		Assert.AreEqual(string.Empty, view.Hint);
		Assert.IsFalse(view.Visible);
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
