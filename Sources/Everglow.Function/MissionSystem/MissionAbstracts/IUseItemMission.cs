using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionAbstracts;

public interface IUseItemMission : IMissionAbstract
{
	public abstract List<ItemRequirement> DemandUseItems { get; init; }

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountUse(Item item)
	{
		foreach (var di in DemandUseItems.Where(x => x.Items.Contains(item.type)))
		{
			di.Count();
		}
	}

	/// <summary>
	/// Calculate mission progress
	/// </summary>
	/// <returns></returns>
	public float CalculateProgress()
	{

		if (DemandUseItems.Count == 0 || DemandUseItems.Select(x => x.Requirement).Sum() == 0)
		{
			return 1f;
		}

		return DemandUseItems.Select(x => x.Progress()).Average();
	}

	public void Load(TagCompound tag)
	{
		tag.TryGet<List<ItemRequirement>>(nameof(DemandUseItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandUseItems)
			{
				demand.Count(
					demandNPCs
						.Where(d => d.Items.Intersect(demand.Items).Any())
						.Sum(x => x.Counter));
			}
		}

		MissionBase.LoadVanillaItemTextures(DemandUseItems.SelectMany(x => x.Items));
	}

	public void Save(TagCompound tag)
	{
		tag.Add(nameof(DemandUseItems), DemandUseItems);
	}
}