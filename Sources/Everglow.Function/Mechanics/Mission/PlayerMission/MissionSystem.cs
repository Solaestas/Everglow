using Everglow.Commons.Mechanics.Mission.PlayerMission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerMission.UI;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission;

public class MissionSystem : ModSystem
{
	public override void Load()
	{
		MissionManager.Load();
	}

	public override void Unload()
	{
		MissionManager.UnLoad();
		MissionContainer.Instance?.Unload();
	}
}