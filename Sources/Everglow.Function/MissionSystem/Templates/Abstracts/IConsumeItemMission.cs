using System.Text;
using Everglow.Commons.MissionSystem.Abstracts;
using Everglow.Commons.MissionSystem.Core;
using Everglow.Commons.MissionSystem.Shared;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates.Abstracts;

public interface IConsumeItemMission : IMissionObjective
{
	public abstract List<CountItemRequirement> DemandConsumeItems { get; init; }

	/// <summary>
	/// Count kill for each demand group
	/// </summary>
	/// <param name="type">The type of NPC</param>
	/// <param name="count">The count of kill. Default to 1.</param>
	public void CountConsume(Item item)
	{
		foreach (var di in DemandConsumeItems.Where(x => x.Items.Contains(item.type)))
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
		if (DemandConsumeItems.Count == 0 || DemandConsumeItems.Select(x => x.Requirement).Sum() == 0)
		{
			return 1f;
		}

		return DemandConsumeItems.Select(x => x.Progress()).Average();
	}

	public void Load(TagCompound tag)
	{
		tag.TryGet<List<CountItemRequirement>>(nameof(DemandConsumeItems), out var demandNPCs);
		if (demandNPCs != null && demandNPCs.Count != 0)
		{
			foreach (var demand in DemandConsumeItems)
			{
				demand.Count(
					demandNPCs
						.Where(d => d.Items.Intersect(demand.Items).Any())
						.Sum(x => x.Counter));
			}
		}

		MissionBase.LoadVanillaItemTextures(DemandConsumeItems.SelectMany(x => x.Items));
	}

	public void Save(TagCompound tag)
	{
		tag.Add(nameof(DemandConsumeItems), DemandConsumeItems);
	}

	public string GetObjectivesString()
	{
		var objectives = new StringBuilder();

		foreach (var demand in DemandConsumeItems)
		{
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => $"[ItemDrawer,Type='{i}']"));
				objectives.Append($"消耗{itemString}合计{demand.Requirement}个\n");
			}
			else
			{
				objectives.Append($"消耗[ItemDrawer,Type='{demand.Items.First()}']{demand.Requirement}个\n");
			}
		}

		return objectives.ToString();
	}
}