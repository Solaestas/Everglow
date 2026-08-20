using System.Text;
using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public static class TextDefinition
{
	public static string GetMissionTypeText(MissionType? type) => type?.ToString() ?? "All";

	public static string GetMissionNotificationText(MissionView mission, MissionNotification notification)
	{
		ArgumentNullException.ThrowIfNull(mission);

		return notification.Type switch
		{
			MissionNotificationType.Unlocked => $"[{mission.DisplayName}]任务已解锁",
			MissionNotificationType.Restored => $"[{mission.DisplayName}]任务已恢复",
			MissionNotificationType.Failed => $"[{mission.DisplayName}]任务已失败",
			MissionNotificationType.Completed => $"[{mission.DisplayName}]任务已完成",
			MissionNotificationType.Restarted => $"[{mission.DisplayName}]任务已重启",
			MissionNotificationType.ObjectiveCompleted => $"[{mission.DisplayName}]任务当前节点[{notification.Detail}]中目标已完成",
			_ => throw new ArgumentOutOfRangeException(nameof(notification)),
		};
	}

	public static string GetMissionStateText(MissionViewState? state) => state switch
	{
		MissionViewState.Active => "Accepted",
		null => "All",
		_ => state.ToString(),
	};

	public static string GetMissionDetailText(MissionView mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		var text = new StringBuilder();
		if (mission.TimeLimit.HasValue)
		{
			text.Append($"[TimerIconDrawer,MissionName='{mission.Identity.DefinitionId}'] 剩余时间:[TimerStringDrawer,MissionName='{mission.Identity.DefinitionId}']\n\n");
		}

		text.Append("描述：\n");
		text.Append(string.IsNullOrWhiteSpace(mission.Description) ? "无\n" : mission.Description + "\n");
		return text.ToString();
	}

	public static string GetMissionObjectivesText(MissionView mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		var text = new StringBuilder("目标：\n");
		int mainIndex = 1;
		foreach (ObjectiveNodeView node in mission.ObjectiveNodes)
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

	public static string GetMissionActionText(MissionPresentationEntry entry, string color)
	{
		if (entry is null)
		{
			return "[TextDrawer,Text='',Color='{color}']";
		}

		string text = entry.View.State switch
		{
			MissionViewState.Available => "接取",
			MissionViewState.Active when entry.Actions.Any(action => action.Type == MissionActionType.Submit) => "提交",
			MissionViewState.Active => "放弃",
			MissionViewState.Completed => "完成",
			MissionViewState.Overdue => "过期",
			MissionViewState.Failed => "失败",
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

	public static string GetMissionLevelTooltip(int stars) => $"Mission Level: {stars}";

	public static string GetColoredText(string text, string color) => $"[TextDrawer,Text='{text}',Color='{color}']";

	private static IEnumerable<string> GetObjectiveLines(ObjectiveNodeView node)
	{
		return node switch
		{
			LeafObjectiveNodeView leaf => [leaf.Objective.Description],
			ParallelObjectiveNodeView parallel => parallel.Objectives.Select(objective => objective.Description),
			AnyOfObjectiveNodeView anyOf => anyOf.Objectives.Select(objective => objective.Description),
			BranchObjectiveNodeView branch => branch.Branches.SelectMany((branchView, branchIndex) =>
				branchView.Objectives.Select(objective =>
					$"[TextDrawer,Text='(Branch {branchIndex + 1})',Color='{GetBranchColor(branchView.State)}'] {objective.Description}")),
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
