using Everglow.Commons.Mechanics.Mission.PlayerSide.Primitives;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Shared;

public sealed class SystemMissionSource : MissionSourceBase
{
	internal SystemMissionSource()
	{
	}

	public override Texture2D Texture => ModAsset.Point.Value;

	public override string Name => "Everglow System";
}