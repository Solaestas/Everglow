using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to kill a specified NPC or a quantity of NPCs.
/// </summary>
public class KillNPCMission : MissionBase
{
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

	private string name = string.Empty;
	private string displayName = string.Empty;
	private string description = string.Empty;
	private float progress = 0f;
	private long timeMax = -1;

	public override string Name => name;

	public override string DisplayName => displayName;

	public override string Description => description;

	public override Texture2D Icon => TextureAssets.NpcHead[DemandNPCs.First()?.NPCs.First() ?? NPCID.BlueSlime].Value;

	public override float Progress => progress;

	public override long TimeMax => timeMax;

	public string SourceContext => $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";

	public List<KillNPCRequirement> DemandNPCs { get; init; } = [];

	public List<Item> RewardItems { get; init; } = [];

	/// <summary>
	/// Sets the basic information for the mission.
	/// </summary>
	/// <param name="name">The unique name of the mission.</param>
	/// <param name="displayName">The display name of the mission.</param>
	/// <param name="description">A brief description of the mission.</param>
	/// <param name="timeMax">The maximum time allowed to complete the mission, in ticks. Use -1 for no time limit.</param>
	/// <exception cref="ArgumentNullException">Thrown if any of the string parameters are null.</exception>
	public void SetInfo(string name, string displayName, string description, long timeMax = -1)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException(nameof(name), "Mission name cannot be null or empty.");
		}

		if (string.IsNullOrWhiteSpace(displayName))
		{
			throw new ArgumentNullException(nameof(displayName), "Mission display name cannot be null or empty.");
		}

		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentNullException(nameof(description), "Mission description cannot be null or empty.");
		}

		this.name = name;
		this.displayName = displayName;
		this.description = description;
		this.timeMax = timeMax;
	}

	public override void OnCompleteCustom()
	{
		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(SourceContext), item, item.stack);
		}
	}

	public override void Update()
	{
		base.Update();

		UpdateProgress();
	}

	public override void UpdateProgress(params object[] objs)
	{
		if (PoolType != MissionManager.PoolType.Accepted)
		{
			return;
		}

		if (DemandNPCs.Count == 0)
		{
			progress = 1f;
			return;
		}

		// The final progress is calculated as the average of the individual progress for each NPC type group
		progress = DemandNPCs.Select(x => x.Progress).Average();
	}

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountKill(int type, int count = 1)
	{
		foreach (var kmDemand in DemandNPCs.Where(x => x.NPCs.Contains(type)))
		{
			if (kmDemand.EnableIndividualCounter)
			{
				kmDemand.Count(count);
			}
		}
	}

	public override void Load(TagCompound tag)
	{
		base.Load(tag);
		tag.TryGet(nameof(Name), out name);
		tag.TryGet(nameof(DisplayName), out displayName);
		tag.TryGet(nameof(Description), out description);
		tag.TryGet(nameof(TimeMax), out timeMax);

		DemandNPCs.Clear();
		tag.TryGet<List<KillNPCRequirement>>(nameof(DemandNPCs), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			DemandNPCs.AddRange(demandNPCs);
		}

		RewardItems.Clear();
		if (tag.TryGet<IList<TagCompound>>(nameof(RewardItems), out var riTag))
		{
			foreach (var iTag in riTag)
			{
				RewardItems.Add(ItemIO.Load(iTag));
			}
		}

		LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	public override void Save(TagCompound tag)
	{
		base.Save(tag);
		tag.Add(nameof(TimeMax), TimeMax);
		tag.Add(nameof(Name), Name);
		tag.Add(nameof(DisplayName), DisplayName);
		tag.Add(nameof(Description), Description);
		tag.Add(nameof(DemandNPCs), DemandNPCs);
		tag.Add(nameof(RewardItems), RewardItems.ConvertAll(ItemIO.Save));
	}
}