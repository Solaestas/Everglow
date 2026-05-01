using Everglow.Commons.Mechanics.Mission.PlayerMission.Primitives;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Shared;

public sealed class SystemMissionSource : MissionSourceBase
{
	internal SystemMissionSource()
	{
	}

	public override Texture2D Texture => ModAsset.Point.Value;

	public override string Name => "Everglow System";
}