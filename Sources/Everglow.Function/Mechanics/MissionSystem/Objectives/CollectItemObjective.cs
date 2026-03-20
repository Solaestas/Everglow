using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class CollectItemObjective : MissionObjectiveBase
{
	public CollectItemObjective()
	{
	}

	public CollectItemObjective(ItemRequirement requirement, bool enableIndividualCounter = true)
	{
		DemandCollectItem = requirement;
		EnableIndividualCounter = enableIndividualCounter;
	}

	public ItemRequirement DemandCollectItem { get; set; }

	public ProgressCounter Counter { get; private set; } = new();

	public bool EnableIndividualCounter { get; set; } = false;

	public override float Progress => CalculateProgress(Main.LocalPlayer);

	/// <summary>
	/// Calculate the progress of the objective.
	/// <para/> This method is created for unit tests, so it is not recommended to use it in other places.
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public float CalculateProgress(Player player) => EnableIndividualCounter
		? DemandCollectItem.GetCounterProgress(Counter)
		: DemandCollectItem.GetInventoryProgress(player.inventory);

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
		string progress = EnableIndividualCounter
			? $"({Counter}/{DemandCollectItem.Requirement})"
			: $"({Main.LocalPlayer.inventory.Where(i => DemandCollectItem.Items.Contains(i.type)).Sum(i => i.stack)}/{DemandCollectItem.Requirement})";
		var verbString = EnableIndividualCounter ? "获取" : "拥有";
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
		if (DemandCollectItem.Items.Contains(item.type) && EnableIndividualCounter)
		{
			Counter.Count(item.stack);
		}
	}

	public override void Activate(MissionBase sourceMission)
	{
		MissionPlayer.OnPickupEvent += MissionPlayer_OnPickUp;
	}

	public override void Deactivate()
	{
		MissionPlayer.OnPickupEvent -= MissionPlayer_OnPickUp;
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