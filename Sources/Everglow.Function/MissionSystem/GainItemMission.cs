using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem;

public class GainItemMission : MissionBase
{
	private string _name = string.Empty;
	private string _displayName = string.Empty;
	private string _description = string.Empty;
	private float _progress = 0f;
	private long _timeMax = -1;

	public override string Name => _name;

	public override string DisplayName => _displayName;

	public override string Description => _description;

	public override float Progress => _progress;

	public override long TimeMax => _timeMax;

	public override Texture2D Icon => DemandItem.Count > 0 ? TextureAssets.Item[DemandItem.First().type].Value : null;

	public List<Item> DemandItem { get; init; } = [];

	public List<Item> RewardItem { get; init; } = [];

	public void SetInfo(string name, string displayName, string description, long timeMax = -1)
	{
		_name = name;
		_displayName = displayName;
		_description = description;
		_timeMax = timeMax;
	}

	public override void OnFinish()
	{
		base.OnFinish();

		string sourceContext = $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";
		RewardItem.ForEach(i => Main.LocalPlayer.QuickSpawnItemDirect(Main.LocalPlayer.GetSource_Misc(sourceContext), i));
	}

	public override void Load(TagCompound tag)
	{
		base.Load(tag);
		tag.TryGet("Name", out _name);
		tag.TryGet("DisplayName", out _displayName);
		tag.TryGet("Description", out _description);
		tag.TryGet("TimeMax", out _timeMax);

		DemandItem.Clear();
		if (tag.TryGet<IList<TagCompound>>(nameof(DemandItem), out var diTag))
		{
			foreach (var iTag in diTag)
			{
				DemandItem.Add(ItemIO.Load(iTag));
			}
		}

		RewardItem.Clear();
		if (tag.TryGet<IList<TagCompound>>(nameof(RewardItem), out var riTag))
		{
			foreach (var iTag in riTag)
			{
				RewardItem.Add(ItemIO.Load(iTag));
			}
		}

		// Load not-loaded textures for required vanilla items (DemandItem, RewardItem)
		foreach (var type in DemandItem.Select(x => x.type)
			.Concat(RewardItem.Select(x => x.type))
			.Distinct())
		{
			// The Main.LoadItem function will skip the loaded items
			Main.instance.LoadItem(type);
		}
	}

	public override void Save(TagCompound tag)
	{
		base.Save(tag);
		tag.Add("TimeMax", TimeMax);
		tag.Add("Name", Name);
		tag.Add("DisplayName", DisplayName);
		tag.Add("Description", Description);
		tag.Add("DemandItem", DemandItem.ConvertAll(ItemIO.Save));
		tag.Add("RewardItem", RewardItem.ConvertAll(ItemIO.Save));
	}

	public override void Update()
	{
		base.Update();
		UpdateProgress(new List<Item>(Main.LocalPlayer.inventory));
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
		Dictionary<int, int> demandItemStacks = DemandItem
			.GroupBy(item => item.type)
			.ToDictionary(
				group => group.Key,
				group => group.Sum(item => item.stack));

		// Calculate owned item types and their stack
		Dictionary<int, int> ownedItemStacks = paramItems
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