using Everglow.Commons.Mechanics.Quest.WorldSide.Structure;

namespace Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;

public interface IQuestBehavior
{
	public int WhoAmI { get; }

	public WorldQuestState State { get; }

	public float Progress { get; }

	public WorldObjectiveContainer Objectives { get; }

	public int Time { get; }

	public bool Retriable { get; }

	/// <summary>
	/// A flag indicating whether the quest reward has been claimed.
	/// <br/>This is used to prevent players from claiming rewards multiple times by resetting the quest.
	/// <para/> TODO: Use player id list instead of a single flag to support multiplayer.
	/// </summary>
	public bool RewardClaimed { get; }

	public void Unlock();

	public void CompleteQuest();

	public void Update();

	public void Retry();

	public void GiveRewards();

	// public void Activate();

	// public void Deactivate();

	public void Reset();

	public void ResetProgress();
}
