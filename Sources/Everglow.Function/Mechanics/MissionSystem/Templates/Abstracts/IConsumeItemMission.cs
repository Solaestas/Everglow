using System.Text;
using Everglow.Commons.Mechanics.MissionSystem.Abstracts;
using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Shared;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Templates.Abstracts;

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

	public IEnumerable<string> GetObjectivesString()
	{
		var objectives = new List<string>();

		foreach (var demand in DemandConsumeItems)
		{
			var progress = $"({demand.Counter}/{demand.Requirement})";
			if (demand.Items.Count > 1)
			{
				var itemString = string.Join(' ', demand.Items.ConvertAll(i => ItemDrawer.Create(i)));
				objectives.Add($"消耗{itemString}合计{demand.Requirement}个 {progress}\n");
			}
			else
			{
				objectives.Add($"消耗{ItemDrawer.Create(demand.Items.First())}{demand.Requirement}个 {progress}\n");
			}
		}

		return objectives;
	}
}