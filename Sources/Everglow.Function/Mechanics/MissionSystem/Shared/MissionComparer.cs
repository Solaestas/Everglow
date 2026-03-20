using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public class MissionComparer : IComparer<MissionBase>
{
	public static readonly MissionComparer Instance = new MissionComparer();

	public int Compare(MissionBase x, MissionBase y)
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