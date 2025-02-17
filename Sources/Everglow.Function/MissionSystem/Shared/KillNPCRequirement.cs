using Everglow.Commons.MissionSystem.Core;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Shared;

/// <summary>
/// A group of npc which use the same requirement
/// </summary>
public class KillNPCRequirement
{
	private KillNPCRequirement(IEnumerable<int> nPCs, int requirement, bool enableIndividualCounter, int counter = 0)
	{
		NPCs = nPCs.ToList();
		Requirement = requirement;
		EnableIndividualCounter = enableIndividualCounter;
		Counter = counter;
	}

	/// <summary>
	/// NPC types
	/// </summary>
	public List<int> NPCs { get; init; }

	/// <summary>
	/// Kill count requirement
	/// </summary>
	public int Requirement { get; init; }

	/// <summary>
	/// Use individual counter to calculate progress
	/// </summary>
	public bool EnableIndividualCounter { get; init; }

	/// <summary>
	/// Gets or sets the count of NPCs killed, based on whether individual counting is enabled.
	/// </summary>
	public int Counter { get; private set; }

	/// <summary>
	/// Represents the progress towards fulfilling the NPC kill requirement.
	/// </summary>
	/// <remarks>
	/// This property returns a floating-point number between 0 and 1, representing the ratio of the current NPC kills to the required number of kills.
	/// <para/>
	/// - When <see cref="EnableIndividualCounter"/> is <c>true</c>, the progress is calculated based on the value of the <see cref="Counter"/> property.
	/// <para/>
	/// - When <see cref="EnableIndividualCounter"/> is <c>false</c>, the progress is calculated by summing the kill counts for the NPCs in the <see cref="NPCs"/> list from a global kill counter (<see cref="MissionManager.NPCKillCounter"/>).
	/// <para/>
	/// The returned value is clamped to the range [0, 1], ensuring that the progress is always represented as a percentage (0% to 100%).
	/// </remarks>
	public float Progress(IDictionary<int, int> nPCKillCounter) => EnableIndividualCounter
		? Math.Min(1f, Math.Max(0f, Counter / (float)Requirement))
		: Math.Min(1f, Math.Max(0f, nPCKillCounter.Where(x => NPCs.Contains(x.Key)).Select(x => x.Value).Sum() / (float)Requirement));

	public MissionCondition Condition { get; private set; }

	/// <summary>
	/// Create a new instance of <see cref="KillNPCRequirement"/> class if the input is valid.
	/// </summary>
	/// <param name="nPCs">A list of NPC id. Must not be empty.</param>
	/// <param name="requirement">The requirement value. Must be greater than 0.</param>
	/// <returns>A new <see cref="KillNPCRequirement"/> instance if the input is valid; otherwise, returns <c>null</c>.</returns>
	public static KillNPCRequirement Create(List<int> nPCs, int requirement, bool enableIndividualCounter = false)
	{
		if (nPCs.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (requirement <= 0)
		{
			throw new InvalidDataException();
		}

		return new KillNPCRequirement(nPCs, requirement, enableIndividualCounter);
	}

	public KillNPCRequirement SetCondition(MissionCondition condition)
	{
		Condition = condition;
		return this;
	}

	/// <summary>
	/// Add count to Counter
	/// <para/>This method should only be called when <see cref="EnableIndividualCounter"/> is <c>true</c>
	/// </summary>
	/// <param name="count"></param>
	public void Count(int count = 1)
	{
		if (Condition != null && Condition.IsFalse)
		{
			return;
		}

		if (EnableIndividualCounter)
		{
			Counter += count;

			if (Counter > Requirement)
			{
				Counter = Requirement;
			}
		}
	}

	public class KillNPCRequirementSerializer : TagSerializer<KillNPCRequirement, TagCompound>
	{
		public override TagCompound Serialize(KillNPCRequirement value) => new TagCompound()
		{
			[nameof(NPCs)] = value.NPCs,
			[nameof(Requirement)] = value.Requirement,
			[nameof(EnableIndividualCounter)] = value.EnableIndividualCounter,
			[nameof(Counter)] = value.Counter,
		};

		public override KillNPCRequirement Deserialize(TagCompound tag) =>
			new KillNPCRequirement(
				tag.GetList<int>(nameof(NPCs)),
				tag.GetInt(nameof(Requirement)),
				tag.GetBool(nameof(EnableIndividualCounter)),
				tag.GetInt(nameof(Counter)));
	}
}
