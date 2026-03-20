using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

[Obsolete("This class is yet unfinished, don't use it.", true)]
public class UseItemObjective : MissionObjectiveBase
{
	public UseItemObjective()
	{
	}

	public UseItemObjective(ItemRequirement requirement)
	{
		DemandUseItem = requirement;
	}

	public ItemRequirement DemandUseItem { get; }

	public ProgressCounter Counter { get; private set; } = new();

	public override float Progress => DemandUseItem.GetCounterProgress(Counter);

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaItemTextures(DemandUseItem.Items);
	}

	public override bool CheckCompletion() => Counter.Value >= DemandUseItem.Requirement;

	public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	{
		foreach (var item in DemandUseItem.Items)
		{
			iconGroup.Add(ItemMissionIcon.Create(item, new Item(item).Name));
		}
	}

	public override void GetObjectivesText(List<string> lines)
	{
		var progress = $"({Counter.Value}/{DemandUseItem.Requirement})";
		if (DemandUseItem.Items.Count > 1)
		{
			var itemString = string.Join(' ', DemandUseItem.Items.ConvertAll(i => ItemDrawer.Create(i)));
			lines.Add($"使用{itemString}合计{DemandUseItem.Requirement}次 {progress}\n");
		}
		else
		{
			lines.Add($"使用{ItemDrawer.Create(DemandUseItem.Items.First())}{DemandUseItem.Requirement}次 {progress}\n");
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
		if (DemandUseItem.Items.Contains(item.type))
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