using Everglow.Commons.Mechanics.MissionSystem.Abstracts;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Core;

public abstract class MissionObjectiveBase : ITagCompoundEntity
{
	public bool Completed { get; private set; }

	public int ObjectiveID { get; set; }

	public MissionObjectiveBase Next { get; set; }

	public virtual float Progress { get; } = 1f;

	/// <summary>
	/// Objective rewards, different from <see cref="MissionBase.RewardItems"/>
	/// </summary>
	public List<Item> RewardItems { get; } = [];

	public bool HasGivenRewardItems { get; private set; } = false;

	public abstract bool CheckCompletion();

	/// <summary>
	/// Invoked by <see cref="MissionObjectiveData.Add(MissionObjectiveBase)"/>.
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
			if (!HasGivenRewardItems)
			{
				foreach (var item in RewardItems)
				{
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(MissionBase.RewardItemsSourceContext), item, item.stack);
				}

				HasGivenRewardItems = true;
			}

			Completed = true;
		}
	}

	public virtual void ResetProgress() => Completed = false;

	public virtual void Activate(MissionBase sourceMission)
	{
	}

	public virtual void Deactivate()
	{
	}

	public abstract void GetObjectivesText(List<string> lines);

	public virtual void LoadData(TagCompound tag)
	{
		if(tag.TryGet<bool>(nameof(HasGivenRewardItems), out var hasGiven))
		{
			HasGivenRewardItems = hasGiven;
		}
	}

	public virtual void SaveData(TagCompound tag)
	{
		tag.Add(nameof(HasGivenRewardItems), HasGivenRewardItems);
	}
}