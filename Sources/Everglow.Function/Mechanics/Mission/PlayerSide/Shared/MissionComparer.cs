using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;

namespace Everglow.Commons.Mechanics.Mission.PlayerSide.Shared;

public class MissionComparer : IComparer<PlayerMissionBase>
{
	public static readonly MissionComparer Instance = new MissionComparer();

	public int Compare(PlayerMissionBase x, PlayerMissionBase y)
	{
		if (x.PoolType != y.PoolType)
		{
			return x.PoolType - y.PoolType;
		}
		else if (x.MissionType != y.MissionType)
		{
			if (x.MissionType is MissionType.None)
			{
				return 1;
			}

			if (y.MissionType is MissionType.None)
			{
				return -1;
			}

			return x.MissionType - y.MissionType;
		}
		else
		{
			return string.Compare(x.DisplayName, y.DisplayName);
		}
	}
}