using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to obtain a specified item or a quantity of items.
/// </summary>
public class GainItemMission : MissionBase
{
	/// <summary>
	/// A group of items which use the same requirement
	/// </summary>
	public class GainItemRequirement
	{
		private GainItemRequirement(IEnumerable<int> items, int requirement)
		{
			Items = items.ToList();
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

		/// <summary>
		/// Represents the progress towards fulfilling the item requirement.
		/// </summary>
		/// <remarks>
		/// This property returns a floating-point number between 0 and 1, representing the ratio of the current items' stack to the required number.
		/// <para/>
		/// The returned value is clamped to the range [0, 1], ensuring that the progress is always represented as a percentage (0% to 100%).
		/// </remarks>
		public float Progress(IEnumerable<Item> inventory) =>
			Math.Min(1f, Math.Max(0f, inventory.Where(x => Items.Contains(x.type)).Select(x => x.stack).Sum() / (float)Requirement));

		/// <summary>
		/// Create a new instance of <see cref="GainItemRequirement"/> class if the input is valid.
		/// </summary>
		/// <param name="items">A list of NPC id. Must not be empty.</param>
		/// <param name="requirement">The requirement value. Must be greater than 0.</param>
		/// <returns>A new <see cref="GainItemRequirement"/> instance if the input is valid; otherwise, returns <c>null</c>.</returns>
		public static GainItemRequirement Create(List<int> items, int requirement)
		{
			if (items.Count == 0)
			{
				return null;
			}

			if (requirement <= 0)
			{
				return null;
			}

			return new GainItemRequirement(items, requirement);
		}

		public class GainItemRequirementSerializer : TagSerializer<GainItemRequirement, TagCompound>
		{
			public override TagCompound Serialize(GainItemRequirement value) => new TagCompound()
			{
				[nameof(Items)] = value.Items,
				[nameof(Requirement)] = value.Requirement,
			};

			public override GainItemRequirement Deserialize(TagCompound tag) => new GainItemRequirement(
				tag.GetList<int>(nameof(Items)),
				tag.GetInt(nameof(Requirement)));
		}
	}

	private string name = string.Empty;
	private string displayName = string.Empty;
	private string description = string.Empty;
	private float _progress = 0f;
	private long timeMax = -1;

	public override string Name => name;

	public override string DisplayName => displayName;

	public override string Description => description;

	public override float Progress => _progress;

	public override long TimeMax => timeMax;

	public override Texture2D Icon => DemandItems.Count > 0 ? TextureAssets.Item[DemandItems.First().Items.First()].Value : null;

	public string SourceContext => $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";

	/// <summary>
	/// Determine if the demand items will be consumed on mission complete.
	/// </summary>
	public bool Consume { get; set; } = false;

	public List<GainItemRequirement> DemandItems { get; init; } = [];

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

	public override void PostComplete()
	{
		if (Consume)
		{
			foreach (var item in DemandItems)
			{
				var stack = item.Requirement;

				foreach (var inventoryItem in Main.LocalPlayer.inventory.Where(x => item.Items.Contains(x.type)))
				{
					if (inventoryItem.stack < stack)
					{
						stack -= inventoryItem.stack;
						inventoryItem.stack = 0;
					}
					else
					{
						inventoryItem.stack -= stack;
						break;
					}
				}
			}
		}

		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(SourceContext), item, item.stack);
		}
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);
		tag.TryGet(nameof(Name), out name);
		tag.TryGet(nameof(DisplayName), out displayName);
		tag.TryGet(nameof(Description), out description);
		tag.TryGet(nameof(TimeMax), out timeMax);

		DemandItems.Clear();
		if (tag.TryGet<List<GainItemRequirement>>(nameof(DemandItems), out var demandItems))
		{
			DemandItems.AddRange(demandItems);
		}

		RewardItems.Clear();
		if (tag.TryGet<IList<TagCompound>>(nameof(RewardItems), out var riTag))
		{
			foreach (var iTag in riTag)
			{
				RewardItems.Add(ItemIO.Load(iTag));
			}
		}

		LoadVanillaItemTextures(
			DemandItems.SelectMany(x => x.Items)
			.Concat(RewardItems.Select(x => x.type)));
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
		tag.Add(nameof(TimeMax), TimeMax);
		tag.Add(nameof(Name), Name);
		tag.Add(nameof(DisplayName), DisplayName);
		tag.Add(nameof(Description), Description);
		tag.Add(nameof(DemandItems), DemandItems);
		tag.Add(nameof(RewardItems), RewardItems.ConvertAll(ItemIO.Save));
	}

	public override void Update()
	{
		base.Update();

		UpdateProgress(Main.LocalPlayer.inventory.ToList());
	}

	public override void UpdateProgress(params object[] objs)
	{
		if (PoolType != MissionManager.PoolType.Accepted)
		{
			return;
		}

		var paramItems = objs switch
		{
		[List<Item> items] => items,
			_ => null,
		};
		if (paramItems == null)
		{
			return;
		}

		// Calculate mission progress
		if (DemandItems.Count == 0 || DemandItems.Select(x => x.Requirement).Sum() == 0)
		{
			_progress = 1f;
		}
		else
		{
			_progress = DemandItems.Select(x => x.Progress(paramItems)).Average();
		}
	}
}