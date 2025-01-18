using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Templates;

/// <summary>
/// Represent a mission which contains serveral sub missions
/// </summary>
[Obsolete("This mission template will cause a lot bug", true)]
public abstract class MultipleMission : MissionBase
{
	private int currentIndex = 0;

	public override float Progress => SubMissions.Count != 0
		? SubMissions.Where(sm => sm.CheckComplete()).Count() / (float)SubMissions.Count
		: 1;

	public override MissionIconGroup Icon => null;

	public List<MissionBase> SubMissions { get; } = [];

	public int CurrentIndex => currentIndex;

	public MissionBase CurrentMission => SubMissions[currentIndex];

	public override void Update()
	{
		base.Update();

		// If current is not the last sub mission
		if (currentIndex < SubMissions.Count - 1)
		{
			if (CurrentMission.CheckComplete())
			{
				CurrentMission.PostComplete();
				CurrentMission.PoolType = MissionManager.PoolType.Completed;
				currentIndex++;
			}
		}
		else // If current is the last sub mission
		{
			if (CurrentMission.CheckComplete() && CurrentMission.PoolType != MissionManager.PoolType.Completed)
			{
				CurrentMission.PostComplete();
				CurrentMission.PoolType = MissionManager.PoolType.Completed;
			}
		}

		CurrentMission.Update();
	}

	private string SubMissionTypesSaveKey => nameof(SubMissions) + ".Type";

	public override void SaveData(TagCompound tag)
	{
		base.SaveData(tag);

		tag.Add(nameof(CurrentIndex), currentIndex);

		tag.Add(
			SubMissionTypesSaveKey,
			SubMissions.ConvertAll(m => m.GetType().FullName));
		tag.Add(
			nameof(SubMissions),
			SubMissions.ConvertAll(m =>
			{
				TagCompound t = [];
				m.SaveData(t);
				return t;
			}));
	}

	public override void LoadData(TagCompound tag)
	{
		base.LoadData(tag);

		tag.TryGet(nameof(CurrentIndex), out currentIndex);

		SubMissions.Clear();
		if (tag.TryGet<IList<string>>(SubMissionTypesSaveKey, out var smt) &&
				tag.TryGet<IList<TagCompound>>(nameof(SubMissions), out var sm))
		{
			for (int j = 0; j < smt.Count; j++)
			{
				var mission = (MissionBase)Activator.CreateInstance(Type.GetType(smt[j]));
				mission.LoadData(sm[j]);
				SubMissions.Add(mission);
			}
		}
	}
}