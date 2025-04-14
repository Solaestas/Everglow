using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class CollectItemObjective : MissionObjectiveBase
{
	public CollectItemObjective()
	{
	}

	public CollectItemObjective(CountItemRequirement requirement)
	{
		DemandCollectItem = requirement;
	}

	public CountItemRequirement DemandCollectItem { get; set; }

	public override float Progress => DemandCollectItem.Progress(Main.LocalPlayer);

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaItemTextures(DemandCollectItem.Items);
	}

	public override bool CheckCompletion() => Progress >= 1f;

	public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	{
		foreach (var item in DemandCollectItem.Items)
		{
			iconGroup.Add(ItemMissionIcon.Create(item, new Item(item).Name));
		}
	}

	public override void GetObjectivesText(List<string> lines)
	{
		string progress = DemandCollectItem.EnableIndividualCounter
			? $"({DemandCollectItem.Counter}/{DemandCollectItem.Requirement})"
			: $"({Main.LocalPlayer.inventory.Where(i => DemandCollectItem.Items.Contains(i.type)).Sum(i => i.stack)}/{DemandCollectItem.Requirement})";
		var verbString = DemandCollectItem.EnableIndividualCounter ? "获取" : "拥有";
		if (DemandCollectItem.Items.Count > 1)
		{
			var itemString = string.Join(' ', DemandCollectItem.Items.ConvertAll(i => ItemDrawer.Create(i)));
			lines.Add($"{verbString}{itemString}合计{DemandCollectItem.Requirement}个 {progress}\n");
		}
		else
		{
			lines.Add($"{verbString}{ItemDrawer.Create(DemandCollectItem.Items.First())}{DemandCollectItem.Requirement}个 {progress}\n");
		}
	}

	/// <summary>
	/// Count pick item.
	/// </summary>
	/// <param name="item"></param>
	public void MissionPlayer_OnPickUp(Item item)
	{
		if (DemandCollectItem.Items.Contains(item.type) && DemandCollectItem.EnableIndividualCounter)
		{
			DemandCollectItem.Count(item.stack);
		}
	}

	public override void Activate(MissionBase sourceMission)
	{
		MissionPlayer.GlobalOnPickupEvent += MissionPlayer_OnPickUp;
	}

	public override void Deactivate()
	{
		MissionPlayer.GlobalOnPickupEvent -= MissionPlayer_OnPickUp;
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		tag.TryGet<CountItemRequirement>(nameof(DemandCollectItem), out var demandCollectItem);
		if (DemandCollectItem.EnableIndividualCounter)
		{
			if (demandCollectItem != null && demandCollectItem.Counter > 0)
			{
				DemandCollectItem.Count(demandCollectItem.Counter);
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		tag.Add(nameof(DemandCollectItem), DemandCollectItem);
	}
}