using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared.Requirements;

/// <summary>
/// A group of items which use the same requirement
/// </summary>
public class CountItemRequirement : ItemRequirement
{
	private CountItemRequirement(List<int> items, int requirement, bool enableIndividualCounter, int counter = 0)
		: base(items, requirement)
	{
		Counter = counter;
		EnableIndividualCounter = enableIndividualCounter;
	}

	protected int counter = 0;

	public int Counter
	{
		get => counter;
		protected set => counter = value;
	}

	public bool EnableIndividualCounter { get; init; }

	/// <summary>
	/// Add count to Counter
	/// <para/>This method should only be called when <see cref="EnableIndividualCounter"/> is <c>true</c>
	/// </summary>
	/// <param name="count"></param>
	public void Count(int count = 1)
	{
		if (EnableIndividualCounter)
		{
			Counter += count;

			if (Counter > Requirement)
			{
				Counter = Requirement;
			}
		}
		else
		{
			throw new InvalidOperationException();
		}
	}

	public float CheckCounter() => Requirement != 0 ? Math.Clamp(Counter / (float)Requirement, 0f, 1f) : 1f;

	/// <summary>
	/// Represents the progress towards fulfilling the item requirement.
	/// </summary>
	/// <remarks>
	/// This property returns a floating-point number between 0 and 1, representing the ratio of the current items' stack to the required number.
	/// <para/>
	/// The returned value is clamped to the range [0, 1], ensuring that the progress is always represented as a percentage (0% to 100%).
	/// </remarks>
	public override float Progress(Player player) => EnableIndividualCounter
		? CheckCounter()
		: CheckInventory(player.inventory);

	/// <summary>
	/// Create a new instance of <see cref="CountItemRequirement"/> class if the input is valid.
	/// </summary>
	/// <param name="items">A list of NPC id. Must not be empty.</param>
	/// <param name="requirement">The requirement value. Must be greater than 0.</param>
	/// <param name="enableIndividualCounter"> <c>true</c> to enable individual counter; otherwise, <c>false</c>.</param>
	/// <returns>A new <see cref="CountItemRequirement"/> instance if the input is valid; otherwise, returns <c>null</c>.</returns>
	public static CountItemRequirement Create(List<int> items, int requirement, bool enableIndividualCounter = true)
	{
		if (items.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (requirement <= 0)
		{
			throw new InvalidDataException();
		}

		return new CountItemRequirement(items, requirement, enableIndividualCounter);
	}

	public class CollectItemRequirementSerializer : TagSerializer<CountItemRequirement, TagCompound>
	{
		public override TagCompound Serialize(CountItemRequirement value) => new TagCompound()
		{
			[nameof(Items)] = value.Items,
			[nameof(Requirement)] = value.Requirement,
			[nameof(EnableIndividualCounter)] = value.EnableIndividualCounter,
			[nameof(Counter)] = value.counter,
		};

		public override CountItemRequirement Deserialize(TagCompound tag) => new CountItemRequirement(
			tag.GetList<int>(nameof(Items)).ToList(),
			tag.GetInt(nameof(Requirement)),
			tag.GetBool(nameof(EnableIndividualCounter)),
			tag.GetInt(nameof(Counter)));
	}
}