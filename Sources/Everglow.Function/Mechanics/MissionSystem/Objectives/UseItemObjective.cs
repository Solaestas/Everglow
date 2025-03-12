using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

[Obsolete("This class is yet unfinished, don't use it.", true)]
public class UseItemObjective : MissionObjectiveBase
{
	public List<CountItemRequirement> DemandUseItems { get; } = [];

	public override float Progress => DemandUseItems.Count != 0 && DemandUseItems.All(i => i.Requirement != 0)
		? DemandUseItems.Average(i => i.Progress())
		: 1f;

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaItemTextures(DemandUseItems.SelectMany(x => x.Items));
	}

	public override bool CheckCompletion() => DemandUseItems.All(i => i.Counter >= i.Requirement);

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var demand in DemandUseItems)
		{
			var progress = $"({demand.Counter}/{demand.Requirement})";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				lines.Add($"使用{itemString}合计{demand.Requirement}次 {progress}\n");
			}
			else
			{
				lines.Add($"使用{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}次 {progress}\n");
			}
		}
	}

	public override void Activate(MissionBase sourceMission)
	{
		MissionGlobalItem.OnUseItemEvent += MissionGlobalItem_OnUseItem;
	}

	public override void Deactivate()
	{
		MissionGlobalItem.OnUseItemEvent -= MissionGlobalItem_OnUseItem;
	}

	private void MissionGlobalItem_OnUseItem(Item item)
	{
		foreach (var di in DemandUseItems.Where(x => x.Items.Contains(item.type)))
		{
			di.Count();
		}
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		tag.TryGet<List<CountItemRequirement>>(nameof(DemandUseItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandUseItems)
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
		base.SaveData(tag);
		tag.Add(nameof(DemandUseItems), DemandUseItems);
	}
}