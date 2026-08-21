using Everglow.Commons.Mechanics.Quest.Core;
using Everglow.Commons.Mechanics.Quest.Presentation.Views;

namespace Everglow.Commons.Mechanics.Quest.Presentation;

public class QuestViewComparer : IComparer<QuestView>
{
	public static readonly QuestViewComparer Instance = new QuestViewComparer();

	public int Compare(QuestView x, QuestView y)
	{
		if (x.State != y.State)
		{
			return x.State.CompareTo(y.State);
		}
		else if (x.Type != y.Type)
		{
			if (x.Type is QuestType.None)
			{
				return 1;
			}

			if (y.Type is QuestType.None)
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
