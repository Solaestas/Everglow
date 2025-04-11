using Everglow.Commons.Mechanics.MissionSystem.Primitives;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public class SystemMissionSource : MissionSourceBase
{
	internal SystemMissionSource()
	{
	}

	public override Texture2D Texture => ModAsset.Point.Value;

	public override string Name => "Everglow System";
}