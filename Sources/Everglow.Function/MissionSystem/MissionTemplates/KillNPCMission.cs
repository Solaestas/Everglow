using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to kill a specified NPC or a quantity of NPCs.
/// </summary>
public class KillNPCMission : MissionBase
{
	public class KillNPCRequirement
	{
		private KillNPCRequirement(List<int> nPCs, int requirement, bool enableIndividualCounter)
		{
			NPCs = nPCs;
			Requirement = requirement;
			EnableIndividualCounter = enableIndividualCounter;
			if (EnableIndividualCounter)
			{
				Counter = 0;
			}
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
		/// Individual counter
		/// </summary>
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
		/// The progress of requirement
		/// </summary>
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

	public override void OnComplete()
	{
		base.OnComplete();

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

	public override void Load(TagCompound tag) => base.Load(tag);

	// TODO: DemandNPCs and RewardItems persistence
	public override void Save(TagCompound tag) => base.Save(tag);
}