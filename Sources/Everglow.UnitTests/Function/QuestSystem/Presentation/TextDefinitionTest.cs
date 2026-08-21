using Everglow.Commons.Mechanics.Quest.Core;
using Everglow.Commons.Mechanics.Quest.Presentation;
using Everglow.Commons.Mechanics.Quest.Presentation.Views;
using Microsoft.Xna.Framework;

namespace Everglow.UnitTests.Function.QuestSystem;

[TestClass]
public class TextDefinitionTest
{
	[TestMethod]
	[DataRow(null, "All")]
	[DataRow(QuestViewState.Active, "Accepted")]
	[DataRow(QuestViewState.Completed, "Completed")]
	public void GetQuestStateText_ReturnsPresentationLabel(QuestViewState? state, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetQuestStateText(state));
	}

	[TestMethod]
	[DataRow(null, "All")]
	[DataRow(QuestType.MainStory, "MainStory")]
	public void GetQuestTypeText_ReturnsPresentationLabel(QuestType? type, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetQuestTypeText(type));
	}

	[TestMethod]
	public void GetQuestDetailText_FormatsTimerAndDescription()
	{
		const string description = "[TextDrawer,Text='Description',Color='1,2,3,255']";
		var quest = new QuestView
		{
			Identity = new QuestIdentity(QuestSide.Player, "TestQuest", "TestQuest"),
			Description = description,
			TimeLimit = 60,
		};

		Assert.AreEqual(
			$"[TimerIconDrawer,QuestName='TestQuest'] 剩余时间:[TimerStringDrawer,QuestName='TestQuest']\n\n描述：\n{description}\n",
			TextDefinition.GetQuestDetailText(quest));
	}

	[TestMethod]
	public void GetQuestObjectivesText_FormatsCompletedAndBranchObjectives()
	{
		var quest = new QuestView
		{
			ObjectiveNodes =
			[
				new LeafObjectiveNodeView(new ObjectiveView
				{
					Description = "must not render",
					ObjectiveText = "First",
					State = ObjectiveViewState.Completed,
				}),
				new BranchObjectiveNodeView(
				[
					new ObjectiveBranchView(
						ObjectiveBranchState.Candidate,
						[new ObjectiveView
						{
							Description = "must not render",
							ObjectiveText = "Second",
						}]),
				]),
			],
		};

		string text = TextDefinition.GetQuestObjectivesText(quest);

		Assert.AreEqual(
			"目标：\n1.1 [TextDrawer,Text='(已完成)',Color='100,100,100,255'] First\n2.1 [TextDrawer,Text='(Branch 1)',Color='100,180,120,255'] Second\n",
			text);
		Assert.DoesNotContain("must not render", text);
	}

	[TestMethod]
	public void GetQuestActionText_UsesAvailableSubmitAction()
	{
		var identity = new QuestIdentity(QuestSide.Player, "TestQuest", "TestQuest");
		var entry = new QuestPresentationEntry(
			new QuestView { Identity = identity, State = QuestViewState.Active },
			[new QuestAction(identity, QuestActionType.Submit)]);

		Assert.AreEqual(
			"[TextDrawer,Text='提交',Color='45,38,33']",
			TextDefinition.GetQuestActionText(entry, "45,38,33"));
	}

	[TestMethod]
	public void GetQuestActionText_UsesLockedStateLabel()
	{
		var identity = new QuestIdentity(QuestSide.World, "TestQuest", "TestQuest");
		var entry = new QuestPresentationEntry(
			new QuestView { Identity = identity, State = QuestViewState.Locked },
			[]);

		Assert.AreEqual(
			"[TextDrawer,Text='锁定',Color='45,38,33']",
			TextDefinition.GetQuestActionText(entry, "45,38,33"));
	}

	[TestMethod]
	[DataRow(null, "Indefinitely")]
	[DataRow(3720, "1Min 2s")]
	public void GetRemainingTimeText_FormatsTicks(int? remainingTime, string expected)
	{
		Assert.AreEqual(expected, TextDefinition.GetRemainingTimeText(remainingTime));
	}

	[TestMethod]
	[DataRow(QuestNotificationType.Unlocked, null, "[World Quest]任务已解锁", 150, 150, 250)]
	[DataRow(QuestNotificationType.Restored, null, "[World Quest]任务已恢复", 150, 150, 250)]
	[DataRow(QuestNotificationType.Failed, null, "[World Quest]任务已失败", 250, 150, 150)]
	[DataRow(QuestNotificationType.Completed, null, "[World Quest]任务已完成", 150, 250, 150)]
	[DataRow(QuestNotificationType.Restarted, null, "[World Quest]任务已重启", 150, 250, 150)]
	[DataRow(QuestNotificationType.ObjectiveCompleted, "Node", "[World Quest]任务当前节点[Node]中目标已完成", 250, 250, 150)]
	public void QuestNotificationDefinitions_ReturnPresentationValues(
		QuestNotificationType type,
		string detail,
		string expectedText,
		int red,
		int green,
		int blue)
	{
		var quest = new QuestView { DisplayName = "World Quest" };
		var notification = new QuestNotification(
			new QuestIdentity(QuestSide.World, "TestQuest", "TestQuest"),
			type,
			detail);

		Assert.AreEqual(expectedText, TextDefinition.GetQuestNotificationText(quest, notification));
		Assert.AreEqual(new Color(red, green, blue), ColorDefinition.GetQuestNotificationColor(type));
	}
}
