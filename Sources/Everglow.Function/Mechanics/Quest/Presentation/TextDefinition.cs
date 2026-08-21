using System.Text;
using Everglow.Commons.Mechanics.Quest.Core;
using Everglow.Commons.Mechanics.Quest.Presentation.Views;

namespace Everglow.Commons.Mechanics.Quest.Presentation;

public static class TextDefinition
{
	public static string GetQuestTypeText(QuestType? type) => type?.ToString() ?? "All";

	public static string GetQuestNotificationText(QuestView quest, QuestNotification notification)
	{
		ArgumentNullException.ThrowIfNull(quest);

		return notification.Type switch
		{
			QuestNotificationType.Unlocked => $"[{quest.DisplayName}]任务已解锁",
			QuestNotificationType.Restored => $"[{quest.DisplayName}]任务已恢复",
			QuestNotificationType.Failed => $"[{quest.DisplayName}]任务已失败",
			QuestNotificationType.Completed => $"[{quest.DisplayName}]任务已完成",
			QuestNotificationType.Restarted => $"[{quest.DisplayName}]任务已重启",
			QuestNotificationType.ObjectiveCompleted => $"[{quest.DisplayName}]任务当前节点[{notification.Detail}]中目标已完成",
			_ => throw new ArgumentOutOfRangeException(nameof(notification)),
		};
	}

	public static string GetQuestStateText(QuestViewState? state) => state switch
	{
		QuestViewState.Active => "Accepted",
		null => "All",
		_ => state.ToString(),
	};

	public static string GetQuestDetailText(QuestView quest)
	{
		ArgumentNullException.ThrowIfNull(quest);

		var text = new StringBuilder();
		if (quest.TimeLimit.HasValue)
		{
			text.Append($"[TimerIconDrawer,QuestName='{quest.Identity.DefinitionId}'] 剩余时间:[TimerStringDrawer,QuestName='{quest.Identity.DefinitionId}']\n\n");
		}

		text.Append("描述：\n");
		text.Append(string.IsNullOrWhiteSpace(quest.Description) ? "无\n" : quest.Description + "\n");
		return text.ToString();
	}

	public static string GetQuestObjectivesText(QuestView quest)
	{
		ArgumentNullException.ThrowIfNull(quest);

		var text = new StringBuilder("目标：\n");
		int mainIndex = 1;
		foreach (ObjectiveNodeView node in quest.ObjectiveNodes)
		{
			int subIndex = 1;
			bool completed = IsCompleted(node);
			foreach (string objective in GetObjectiveLines(node))
			{
				string line = completed
					? $"[TextDrawer,Text='(已完成)',Color='100,100,100,255'] {objective}"
					: objective;
				text.Append($"{mainIndex}.{subIndex++} {line}");
				if (!line.EndsWith('\n'))
				{
					text.Append('\n');
				}
			}
			mainIndex++;
		}

		return text.ToString();
	}

	public static string GetQuestActionText(QuestPresentationEntry entry, string color)
	{
		if (entry is null)
		{
			return "[TextDrawer,Text='',Color='{color}']";
		}

		string text = entry.View.State switch
		{
			QuestViewState.Available => "接取",
			QuestViewState.Active when entry.Actions.Any(action => action.Type == QuestActionType.Submit) => "提交",
			QuestViewState.Active => "放弃",
			QuestViewState.Completed => "完成",
			QuestViewState.Overdue => "过期",
			QuestViewState.Failed => "失败",
			QuestViewState.Locked => "锁定",
			_ => "未知",
		};
		return GetColoredText(text, color);
	}

	public static string GetRemainingTimeText(int? remainingTime)
	{
		if (!remainingTime.HasValue)
		{
			return "Indefinitely";
		}

		var time = new TimeSpan(0, 0, remainingTime.Value / 60);
		return $"{(int)time.TotalMinutes}Min {time.Seconds}s";
	}

	public static string GetObjectiveTimerTooltip(float timer) => $"Time Remain: {(int)(timer / 60f)}s";

	public static string GetObjectiveDurationTooltip(float currentDuration, float maxDuration) => $"Duration: {(int)currentDuration}/{(int)maxDuration}";

	public static string GetQuestLevelTooltip(int stars) => $"Quest Level: {stars}";

	public static string GetColoredText(string text, string color) => $"[TextDrawer,Text='{text}',Color='{color}']";

	private static IEnumerable<string> GetObjectiveLines(ObjectiveNodeView node)
	{
		return node switch
		{
			LeafObjectiveNodeView leaf => [leaf.Objective.ObjectiveText],
			ParallelObjectiveNodeView parallel => parallel.Objectives.Select(objective => objective.ObjectiveText),
			AnyOfObjectiveNodeView anyOf => anyOf.Objectives.Select(objective => objective.ObjectiveText),
			BranchObjectiveNodeView branch => branch.Branches.SelectMany((branchView, branchIndex) =>
				branchView.Objectives.Select(objective =>
					$"[TextDrawer,Text='(Branch {branchIndex + 1})',Color='{GetBranchColor(branchView.State)}'] {objective.ObjectiveText}")),
			_ => [],
		};
	}

	private static bool IsCompleted(ObjectiveNodeView node)
	{
		return node switch
		{
			LeafObjectiveNodeView leaf => leaf.Objective.State == ObjectiveViewState.Completed,
			ParallelObjectiveNodeView parallel => parallel.Objectives.All(objective => objective.State == ObjectiveViewState.Completed),
			AnyOfObjectiveNodeView anyOf => anyOf.Objectives.Any(objective => objective.State == ObjectiveViewState.Completed),
			BranchObjectiveNodeView branch => branch.Branches.Any(branchView =>
				branchView.State == ObjectiveBranchState.Selected
				&& branchView.Objectives.All(objective => objective.State == ObjectiveViewState.Completed)),
			_ => false,
		};
	}

	private static string GetBranchColor(ObjectiveBranchState state) => state switch
	{
		ObjectiveBranchState.Candidate => "100,180,120,255",
		ObjectiveBranchState.Selected => "100,255,100,255",
		ObjectiveBranchState.Skipped => "100,100,100,255",
		_ => "100,100,100,255",
	};
}
