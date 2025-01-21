using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

/// <summary>
/// Represents a mission where the player needs to obtain a specified item or a quantity of items.
/// </summary>
public interface ICollectItemMission : IMissionObjective
{
	public abstract List<CollectItemRequirement> DemandCollectItems { get; init; }

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountPick(Item item)
	{
		foreach (var kmDemand in DemandCollectItems.Where(x => x.Items.Contains(item.type)))
		{
			if (kmDemand.EnableIndividualCounter)
			{
				kmDemand.Count(item.stack);
			}
		}
	}

	/// <summary>
	/// Calculate mission progress
	/// </summary>
	/// <param name="inventory"></param>
	/// <returns></returns>
	public float CalculateProgress(IEnumerable<Item> inventory)
	{
		if (DemandCollectItems.Count == 0 || DemandCollectItems.Select(x => x.Requirement).Sum() == 0)
		{
			return 1f;
		}

		return DemandCollectItems.Select(x => x.Progress(inventory)).Average();
	}

	public void Load(TagCompound tag)
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

		MissionBase.LoadVanillaItemTextures(DemandCollectItems.SelectMany(x => x.Items));
	}

	public void Save(TagCompound tag)
	{
		tag.Add(nameof(DemandCollectItems), DemandCollectItems);
	}

	public IEnumerable<string> GetObjectivesString(IEnumerable<Item> inventory)
	{
		var objectives = new List<string>();

		foreach (var demand in DemandCollectItems)
		{
			string progress = demand.EnableIndividualCounter
				? $"({demand.Counter}/{demand.Requirement})"
				: $"({inventory.Where(i => demand.Items.Contains(i.type)).Sum(i => i.stack)}/{demand.Requirement})";
			var verb = demand.EnableIndividualCounter
				? "获取"
				: "拥有";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				objectives.Add($"{verb}{itemString}合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				objectives.Add($"{verb}{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}个 {progress}\n");
			}
		}

		return objectives;
	}
}