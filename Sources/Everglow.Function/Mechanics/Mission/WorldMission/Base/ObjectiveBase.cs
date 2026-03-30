using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract class ObjectiveBase
{
	public ObjectiveBase()
	{
	}

	public bool Completed { get; private set; }

	public int ObjectiveID { get; set; }

	public ObjectiveBase Next { get; set; }

	public virtual float Progress { get; } = 1f;

	/// <summary>
	/// Objective rewards, different from <see cref="MissionBase_New.RewardItems"/>
	/// </summary>
	public List<Item> RewardItems { get; } = [];

	public bool RewardClaimed { get; private set; } = false;

	public abstract bool CheckCompletion();

	/// <summary>
	/// Invoked by <see cref="MissionObjectiveContainer_New.Add(ObjectiveBase)"/>.
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
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(MissionBase_New.RewardItemsSourceContext), item, item.stack);
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

	public virtual void Activate(MissionBase_New sourceMission)
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