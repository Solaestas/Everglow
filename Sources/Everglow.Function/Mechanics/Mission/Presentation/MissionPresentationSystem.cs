using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.UI;
using Everglow.Commons.Mechanics.Mission.WorldSide;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public sealed class MissionPresentationSystem : ModSystem
{
	public MissionPresentationService Service { get; private set; }

	public bool NeedRefresh { get; set; }

	public override void PostSetupContent()
	{
		PlayerMissionSystem playerSystem = ModContent.GetInstance<PlayerMissionSystem>();
		WorldMissionSystem worldSystem = ModContent.GetInstance<WorldMissionSystem>();
		Service = new MissionPresentationService(
			playerSystem.Manager,
			playerSystem.Actions,
			worldSystem.Manager,
			worldSystem.Actions);

		if (!Main.dedServ)
		{
			playerSystem.Manager.Changed += () => NeedRefresh = true;
			worldSystem.Manager.Changed += () => NeedRefresh = true;
		}
	}

	public override void Unload()
	{
		if (!Main.dedServ)
		{
			MissionContainer.Instance.Unload();
		}

		NeedRefresh = false;
		Service = null;
	}
}
