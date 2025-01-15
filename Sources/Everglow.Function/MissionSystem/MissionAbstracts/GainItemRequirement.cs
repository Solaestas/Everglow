using MathNet.Numerics;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionAbstracts;

/// <summary>
/// A group of items which use the same requirement
/// </summary>
public class GainItemRequirement : ItemRequirement
{
	private GainItemRequirement(IEnumerable<int> items, int requirement, bool enableIndividualCounter, int counter = 0)
		: base(items, requirement, counter)
	{
		EnableIndividualCounter = enableIndividualCounter;
	}

	public bool EnableIndividualCounter { get; init; }

	/// <summary>
	/// Add count to Counter
	/// <para/>This method should only be called when <see cref="EnableIndividualCounter"/> is <c>true</c>
	/// </summary>
	/// <param name="count"></param>
	public new void Count(int count = 1)
	{
		if (EnableIndividualCounter)
		{
			base.Count(count);
		}
		else
		{
			throw new InvalidOperationException();
		}
	}

	private new float Progress() => base.Progress();

	/// <summary>
	/// Represents the progress towards fulfilling the item requirement.
	/// </summary>
	/// <remarks>
	/// This property returns a floating-point number between 0 and 1, representing the ratio of the current items' stack to the required number.
	/// <para/>
	/// The returned value is clamped to the range [0, 1], ensuring that the progress is always represented as a percentage (0% to 100%).
	/// </remarks>
	public float Progress(IEnumerable<Item> inventory) => EnableIndividualCounter
		? Progress()
		: Math.Min(1f, Math.Max(0f, inventory.Where(x => Items.Contains(x.type)).Select(x => x.stack).Sum() / (float)Requirement));

	/// <summary>
	/// Create a new instance of <see cref="GainItemRequirement"/> class if the input is valid.
	/// </summary>
	/// <param name="items">A list of NPC id. Must not be empty.</param>
	/// <param name="requirement">The requirement value. Must be greater than 0.</param>
	/// <returns>A new <see cref="GainItemRequirement"/> instance if the input is valid; otherwise, returns <c>null</c>.</returns>
	public static GainItemRequirement Create(List<int> items, int requirement, bool enableIndividualCounter = false)
	{
		if (items.Count == 0)
		{
			throw new InvalidParameterException();
		}

		if (requirement <= 0)
		{
			throw new InvalidParameterException();
		}

		return new GainItemRequirement(items, requirement, enableIndividualCounter);
	}

	public class GainItemRequirementSerializer : TagSerializer<GainItemRequirement, TagCompound>
	{
		public override TagCompound Serialize(GainItemRequirement value) => new TagCompound()
		{
			[nameof(Items)] = value.Items,
			[nameof(Requirement)] = value.Requirement,
			[nameof(EnableIndividualCounter)] = value.EnableIndividualCounter,
			[nameof(Counter)] = value.counter,
		};

		public override GainItemRequirement Deserialize(TagCompound tag) => new GainItemRequirement(
			tag.GetList<int>(nameof(Items)),
			tag.GetInt(nameof(Requirement)),
			tag.GetBool(nameof(EnableIndividualCounter)),
			tag.GetInt(nameof(Counter)));
	}
}