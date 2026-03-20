using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class ConsumeItemObjective : MissionObjectiveBase
{
	public ConsumeItemObjective()
	{
	}

	public ConsumeItemObjective(ItemRequirement requirement)
	{
		DemandConsumeItem = requirement;
	}

	public ItemRequirement DemandConsumeItem { get; set; }

	public ProgressCounter Counter { get; private set; } = new();

	public override float Progress => DemandConsumeItem.GetCounterProgress(Counter);

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaItemTextures(DemandConsumeItem.Items);
	}

	public override bool CheckCompletion() => Counter.Value >= DemandConsumeItem.Requirement;

	public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	{
		foreach (var item in DemandConsumeItem.Items)
		{
			iconGroup.Add(ItemMissionIcon.Create(item, new Item(item).Name));
		}
	}

	public override void GetObjectivesText(List<string> lines)
	{
		var progress = $"({Counter.Value}/{DemandConsumeItem.Requirement})";
		if (DemandConsumeItem.Items.Count > 1)
		{
			var itemString = string.Join(' ', DemandConsumeItem.Items.ConvertAll(i => ItemDrawer.Create(i)));
			lines.Add($"消耗{itemString}合计{DemandConsumeItem.Requirement}个 {progress}\n");
		}
		else
		{
			lines.Add($"消耗{ItemDrawer.Create(DemandConsumeItem.Items.First())}{DemandConsumeItem.Requirement}个 {progress}\n");
		}
	}

	public override void Activate(MissionBase sourceMission)
	{
		MissionGlobalItem.OnConsumeItemEvent += MissionGlobalItem_OnConsumeItem;
	}

	public override void Deactivate()
	{
		MissionGlobalItem.OnConsumeItemEvent -= MissionGlobalItem_OnConsumeItem;
	}

	private void MissionGlobalItem_OnConsumeItem(Item item)
	{
		if (DemandConsumeItem.Items.Contains(item.type))
		{
			Counter.Count();
		}
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		if (tag.TryGet<ProgressCounter>(nameof(Counter), out var counter))
		{
			Counter = counter;
		}
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		tag.Add(nameof(Counter), Counter);
	}
}