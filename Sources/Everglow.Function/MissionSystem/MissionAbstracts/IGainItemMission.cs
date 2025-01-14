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
	public abstract bool Consume { get; }

	public abstract List<GainItemRequirement> DemandItems { get; }

	public void ConsumeItem(IEnumerable<Item> inventory)
	{
		if (Consume)
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
		MissionBase.LoadVanillaItemTextures(DemandItems.SelectMany(x => x.Items));
	}
}