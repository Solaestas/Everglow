using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to obtain a specified item or a quantity of items.
/// </summary>
public class GainItemMission : MissionBase
{
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

	public override Texture2D Icon => DemandItems.Count > 0 ? TextureAssets.Item[DemandItems.First().type].Value : null;

	public string SourceContext => $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";

	public List<Item> DemandItems { get; init; } = [];

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
		if (Consume)
		{
			foreach (var item in DemandItems)
			{
				var stack = item.stack;

				foreach (var inventoryItem in Main.LocalPlayer.inventory.Where(x => x.type == item.type))
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

	public override void Load(TagCompound tag)
	{
		base.Load(tag);
		tag.TryGet(nameof(Name), out name);
		tag.TryGet(nameof(DisplayName), out displayName);
		tag.TryGet(nameof(Description), out description);
		tag.TryGet(nameof(TimeMax), out timeMax);

		DemandItems.Clear();
		if (tag.TryGet<IList<TagCompound>>(nameof(DemandItems), out var diTag))
		{
			foreach (var iTag in diTag)
			{
				DemandItems.Add(ItemIO.Load(iTag));
			}
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
			DemandItems.Select(x => x.type)
			.Concat(RewardItems.Select(x => x.type)));
	}

	public override void Save(TagCompound tag)
	{
		base.Save(tag);
		tag.Add(nameof(TimeMax), TimeMax);
		tag.Add(nameof(Name), Name);
		tag.Add(nameof(DisplayName), DisplayName);
		tag.Add(nameof(Description), Description);
		tag.Add(nameof(DemandItems), DemandItems.ConvertAll(ItemIO.Save));
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

		// Calculate required item types and their stack
		var demandItemStacks = DemandItems
			.GroupBy(item => item.type)
			.ToDictionary(
				group => group.Key,
				group => group.Sum(item => item.stack));

		// Calculate owned item types and their stack
		var ownedItemStacks = paramItems
			.Where(item => demandItemStacks.ContainsKey(item.type))
			.GroupBy(item => item.type)
			.ToDictionary(
				group => group.Key,
				group => group.Sum(item => item.stack));

		// Calculate mission progress
		if (demandItemStacks.Count == 0 || demandItemStacks.Values.Sum() == 0)
		{
			_progress = 1f;
		}
		else
		{
			// The final progress is calculated as the average of the individual progress
			// for each item type, where individual progress is the ratio of owned items
			// to required items (capped at 1 and floored at 0) for that item type.
			float IndividualProgress(KeyValuePair<int, int> requiredItem)
			{
				var ownedStack = ownedItemStacks.GetValueOrDefault(requiredItem.Key, 0);
				var requiredStack = (float)requiredItem.Value;
				return Math.Min(1f, Math.Max(0f, ownedStack / requiredStack));
			}
			_progress = demandItemStacks.Select(IndividualProgress).Average();
		}
	}
}