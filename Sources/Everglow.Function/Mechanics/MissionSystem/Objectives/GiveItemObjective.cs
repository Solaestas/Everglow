using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class GiveItemObjective : MissionObjectiveBase
{
	public List<GiveItemRequirement> DemandGiveItems { get; } = [];

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaItemTextures(DemandGiveItems.SelectMany(x => x.Items));
	}

	public override float Progress => DemandGiveItems.Count != 0 && DemandGiveItems.All(x => x.Requirement != 0)
		? DemandGiveItems.Average(x => x.Progress(Main.LocalPlayer.inventory))
		: 1f;

	public override bool CheckCompletion() => DemandGiveItems.Average(x => x.Progress(Main.LocalPlayer.inventory)) >= 1f;

	/// <summary>
	/// Remove required items from player inventory.
	/// </summary>
	/// <param name="inventory"></param>
	public void RemoveItem(IEnumerable<Item> inventory)
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

	public override void Complete()
	{
		// Make sure the items can only be removed once.
		if (!Completed)
		{
			RemoveItem(Main.LocalPlayer.inventory);
		}

		base.Complete();
	}

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var demand in DemandGiveItems)
		{
			var progress = $"({Main.LocalPlayer.inventory.Where(i => demand.Items.Contains(i.type)).Sum(i => i.stack)}/{demand.Requirement})";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				lines.Add($"提交{itemString}合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				lines.Add($"提交{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}个 {progress}\n");
			}
		}
	}
}