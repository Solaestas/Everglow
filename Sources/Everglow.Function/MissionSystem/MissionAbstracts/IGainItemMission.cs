using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionAbstracts;

/// <summary>
/// Represents a mission where the player needs to obtain a specified item or a quantity of items.
/// </summary>
public interface IGainItemMission
{
	/// <summary>
	/// Determine if the demand items will be consumed on mission complete.
	/// </summary>
	public abstract bool SubmitItemsOnComplete { get; }

	public abstract List<GainItemRequirement> DemandItems { get; init; }

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountPick(Item item)
	{
		foreach (var kmDemand in DemandItems.Where(x => x.Items.Contains(item.type)))
		{
			if (kmDemand.EnableIndividualCounter)
			{
				kmDemand.Count(item.stack);
			}
		}
	}

	public void ConsumeItem(IEnumerable<Item> inventory)
	{
		if (SubmitItemsOnComplete)
		{
			foreach (var item in DemandItems)
			{
				var stack = item.Requirement;

				foreach (var inventoryItem in inventory.Where(x => item.Items.Contains(x.type)))
				{
					if (inventoryItem.stack < stack)
					{
						stack -= inventoryItem.stack;
						inventoryItem.stack = 0;
					}
					else
					{
						inventoryItem.stack -= stack;
						break;
					}
				}
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

		if (DemandItems.Count == 0 || DemandItems.Select(x => x.Requirement).Sum() == 0)
		{
			return 1f;
		}

		return DemandItems.Select(x => x.Progress(inventory)).Average();
	}

	public void Load(TagCompound tag)
	{
		tag.TryGet<List<GainItemRequirement>>(nameof(DemandItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandItems.Where(d => d.EnableIndividualCounter))
			{
				demand.Count(
					demandNPCs
						.Where(d => d.EnableIndividualCounter && d.Items.Intersect(demand.Items).Any())
						.Sum(x => x.Counter));
			}
		}

		MissionBase.LoadVanillaItemTextures(DemandItems.SelectMany(x => x.Items));
	}

	public void Save(TagCompound tag)
	{
		tag.Add(nameof(DemandItems), DemandItems);
	}
}