using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public class CountItemRequirement : ItemRequirementBase
{
	protected CountItemRequirement(List<int> items, int requirement, int counter = 0)
		: base(items, requirement)
	{
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
	public override List<int> Items { get; init; }

	/// <summary>
	/// Gain count requirement
	/// </summary>
	public override int Requirement { get; init; }

	public void Count(int count = 1)
	{
		Counter += count;

		if (Counter > Requirement)
		{
			Counter = Requirement;
		}
	}

	public float Progress() => Math.Min(1f, Math.Max(0f, Counter / (float)Requirement));

	public static CountItemRequirement Create(List<int> items, int requirement)
	{
		if (items.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (requirement <= 0)
		{
			throw new InvalidDataException();
		}

		return new CountItemRequirement(items, requirement);
	}

	public class ItemRequirementSerializer : TagSerializer<CountItemRequirement, TagCompound>
	{
		public override TagCompound Serialize(CountItemRequirement value) => new TagCompound()
		{
			[nameof(Items)] = value.Items,
			[nameof(Requirement)] = value.Requirement,
			[nameof(Counter)] = value.Counter,
		};

		public override CountItemRequirement Deserialize(TagCompound tag) => new CountItemRequirement(
			tag.GetList<int>(nameof(Items)).ToList(),
			tag.GetInt(nameof(Requirement)),
			tag.GetInt(nameof(Counter)));
	}
}