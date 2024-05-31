namespace Everglow.Commons.QuestSystem;

public class Quest
{
	/// <summary>
	/// Should this Quest be stored in the Quest list or not.
	/// </summary>
	public bool Active { get; private set; }

	/// <summary>
	/// Is this Quest finished or not.
	/// </summary>
	public bool IsFinished { get; set; }

	/// <summary>
	/// Where is this Quest come from.
	/// </summary>
	public object QuestSource { get; set; }

	/// <summary>
	/// The number of completed sub-Quests.
	/// </summary>
	public int CompletedIndex { get; set; }

	/// <summary>
	/// The number of total sub-Quests.
	/// </summary>
	public int TotalIndex { get; set; }

	/// <summary>
	/// The difficulty of this Quest. Usually within a range of 1 to 100.
	/// </summary>
	public int Difficulty { get; set; }

	/// <summary>
	/// Should this Quest be displayed or not.
	/// </summary>
	public bool Hide { get; set; }

	/// <summary>
	/// Returns the completion progress as a value between 0 and 1.
	/// If the duration is within [0, 1], a duration bar will be displayed.
	/// </summary>
	/// <returns>Completion progress.</returns>
	public virtual float GetCompletionProgress()
	{
		return (float)CompletedIndex / TotalIndex;
	}

	/// <summary>
	/// Override this method to modify Quest rewards.
	/// </summary>
	public virtual void SpawnRewards()
	{
		// Code to modify Quest rewards goes here
	}

	/// <summary>
	/// Call this method when the Quest is finished.
	/// </summary>
	public virtual void Settle()
	{
		SpawnRewards();
		IsFinished = true;
		Deactivate();
	}

	/// <summary>
	/// Deactivates the Quest.
	/// </summary>
	public void Deactivate()
	{
		Active = false;
	}
}