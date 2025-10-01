using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.UI;

namespace Everglow.Commons.Mechanics.MissionSystem;

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