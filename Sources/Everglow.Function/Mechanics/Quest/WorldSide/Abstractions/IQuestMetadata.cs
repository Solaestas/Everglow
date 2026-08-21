using Everglow.Commons.Mechanics.Quest.Core;

namespace Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;

public interface IQuestMetadata
{
	/// <summary>
	/// Internal ID of the quest, used for serialization and identification. Should be unique across all quests.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Gets the display name associated with the object.
	/// </summary>
	public string DisplayName { get; }

	public string Description { get; }

	/// <summary>
	/// Gets the type of the quest associated with this instance.
	/// </summary>
	public QuestType Type { get; }

	public QuestSourceBase Source { get; }

	public List<Item> RewardItems { get; }

	public int TimeLimit { get; }

	public bool Visible { get; }

	public bool CanUnlock();

	/// <summary>
	/// Called for quest metadata initialization. Use this to set up objectives, rewards, etc.
	/// </summary>
	public void Initialize();
}
