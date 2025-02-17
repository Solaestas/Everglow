using System.Text;
using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

public interface IGiveItemMission : IMissionObjective
{
	public abstract List<GiveItemRequirement> DemandGiveItems { get; init; }

	/// <summary>
	/// Calculate mission progress
	/// </summary>
	/// <param name="inventory"></param>
	/// <returns></returns>
	public float CalculateProgress(IEnumerable<Item> inventory)
	{
		if (DemandGiveItems.Count == 0 || DemandGiveItems.Select(x => x.Requirement).Sum() == 0)
		{
			return 1f;
		}

		return DemandGiveItems.Select(x => x.Progress(inventory)).Average();
	}

	public void ConsumeItem(IEnumerable<Item> inventory)
	{
		foreach (var item in DemandGiveItems)
		{
			var stack = item.Requirement;

			foreach (var inventoryItem in inventory.Where(x => item.Items.Contains(x.type)))
			{
				if (inventoryItem.stack < stack)
				{
					stack -= inventoryItem.stack;
					inventoryItem.stack = 0;
				}
				else
				{
					inventoryItem.stack -= stack;
					break;
				}
			}
		}
	}

	public IEnumerable<string> GetObjectivesString(IEnumerable<Item> inventory)
	{
		var objectives = new List<string>();

		foreach (var demand in DemandGiveItems)
		{
			var progress = $"({inventory.Where(i => demand.Items.Contains(i.type)).Sum(i => i.stack)}/{demand.Requirement})";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				objectives.Add($"提交{itemString}合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				objectives.Add($"提交{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}个 {progress}\n");
			}
		}

		return objectives;
	}
}