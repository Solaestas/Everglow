using Everglow.Commons.Mechanics.MissionSystem.Abstracts;
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

	public abstract bool CheckCompletion();

	/// <summary>
	/// Invoked by <see cref="MissionObjectiveData.Add(MissionObjectiveBase)"/>.
	/// <para/>In this hook you can do initializations, like load vanilla textures.
	/// </summary>
	public virtual void OnInitialize()
	{
		MissionBase.LoadVanillaItemTextures(RewardItems.Select(x => x.type));
	}

	/// <summary>
	/// Complete the objective.
	/// </summary>
	public virtual void Complete()
	{
		if (!Completed)
		{
			foreach (var item in RewardItems)
			{
				Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(MissionBase.RewardItemsSourceContext), item, item.stack);
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
	}

	public virtual void SaveData(TagCompound tag)
	{
	}
}