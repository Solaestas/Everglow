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
		RewardItem.ForEach(i => Main.LocalPlayer.QuickSpawnItemDirect(Main.LocalPlayer.GetSource_Misc($"Everglow.GainItemMission.{Name}"), i));
	}

	public override void Load(TagCompound tag)
	{
		base.Load(tag);
		tag.TryGet("Name", out _name);
		tag.TryGet("DisplayName", out _displayName);
		tag.TryGet("Description", out _description);
		tag.TryGet<long>("TimeMax", out _timeMax);
		DemandItem.Clear();
		if (tag.TryGet<IList<TagCompound>>("DemandItem", out var diTag))
		{
			foreach (var iTag in diTag)
			{
				DemandItem.Add(ItemIO.Load(iTag));
			}
		}
		RewardItem.Clear();
		if (tag.TryGet("RewardItem", out diTag))
		{
			foreach (var iTag in diTag)
			{
				RewardItem.Add(ItemIO.Load(iTag));
			}
		}

		// Load not-loaded texture for required vanilla items (DemandItem, RewardItem)
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

		var param = objs switch
		{
		[List<Item> items] => items,
			_ => null,
		};

		if (param == null)
		{
			return;
		}

		int demandSum = 0, backpackSum = 0;
		Dictionary<int, int> demandItemStack = [];
		foreach (var item in DemandItem)
		{
			if (!demandItemStack.TryAdd(item.type, item.stack))
			{
				demandItemStack[item.type] += item.stack;
			}
			demandSum += item.stack;
		}

		Dictionary<int, int> itemStack = [];
		foreach (var item in param)
		{
			if (!itemStack.TryAdd(item.type, item.stack))
			{
				itemStack[item.type] += item.stack;
			}
			if (demandItemStack.TryGetValue(item.type, out int dis))
			{
				if (itemStack[item.type] > dis)
				{
					itemStack[item.type] = dis;
				}
				backpackSum += itemStack[item.type];
			}
		}
		if (demandSum == 0)
		{
			_progress = 0f;
		}
		else
		{
			_progress = Math.Min(1f, Math.Max(0f, backpackSum / (float)demandSum));
		}
	}
}