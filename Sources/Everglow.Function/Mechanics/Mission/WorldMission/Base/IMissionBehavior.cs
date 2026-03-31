namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public interface IMissionBehavior
{
	public int WhoAmI { get; }

	public WorldMissionState State { get; }

	public float Progress { get; }

	public MissionObjectiveContainer_New Objectives { get; }

	public ObjectiveBase CurrentObjective { get; }

	public int Time { get; }

	public bool Retriable { get; }

	/// <summary>
	/// A flag indicating whether the mission reward has been claimed.
	/// <br/>This is used to prevent players from claiming rewards multiple times by resetting the mission.
	/// <para/> TODO: Use player id list instead of a single flag to support multiplayer.
	/// </summary>
	public bool RewardClaimed { get; }

	public void Unlock();

	public bool CheckComplete();

	public void Update();

	public void Retry();

	public void GiveRewards();

	public void Activate();

	public void Deactivate();

	public void Reset();

	public void ResetProgress();

	public void OnUnlock();

	public void OnComplete();

	public void OnExpire();

	public void OnReset();
}