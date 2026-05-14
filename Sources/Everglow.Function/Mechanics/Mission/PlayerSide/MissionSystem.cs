using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.UI;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide;

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