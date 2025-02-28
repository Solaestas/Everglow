using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Hooks;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class CollectItemObjective : MissionObjectiveBase
{
	public List<CollectItemRequirement> DemandCollectItems { get; } = [];

	public override float Progress => DemandCollectItems.Count != 0 && DemandCollectItems.All(x => x.Requirement != 0)
		? DemandCollectItems.Select(x => x.Progress(Main.LocalPlayer.inventory)).Average()
		: 1f;

	public override void OnInitialize()
	{
		base.OnInitialize();
		MissionBase.LoadVanillaItemTextures(DemandCollectItems.SelectMany(x => x.Items));
	}

	public override bool CheckCompletion() => throw new NotImplementedException();

	public override void GetObjectivesText(List<string> lines)
	{
		foreach (var demand in DemandCollectItems)
		{
			string progress = demand.EnableIndividualCounter
				? $"({demand.Counter}/{demand.Requirement})"
				: $"({Main.LocalPlayer.inventory.Where(i => demand.Items.Contains(i.type)).Sum(i => i.stack)}/{demand.Requirement})";
			var verb = demand.EnableIndividualCounter
				? "获取"
				: "拥有";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				lines.Add($"{verb}{itemString}合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				lines.Add($"{verb}{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}个 {progress}\n");
			}
		}
	}

	/// <summary>
	/// Count pick item.
	/// </summary>
	/// <param name="item"></param>
	public void MissionPlayer_OnPickUp(Item item)
	{
		foreach (var kmDemand in DemandCollectItems.Where(x => x.Items.Contains(item.type)))
		{
			if (kmDemand.EnableIndividualCounter)
			{
				kmDemand.Count(item.stack);
			}
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
		tag.TryGet<List<CollectItemRequirement>>(nameof(DemandCollectItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandCollectItems.Where(d => d.EnableIndividualCounter))
			{
				demand.Count(
					demandNPCs
						.Where(d => d.EnableIndividualCounter && d.Items.Intersect(demand.Items).Any())
						.Sum(x => x.Counter));
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag.Add(nameof(DemandCollectItems), DemandCollectItems);
	}
}