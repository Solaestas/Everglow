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
public partial class PlayerMissionViewAdapterTest
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

		public int TimeLimitValue { get; set; } = -1;

		public override string Name => NameValue;

		public override string DisplayName => DisplayNameValue;

		public override string Description => DescriptionValue;

		public override string Hint => HintValue;

		public override MissionSourceBase Source => SourceValue!;

		public override MissionSourceBase SubSource => SubSourceValue!;

		public override MissionType Type => TypeValue;

		public override float Progress => ProgressValue;

		public override int TimeLimit => TimeLimitValue;
	}

	private sealed class StubObjective : PlayerObjectiveBase
	{
		public StubObjective(params string[] lines)
		{
			ObjectiveTextValue = string.Join('\n', lines);
		}

		public string DescriptionValue { get; set; } = string.Empty;

		public string ObjectiveTextValue { get; set; } = string.Empty;

		public bool Ready { get; set; }

		public float ProgressValue { get; set; }

		public MissionIconBase? Icon { get; set; }

		public bool ThrowOnTextRead { get; set; }

		public override string Description => DescriptionValue;

		public override float Progress => ProgressValue;

		public override bool CheckCompletion() => Ready;

		public override void GetObjectivesIcon(MissionIconGroup iconGroup)
		{
			if (Icon is not null)
			{
				iconGroup.Add(Icon);
			}
		}

		public override string GetObjectiveText()
		{
			if (ThrowOnTextRead)
			{
				throw new InvalidOperationException("Hidden objective text must not be read.");
			}

			return ObjectiveTextValue;
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
		const string description = "[TextDrawer,Text='mission body',Color='1,2,3,255']";
		const string hint = "[TextDrawer,Text='mission hint',Color='4,5,6,255']";
		var source = new StubSource("source");
		var subSource = new StubSource("sub-source");
		var mission = new StubMission
		{
			NameValue = "mission-definition",
			DisplayNameValue = "Mission title",
			DescriptionValue = description,
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
		Assert.AreEqual(description, view.Description);
		Assert.AreEqual(string.Empty, view.Hint);
		Assert.IsFalse(view.Visible);

		mission.HintValue = hint;
		MissionView hintedView = PlayerMissionViewAdapter.Create(mission);

		Assert.AreEqual(hint, hintedView.Hint);

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
	public void Create_NonWhitespaceHintHidesDetailsButKeepsVisibilityAndIcons(string hint)
	{
		var visibleIcon = new StubIcon();
		var objective = new StubObjective("secret objective")
		{
			Icon = visibleIcon,
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
		Assert.AreEqual(0, view.ElapsedTime);
		Assert.IsNull(view.TimeLimit);
		Assert.IsNull(view.RemainingTime);
		Assert.HasCount(2, view.Icons);
		Assert.IsInstanceOfType<MissionSourceIcon>(view.Icons[0]);
		Assert.AreSame(visibleIcon, view.Icons[1]);
	}

	[TestMethod]
	[DataRow(" ")]
	[DataRow("\t")]
	public void Create_WhitespaceHintDoesNotHideDetails(string hint)
	{
		var mission = new StubMission
		{
			HintValue = hint,
			DescriptionValue = "visible description",
			ProgressValue = 0.75f,
		};

		MissionView view = PlayerMissionViewAdapter.Create(mission);

		Assert.AreEqual(hint, view.Hint);
		Assert.AreEqual("visible description", view.Description);
		Assert.AreEqual(0.75f, view.Progress);
	}

}
