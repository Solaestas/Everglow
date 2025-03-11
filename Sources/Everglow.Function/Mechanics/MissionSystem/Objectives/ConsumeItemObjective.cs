using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class ConsumeItemObjective : MissionObjectiveBase
{
	public ConsumeItemObjective()
	{
	}

	public ConsumeItemObjective(CountItemRequirement requirement)
	{
		DemandConsumeItem = requirement;
	}

	public CountItemRequirement DemandConsumeItem { get; set; }

	public override float Progress => DemandConsumeItem.Progress();

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaItemTextures(DemandConsumeItem.Items);
	}

	public override bool CheckCompletion() => DemandConsumeItem.Counter >= DemandConsumeItem.Requirement;

	public override void GetObjectivesText(List<string> lines)
	{
		var progress = $"({DemandConsumeItem.Counter}/{DemandConsumeItem.Requirement})";
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
			DemandConsumeItem.Count();
		}
	}

	public override void LoadData(TagCompound tag)
	{
		tag.TryGet<CountItemRequirement>(nameof(DemandConsumeItem), out var demandNPC);
		if (demandNPC != null && demandNPC.Counter > 0)
		{
			DemandConsumeItem.Count(demandNPC.Counter);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add(nameof(DemandConsumeItem), DemandConsumeItem);
	}
}