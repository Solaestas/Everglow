namespace Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;

public class ItemRequirement
{
	protected ItemRequirement(List<int> items, int requirement)
	{
		Items = items;
		Requirement = requirement;
	}

	/// <summary>
	/// Item types
	/// </summary>
	public List<int> Items { get; init; }

	/// <summary>
	/// Gain count requirement
	/// </summary>
	public int Requirement { get; init; }

	public float CheckInventory(IEnumerable<Item> inventory) => Math.Clamp(inventory.Where(x => Items.Contains(x.type)).Sum(x => x.stack) / (float)Requirement, 0f, 1f);

	public virtual float Progress(Player player) => CheckInventory(player.inventory);

	public static ItemRequirement Create(List<int> items, int requirement)
	{
		if (items.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (requirement <= 0)
		{
			throw new InvalidDataException();
		}

		return new ItemRequirement(items, requirement);
	}
}