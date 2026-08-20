using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Microsoft.Xna.Framework;

namespace Everglow.UnitTests.Function.MissionSystem;

[TestClass]
public class TextDefinitionTest
{
	[TestMethod]
	[DataRow(null, "All")]
	[DataRow(MissionViewState.Active, "Accepted")]
	[DataRow(MissionViewState.Completed, "Completed")]
	public void GetMissionStateText_ReturnsPresentationLabel(MissionViewState? state, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetMissionStateText(state));
	}

	[TestMethod]
	[DataRow(null, "All")]
	[DataRow(MissionType.MainStory, "MainStory")]
	public void GetMissionTypeText_ReturnsPresentationLabel(MissionType? type, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetMissionTypeText(type));
	}

	[TestMethod]
	public void GetMissionDetailText_FormatsTimerAndDescription()
	{
		var mission = new MissionView
		{
			Identity = new MissionIdentity(MissionSide.Player, "TestMission", "TestMission"),
			Description = "Description",
			TimeLimit = 60,
		};

		Assert.AreEqual(
			"[TimerIconDrawer,MissionName='TestMission'] 剩余时间:[TimerStringDrawer,MissionName='TestMission']\n\n描述：\nDescription\n",
			TextDefinition.GetMissionDetailText(mission));
	}

	[TestMethod]
	public void GetMissionObjectivesText_FormatsCompletedAndBranchObjectives()
	{
		var mission = new MissionView
		{
			ObjectiveNodes =
			[
				new LeafObjectiveNodeView(new ObjectiveView
				{
					Description = "First",
					State = ObjectiveViewState.Completed,
				}),
				new BranchObjectiveNodeView(
				[
					new ObjectiveBranchView(
						ObjectiveBranchState.Candidate,
						[new ObjectiveView { Description = "Second" }]),
				]),
			],
		};

		Assert.AreEqual(
			"目标：\n1.1 [TextDrawer,Text='(已完成)',Color='100,100,100,255'] First\n2.1 [TextDrawer,Text='(Branch 1)',Color='100,180,120,255'] Second\n",
			TextDefinition.GetMissionObjectivesText(mission));
	}

	[TestMethod]
	public void GetMissionActionText_UsesAvailableSubmitAction()
	{
		var identity = new MissionIdentity(MissionSide.Player, "TestMission", "TestMission");
		var entry = new MissionPresentationEntry(
			new MissionView { Identity = identity, State = MissionViewState.Active },
			[new MissionAction(identity, MissionActionType.Submit)]);

		Assert.AreEqual(
			"[TextDrawer,Text='提交',Color='45,38,33']",
			TextDefinition.GetMissionActionText(entry, "45,38,33"));
	}

	[TestMethod]
	public void GetMissionActionText_UsesLockedStateLabel()
	{
		var identity = new MissionIdentity(MissionSide.World, "TestMission", "TestMission");
		var entry = new MissionPresentationEntry(
			new MissionView { Identity = identity, State = MissionViewState.Locked },
			[]);

		Assert.AreEqual(
			"[TextDrawer,Text='锁定',Color='45,38,33']",
			TextDefinition.GetMissionActionText(entry, "45,38,33"));
	}

	[TestMethod]
	[DataRow(null, "Indefinitely")]
	[DataRow(3720, "1Min 2s")]
	public void GetRemainingTimeText_FormatsTicks(int? remainingTime, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetRemainingTimeText(remainingTime));
	}

	[TestMethod]
	[DataRow(MissionNotificationType.Unlocked, null, "[World Mission]任务已解锁", 150, 150, 250)]
	[DataRow(MissionNotificationType.Restored, null, "[World Mission]任务已恢复", 150, 150, 250)]
	[DataRow(MissionNotificationType.Failed, null, "[World Mission]任务已失败", 250, 150, 150)]
	[DataRow(MissionNotificationType.Completed, null, "[World Mission]任务已完成", 150, 250, 150)]
	[DataRow(MissionNotificationType.Restarted, null, "[World Mission]任务已重启", 150, 250, 150)]
	[DataRow(MissionNotificationType.ObjectiveCompleted, "Node", "[World Mission]任务当前节点[Node]中目标已完成", 250, 250, 150)]
	public void MissionNotificationDefinitions_ReturnPresentationValues(
		MissionNotificationType type,
		string detail,
		string expectedText,
		int red,
		int green,
		int blue)
	{
		var mission = new MissionView { DisplayName = "World Mission" };
		var notification = new MissionNotification(
			new MissionIdentity(MissionSide.World, "TestMission", "TestMission"),
			type,
			detail);

		Assert.AreEqual(expectedText, TextDefinition.GetMissionNotificationText(mission, notification));
		Assert.AreEqual(new Color(red, green, blue), ColorDefinition.GetMissionNotificationColor(type));
	}
}
