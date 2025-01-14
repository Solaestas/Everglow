using Everglow.Commons.MissionSystem.MissionAbstracts;
using Everglow.Commons.MissionSystem.MissionIcons;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to obtain a specified item or a quantity of items.
/// </summary>
public abstract class GainItemMission : MissionBase
{

	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => new MissionIconGroup(
		[
			ItemMissionIcon.Create(DemandItems.Count > 0
				? DemandItems.First().Items.First()
				: 1)
		]);

	public string SourceContext => $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";

	/// <summary>
	/// Determine if the demand items will be consumed on mission complete.
	/// </summary>
	public bool Consume { get; set; } = false;

	public abstract List<GainItemRequirement> DemandItems { get; }

	public abstract List<Item> RewardItems { get; }

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

		LoadVanillaItemTextures(
			DemandItems.SelectMany(x => x.Items)
			.Concat(RewardItems.Select(x => x.type)));
	}

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);
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
			progress = 1f;
		}
		else
		{
			progress = DemandItems.Select(x => x.Progress(paramItems)).Average();
		}
	}
}