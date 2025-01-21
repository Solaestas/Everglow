using Everglow.Commons.MissionSystem.Core;

namespace Everglow.Commons.MissionSystem.Shared;

public class GiveItemRequirement : ItemRequirementBase
{
	public GiveItemRequirement(List<int> items, int requirement)
		: base(items, requirement)
	{
		Items = items;
		Requirement = requirement;
	}

	/// <summary>
	/// Item types
	/// </summary>
	public override List<int> Items { get; init; }

	/// <summary>
	/// Gain count requirement
	/// </summary>
	public override int Requirement { get; init; }

	public float Progress(IEnumerable<Item> inventory) =>
		Math.Min(1f, Math.Max(0f, inventory.Where(x => Items.Contains(x.type)).Select(x => x.stack).Sum() / (float)Requirement));

	public static GiveItemRequirement Create(List<int> items, int requirement)
	{
		if (items.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (requirement <= 0)
		{
			throw new InvalidDataException();
		}

		return new GiveItemRequirement(items, requirement);
	}
}