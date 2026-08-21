using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;

public abstract partial class WorldQuestBase : IQuestPersistence
{
	private const string StateKey = nameof(State);
	private const string TimeKey = nameof(Time);
	private const string RewardKey = nameof(RewardClaimed);
	private const string RewardPlayerKey = nameof(RewardClaimedPlayers);
	private const string ObjectivesSaveKey = nameof(Objectives);

	public void LoadData(TagCompound tag)
	{
		WorldQuestState oldState = State;
		if (tag.TryGet<int>(StateKey, out var ms))
		{
			State = (WorldQuestState)ms;
		}

		if (tag.TryGet<int>(TimeKey, out var mt))
		{
			Time = mt;
		}

		if (tag.TryGet<bool>(RewardKey, out var rc))
		{
			RewardClaimed = rc;
		}

		if (tag.TryGet<IList<string>>(RewardPlayerKey, out var rp))
		{
			RewardClaimedPlayers = rp.ToHashSet();
		}

		if (tag.TryGet<TagCompound>(ObjectivesSaveKey, out var o))
		{
			Objectives.LoadData(o);
		}
		if (!RecoverInvalidObjectiveState())
		{
			ApplyObjectiveSnapshot(oldState, State);
		}
	}

	public void SaveData(TagCompound tag)
	{
		tag.Add(StateKey, (int)State);
		tag.Add(TimeKey, Time);
		tag.Add(RewardKey, RewardClaimed);
		tag.Add(RewardPlayerKey, RewardClaimedPlayers.ToList());

		var o = new TagCompound();
		Objectives.SaveData(o);
		tag.Add(ObjectivesSaveKey, o);
	}

	private bool RecoverInvalidObjectiveState()
	{
		if (!Objectives.RecoveredInvalidState)
		{
			return false;
		}

		Time = 0;
		RewardClaimed = false;
		RewardClaimedPlayers.Clear();
		if (State == WorldQuestState.Completed)
		{
			State = WorldQuestState.Active;
		}

		ResetProgress();
		if (State == WorldQuestState.Active)
		{
			Activate();
		}
		return true;
	}
}
