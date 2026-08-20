using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public class MissionViewComparer : IComparer<MissionView>
{
	public static readonly MissionViewComparer Instance = new MissionViewComparer();

	public int Compare(MissionView x, MissionView y)
	{
		if (x.State != y.State)
		{
			return x.State.CompareTo(y.State);
		}
		else if (x.Type != y.Type)
		{
			if (x.Type is MissionType.None)
			{
				return 1;
			}

			if (y.Type is MissionType.None)
			{
				return -1;
			}

			return x.Type.CompareTo(y.Type);
		}
		else
		{
			return string.Compare(x.DisplayName, y.DisplayName);
		}
	}
}
