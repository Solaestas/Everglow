using Everglow.Commons.Mechanics.Quest.PlayerSide.Structure;
using Everglow.Commons.Mechanics.Quest.Presentation.Icons;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Quest.PlayerSide.Abstractions;

public abstract class PlayerObjectiveBase : ITagCompoundEntity
{
	public bool Completed { get; private set; }

	public int ObjectiveID { get; set; }

	public virtual string Description => string.Empty;

	public virtual float Progress { get; } = 1f;

	/// <summary>
	/// Objective rewards, different from <see cref="PlayerQuestBase.RewardItems"/>
	/// </summary>
	public List<Item> RewardItems { get; } = [];

	public bool HasGivenRewardItems { get; private set; } = false;

	public abstract bool CheckCompletion();

	/// <summary>
	/// Invoked by <see cref="PlayerObjectiveContainer.Add(PlayerObjectiveBase)"/>.
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
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(PlayerQuestBase.RewardItemsSourceContext), item, item.stack);
				}

				HasGivenRewardItems = true;
			}

			Completed = true;
		}
	}

	public virtual void ResetProgress() => Completed = false;

	/// <summary>
	/// Restores completion state saved by the PlayerSide structural node.
	/// Objective-specific persistence remains owned by derived objectives.
	/// </summary>
	internal void RestoreStructuralCompletionState(bool completed) => Completed = completed;

	public virtual void Activate(PlayerQuestBase sourceQuest)
	{
	}

	public virtual void Deactivate()
	{
	}

	public abstract void GetObjectivesIcon(QuestIconGroup iconGroup);

	public abstract string GetObjectiveText();

	public virtual void LoadData(TagCompound tag)
	{
		if (tag.TryGet<bool>(nameof(HasGivenRewardItems), out var hasGiven))
		{
			HasGivenRewardItems = hasGiven;
		}
	}

	public virtual void SaveData(TagCompound tag)
	{
		tag.Add(nameof(HasGivenRewardItems), HasGivenRewardItems);
	}
}
