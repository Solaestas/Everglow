using Everglow.Commons.Mechanics.Quest.Core;
using Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.Quest.WorldSide;

public sealed class WorldQuestActions
{
	private readonly WorldQuestManager _manager;

	public WorldQuestActions(WorldQuestManager manager)
	{
		_manager = manager ?? throw new ArgumentNullException(nameof(manager));
	}

	public static IReadOnlyList<QuestActionType> GetAvailableTypes(WorldQuestBase quest)
	{
		ArgumentNullException.ThrowIfNull(quest);

		if (!NetUtils.IsSingle)
		{
			return [];
		}

		if (quest.State == WorldQuestState.Failed && quest.Retriable)
		{
			return [QuestActionType.Retry];
		}

		if (quest.State == WorldQuestState.Completed && !quest.RewardClaimed)
		{
			return [QuestActionType.ClaimReward];
		}

		return [];
	}

	public bool TryExecute(QuestAction action)
	{
		QuestIdentity identity = action.Quest;
		if (identity.Side != QuestSide.World
			|| !string.Equals(identity.DefinitionId, identity.InstanceId, StringComparison.Ordinal))
		{
			return false;
		}

		var quest = _manager.GetQuest(identity.DefinitionId);
		if (quest is null
			|| !string.Equals(quest.Name, identity.InstanceId, StringComparison.Ordinal)
			|| QuestHintRules.HasContent(quest.Hint)
			|| !GetAvailableTypes(quest).Contains(action.Type))
		{
			return false;
		}

		bool applied = action.Type switch
		{
			QuestActionType.Retry => quest.RetryCore(),
			QuestActionType.ClaimReward => TryClaimReward(quest),
			_ => false,
		};

		if (!applied)
		{
			return false;
		}

		if (action.Type == QuestActionType.Retry)
		{
			_manager.OnQuestObjectiveUpdated(quest);
		}
		_manager.OnQuestStatusUpdated(quest);
		return true;
	}

	private static bool TryClaimReward(WorldQuestBase quest)
	{
		quest.GiveRewards();
		return quest.RewardClaimed;
	}
}
