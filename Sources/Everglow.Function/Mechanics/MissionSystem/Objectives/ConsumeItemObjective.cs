using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class ConsumeItemObjective : MissionObjectiveBase
{
	public List<CountItemRequirement> DemandConsumeItems { get; } = [];

	public override float Progress => DemandConsumeItems.Count != 0 && DemandConsumeItems.All(i => i.Requirement != 0)
	? DemandConsumeItems.Average(i => i.Progress())
	: 1f;

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaItemTextures(DemandConsumeItems.SelectMany(x => x.Items));
	}

	public override bool CheckCompletion() => DemandConsumeItems.All(i => i.Counter >= i.Requirement);

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var demand in DemandConsumeItems)
		{
			var progress = $"({demand.Counter}/{demand.Requirement})";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				lines.Add($"消耗{itemString}合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				lines.Add($"消耗{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}个 {progress}\n");
			}
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
		foreach (var di in DemandConsumeItems.Where(x => x.Items.Contains(item.type)))
		{
			di.Count();
		}
	}

	public override void LoadData(TagCompound tag)
	{
		tag.TryGet<List<CountItemRequirement>>(nameof(DemandConsumeItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandConsumeItems)
			{
				demand.Count(
					demandNPCs
						.Where(d => d.Items.Intersect(demand.Items).Any())
						.Sum(x => x.Counter));
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add(nameof(DemandConsumeItems), DemandConsumeItems);
	}
}