using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class GiveItemObjective : MissionObjectiveBase
{
	public GiveItemObjective()
	{
	}

	public GiveItemObjective(GiveItemRequirement requirement)
	{
		DemandGiveItem = requirement;
	}

	public GiveItemRequirement DemandGiveItem { get; set; }

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaItemTextures(DemandGiveItem.Items);
	}

	public override float Progress => DemandGiveItem.Progress(Main.LocalPlayer.inventory);

	public override bool CheckCompletion() => DemandGiveItem.Progress(Main.LocalPlayer.inventory) >= 1f;

	/// <summary>
	/// Remove required items from player inventory.
	/// </summary>
	/// <param name="inventory"></param>
	public void RemoveItem(IEnumerable<Item> inventory)
	{
		var stackCount = DemandGiveItem.Requirement;
		foreach (var inventoryItem in inventory.Where(x => DemandGiveItem.Items.Contains(x.type)))
		{
			if (inventoryItem.stack < stackCount)
			{
				stackCount -= inventoryItem.stack;
				inventoryItem.stack = 0;
			}
			else
			{
				inventoryItem.stack -= stackCount;
				break;
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
		var progress = $"({Main.LocalPlayer.inventory.Where(i => DemandGiveItem.Items.Contains(i.type)).Sum(i => i.stack)}/{DemandGiveItem.Requirement})";
		if (DemandGiveItem.Items.Count > 1)
		{
			var itemString = string.Join(' ', DemandGiveItem.Items.ConvertAll(i => ItemDrawer.Create(i)));
			lines.Add($"提交{itemString}合计{DemandGiveItem.Requirement}个 {progress}\n");
		}
		else
		{
			lines.Add($"提交{ItemDrawer.Create(DemandGiveItem.Items.First())}{DemandGiveItem.Requirement}个 {progress}\n");
		}
	}
}