using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class MissionBase_New : IMissionPersistence
{
	public static string ObjectivesSaveKey => nameof(Objectives);

	public void LoadData(TagCompound tag)
	{
		if (tag.TryGet<int>(nameof(Time), out var mt))
		{
			Time = mt;
		}

		LoadObjectives(tag, Objectives.AllObjectives);
	}

	public static void LoadObjectives(TagCompound tag, IEnumerable<ObjectiveBase> objectives)
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
		tag.Add(nameof(Time), Time);

		SaveObjectives(tag, Objectives.AllObjectives);
	}

	public static void SaveObjectives(TagCompound tag, IEnumerable<ObjectiveBase> objectives)
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