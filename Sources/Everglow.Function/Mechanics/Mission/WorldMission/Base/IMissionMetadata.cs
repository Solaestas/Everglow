namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public interface IMissionMetadata
{
	public string Name { get; }

	public string DisplayName { get; }

	public string Description { get; }

	public object MissionType { get; }

	public object MissionSource { get; }

	public object MissionIcons { get; }

	public List<Item> RewardItems { get; }

	public int TimeLimit { get; }

	public bool Visible { get; }

	public bool CanUnlock();

	/// <summary>
	/// Called for mission metadata initialization. Use this to set up objectives, rewards, etc.
	/// </summary>
	public void Initialize();
}