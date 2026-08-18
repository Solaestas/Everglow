using Everglow.Commons.Mechanics.Mission.UI;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

public class PlayerMissionSystem : ModSystem
{
	public PlayerMissionManager Manager { get; private set; }

	public PlayerMissionActions Actions { get; private set; }

	public override void Load()
	{
		Manager = new PlayerMissionManager();
		Actions = new PlayerMissionActions(Manager);
		Manager.Load();
	}

	public override void Unload()
	{
		Manager?.Unload();
		Manager = null;
		Actions = null;
		MissionContainer.Instance?.Unload();
	}
}
