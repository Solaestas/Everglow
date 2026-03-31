using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class WorldMissionBase : IMissionPersistence
{
	private const string StateKey = nameof(State);
	private const string TimeKey = nameof(Time);
	private const string RewardKey = nameof(RewardClaimed);
	private const string RewardPlayerKey = nameof(RewardClaimedPlayers);
	private const string ObjectivesSaveKey = nameof(Objectives);

	public void LoadData(TagCompound tag)
	{
		if (tag.TryGet<int>(StateKey, out var ms))
		{
			State = (WorldMissionState)ms;
		}

		if (tag.TryGet<int>(TimeKey, out var mt))
		{
			Time = mt;
		}

		if (tag.TryGet<bool>(RewardKey, out var rc))
		{
			RewardClaimed = rc;
		}

		if (tag.TryGet<IList<string>>(RewardPlayerKey, out var rp))
		{
			RewardClaimedPlayers = rp.ToHashSet();
		}

		LoadObjectives(tag, Objectives.AllObjectives);
	}

	public static void LoadObjectives(TagCompound tag, IEnumerable<WorldObjectiveBase> objectives)
	{
		if (tag.TryGet<IList<TagCompound>>(ObjectivesSaveKey, out var oTags))
		{
			foreach (var o in objectives)
			{
				if (oTags.Count <= o.ObjectiveID)
				{
					break;
				}

				o.LoadData(oTags[o.ObjectiveID]);
			}
		}
	}

	public void SaveData(TagCompound tag)
	{
		tag.Add(StateKey, (int)State);
		tag.Add(TimeKey, Time);
		tag.Add(RewardKey, RewardClaimed);
		tag.Add(RewardPlayerKey, RewardClaimedPlayers.ToList());

		SaveObjectives(tag, Objectives.AllObjectives);
	}

	public static void SaveObjectives(TagCompound tag, IEnumerable<WorldObjectiveBase> objectives)
	{
		var oTags = new List<TagCompound>();
		foreach (var o in objectives)
		{
			var ot = new TagCompound();
			o.SaveData(ot);
			oTags.Add(ot);
		}
		tag.Add(ObjectivesSaveKey, oTags);
	}
}