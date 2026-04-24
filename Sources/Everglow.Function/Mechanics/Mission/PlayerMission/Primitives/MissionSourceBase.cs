using Everglow.Commons.Mechanics.Mission.PlayerMission.Shared;
using Terraria.DataStructures;

namespace Everglow.Commons.Mechanics.Mission.PlayerMission.Primitives;

public abstract class MissionSourceBase
{
	public static readonly MissionSourceBase Default = new SystemMissionSource();

	public abstract Texture2D Texture { get; }

	public abstract string Name { get; }

	public virtual DrawAnimation Animation => null;

	public override bool Equals(object obj) => obj.GetType() == GetType();

	public override int GetHashCode() => GetType().GetHashCode();
}