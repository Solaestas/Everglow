using Everglow.Commons.Mechanics.Mission.WorldMission.Base;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;

public class WorldCollectItemObjective : WorldObjectiveBase
{
	public WorldCollectItemObjective()
	{
	}

	public WorldCollectItemObjective(int itemType, int itemCount)
	{
		ItemType = itemType;
		ItemCount = itemCount;
	}

	private float progress;

	public int ItemType { get; private set; }

	public int ItemCount { get; private set; }

	public override float Progress => progress;

	public override bool CheckCompletion() => progress >= 1f;

	public override void Update()
	{
		int total = 0;
		foreach (var player in Main.ActivePlayers)
		{
			int remainRequired = ItemCount - total;
			if (remainRequired <= 0)
			{
				break;
			}

			total += player.CountItem(ItemType, remainRequired);
		}

		total = Math.Clamp(total, 0, ItemCount);
		progress = (float)total / ItemCount;
	}

	public override void GetObjectivesText(List<string> lines) => throw new NotImplementedException();
}