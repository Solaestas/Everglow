using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.WorldSide;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public sealed class MissionPresentationSystem : ModSystem
{
	public MissionPresentationService Service { get; private set; }

	public override void PostSetupContent()
	{
		PlayerMissionSystem playerSystem = ModContent.GetInstance<PlayerMissionSystem>();
		WorldMissionSystem worldSystem = ModContent.GetInstance<WorldMissionSystem>();
		Service = new MissionPresentationService(
			playerSystem.Manager,
			playerSystem.Actions,
			worldSystem.Manager,
			worldSystem.Actions);
	}

	public override void Unload()
	{
		Service = null;
	}
}
