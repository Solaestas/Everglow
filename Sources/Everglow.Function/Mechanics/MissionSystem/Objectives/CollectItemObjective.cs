using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class CollectItemObjective : MissionObjectiveBase
{
	public CollectItemObjective()
	{
	}

	public CollectItemObjective(CollectItemRequirement requirement)
	{
		DemandCollectItem = requirement;
	}

	public CollectItemRequirement DemandCollectItem { get; set; }

	public override float Progress => DemandCollectItem.Progress(Main.LocalPlayer.inventory);

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaItemTextures(DemandCollectItem.Items);
	}

	public override bool CheckCompletion() => throw new NotImplementedException();

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
		MissionPlayer.OnPickupEvent += MissionPlayer_OnPickUp;
	}

	public override void Deactivate()
	{
		MissionPlayer.OnPickupEvent -= MissionPlayer_OnPickUp;
	}

	public override void LoadData(TagCompound tag)
	{
		tag.TryGet<CollectItemRequirement>(nameof(DemandCollectItem), out var demandCollectItem);
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
		tag.Add(nameof(DemandCollectItem), DemandCollectItem);
	}
}