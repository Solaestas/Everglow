using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionAbstracts;

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
		this.counter = counter;
	}

	private int counter = 0;

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
	/// <remarks>
	/// <para>
	/// If <see cref="EnableIndividualCounter"/> is <c>true</c>, this property directly tracks the number of NPCs killed for the current mission or objective.
	/// You can set this value to update the kill count, typically in response to an NPC death event.
	/// </para>
	/// <para>
	/// If <see cref="EnableIndividualCounter"/> is <c>false</c>, this property is read-only and its value is derived from a global kill counter that tracks kills for all NPCs in the <see cref="NPCs"/> list.
	/// In this case, setting this property has no effect, as the kill count is managed externally.
	/// </para>
	/// <para>
	/// The property's getter ensures that the returned value is always non-negative, reflecting the actual number of kills recorded.
	/// </para>
	/// </remarks>
	/// <value>
	/// The number of NPCs killed. This value is directly settable when <see cref="EnableIndividualCounter"/> is <c>true</c>.
	/// When <see cref="EnableIndividualCounter"/> is <c>false</c>, this property reflects the aggregated kill count fetched from a global kill counter (<see cref="MissionManager.NPCKillCounter"/>).
	/// </value>
	/// <exception cref="InvalidOperationException">Thrown if an attempt to set the counter is made when <see cref="EnableIndividualCounter"/> is <c>false</c>.</exception>
	public int Counter
	{
		get => EnableIndividualCounter
			? counter
			: MissionManager.Instance.NPCKillCounter.Where(x => NPCs.Contains(x.Key)).Select(x => x.Value).Sum();

		private set => counter =
			EnableIndividualCounter
			? value
			: throw new InvalidOperationException();
	}

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
	public float Progress => Math.Min(1f, Math.Max(0f, Counter / (float)Requirement));

	/// <summary>
	/// Create a new instance of <see cref="KillNPCRequirement"/> class if the input is valid.
	/// </summary>
	/// <param name="nPCs">A list of NPC id. Must not be empty.</param>
	/// <param name="requirement">The requirement value. Must be greater than 0.</param>
	/// <returns>A new <see cref="KillNPCRequirement"/> instance if the input is valid; otherwise, returns <c>null</c>.</returns>
	public static KillNPCRequirement Create(List<int> nPCs, int requirement, bool enableIndividualCounter)
	{
		if (nPCs.Count == 0)
		{
			return null;
		}

		if (requirement <= 0)
		{
			return null;
		}

		return new KillNPCRequirement(nPCs, requirement, enableIndividualCounter);
	}

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
		}
		else
		{
			return;
		}

		// Some times a lot of npc are killed in a shot time, then the kill counter might be increased
		// too much before the mission is moved to completed pool. So we should fix the value
		if (Counter > Requirement)
		{
			Counter = Requirement;
		}
	}

	public class KillNPCRequirementSerializer : TagSerializer<KillNPCRequirement, TagCompound>
	{
		public override TagCompound Serialize(KillNPCRequirement value) => new TagCompound()
		{
			[nameof(NPCs)] = value.NPCs,
			[nameof(Requirement)] = value.Requirement,
			[nameof(EnableIndividualCounter)] = value.EnableIndividualCounter,
			[nameof(Counter)] = value.counter,
		};

		public override KillNPCRequirement Deserialize(TagCompound tag) =>
			new KillNPCRequirement(
				tag.GetList<int>(nameof(NPCs)),
				tag.GetInt(nameof(Requirement)),
				tag.GetBool(nameof(EnableIndividualCounter)),
				tag.GetInt(nameof(Counter)));
	}
}
