using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract class WorldObjectiveBase
{
	public WorldObjectiveBase()
	{
	}

	public bool Completed { get; private set; }

	public int ObjectiveID { get; set; }

	public WorldObjectiveBase Next { get; set; }

	public virtual float Progress { get; } = 1f;

	/// <summary>
	/// Objective rewards, different from <see cref="WorldMissionBase.RewardItems"/>
	/// </summary>
	public List<Item> RewardItems { get; } = [];

	public bool RewardClaimed { get; private set; } = false;

	public abstract bool CheckCompletion();

	/// <summary>
	/// Invoked by <see cref="WorldMissionObjectiveContainer.Add(WorldObjectiveBase)"/>.
	/// <para/>In this hook you can do initializations, like load vanilla textures.
	/// </summary>
	public virtual void OnInitialize()
	{
		AssetUtils.LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	/// <summary>
	/// Update inside the objective
	/// </summary>
	public virtual void Update()
	{
	}

	/// <summary>
	/// Complete the objective.
	/// </summary>
	public virtual void Complete()
	{
		if (!Completed)
		{
			if (!RewardClaimed)
			{
				foreach (var item in RewardItems)
				{
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(WorldMissionBase.RewardItemsSourceContext), item, item.stack);
				}

				RewardClaimed = true;
			}

			Completed = true;
		}
	}

	public virtual void ResetProgress()
	{
		Completed = false;
	}

	public virtual void Activate(WorldMissionBase sourceMission)
	{
	}

	public virtual void Deactivate()
	{
	}

	public abstract void GetObjectivesText(List<string> lines);

	public virtual void LoadData(TagCompound tag)
	{
		if (tag.TryGet<bool>(nameof(RewardClaimed), out var hasGiven))
		{
			RewardClaimed = hasGiven;
		}
	}

	public virtual void SaveData(TagCompound tag)
	{
		tag.Add(nameof(RewardClaimed), RewardClaimed);
	}
}