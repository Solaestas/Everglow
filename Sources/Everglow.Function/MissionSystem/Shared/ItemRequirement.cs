using MathNet.Numerics;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Shared;

public class ItemRequirement
{
	protected ItemRequirement(IEnumerable<int> items, int requirement, int counter = 0)
	{
		Items = items.ToList();
		Requirement = requirement;
		Counter = counter;
	}

	protected int counter = 0;

	public int Counter
	{
		get => counter;
		protected set => counter = value;
	}

	/// <summary>
	/// Item types
	/// </summary>
	public List<int> Items { get; init; }

	/// <summary>
	/// Gain count requirement
	/// </summary>
	public int Requirement { get; init; }

	public void Count(int count = 1)
	{
		Counter += count;

		if (Counter > Requirement)
		{
			Counter = Requirement;
		}
	}

	public float Progress() => Math.Min(1f, Math.Max(0f, Counter / (float)Requirement));

	public static ItemRequirement Create(List<int> items, int requirement)
	{
		if (items.Count == 0)
		{
			throw new InvalidParameterException();
		}

		if (requirement <= 0)
		{
			throw new InvalidParameterException();
		}

		return new ItemRequirement(items, requirement);
	}

	public class ItemRequirementSerializer : TagSerializer<ItemRequirement, TagCompound>
	{
		public override TagCompound Serialize(ItemRequirement value) => new TagCompound()
		{
			[nameof(Items)] = value.Items,
			[nameof(Requirement)] = value.Requirement,
			[nameof(Counter)] = value.Counter,
		};

		public override ItemRequirement Deserialize(TagCompound tag) => new ItemRequirement(
			tag.GetList<int>(nameof(Items)),
			tag.GetInt(nameof(Requirement)),
			tag.GetInt(nameof(Counter)));
	}
}