namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public class ItemRequirement
{
	public ItemRequirement(List<int> items, int requirement)
	{
		if (items.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (requirement <= 0)
		{
			throw new InvalidDataException();
		}

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

	public float GetInventoryProgress(IEnumerable<Item> inventory) => Math.Clamp(inventory.Where(x => Items.Contains(x.type)).Sum(x => x.stack) / (float)Requirement, 0f, 1f);

	public float GetCounterProgress(ProgressCounter counter) => Requirement != 0 ? Math.Clamp(counter.Value / (float)Requirement, 0f, 1f) : 1f;
}