using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

public class MultipleMission : MissionBase
{
	private string name = string.Empty;
	private string displayName = string.Empty;
	private string description = string.Empty;
	private long timeMax = -1;
	private int currentIndex = 0;

	public override string Name => name;

	public override string DisplayName => displayName;

	public override string Description => description;

	public override float Progress => SubMissions.Count != 0
		? SubMissions.Where(sm => sm.CheckFinish()).Count() / (float)SubMissions.Count
		: 1;

	public override long TimeMax => timeMax;

	public override Texture2D Icon => TextureAssets.MagicPixel.Value;

	public List<MissionBase> SubMissions { get; } = [];

	public int CurrentIndex => currentIndex;

	public MissionBase CurrentMission => SubMissions[currentIndex];

	/// <summary>
	/// Sets the basic information for the mission.
	/// </summary>
	/// <param name="name">The unique name of the mission.</param>
	/// <param name="displayName">The display name of the mission.</param>
	/// <param name="description">A brief description of the mission.</param>
	/// <param name="timeMax">The maximum time allowed to complete the mission, in ticks. Use -1 for no time limit.</param>
	/// <exception cref="ArgumentNullException">Thrown if any of the string parameters are null.</exception>
	public void SetInfo(string name, string displayName, string description, long timeMax = -1)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException(nameof(name), "Mission name cannot be null or empty.");
		}

		if (string.IsNullOrWhiteSpace(displayName))
		{
			throw new ArgumentNullException(nameof(displayName), "Mission display name cannot be null or empty.");
		}

		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentNullException(nameof(description), "Mission description cannot be null or empty.");
		}

		this.name = name;
		this.displayName = displayName;
		this.description = description;
		this.timeMax = timeMax;
	}

	public override void Update()
	{
		base.Update();

		// If current is not the last sub mission
		if (currentIndex < SubMissions.Count - 1)
		{
			if (CurrentMission.CheckFinish())
			{
				CurrentMission.OnCompleteCustom();
				CurrentMission.PoolType = MissionManager.PoolType.Completed;
				currentIndex++;
			}
		}
		else // If current is the last sub mission
		{
			if (CurrentMission.CheckFinish() && CurrentMission.PoolType != MissionManager.PoolType.Completed)
			{
				CurrentMission.OnCompleteCustom();
				CurrentMission.PoolType = MissionManager.PoolType.Completed;
			}
		}

		CurrentMission.Update();
	}

	private string SubMissionTypesSaveKey => nameof(SubMissions) + ".Type";

	public override void Save(TagCompound tag)
	{
		base.Save(tag);
		tag.Add(nameof(TimeMax), TimeMax);
		tag.Add(nameof(Name), Name);
		tag.Add(nameof(DisplayName), DisplayName);
		tag.Add(nameof(Description), Description);
		tag.Add(nameof(CurrentIndex), currentIndex);

		tag.Add(
			SubMissionTypesSaveKey,
			SubMissions.ConvertAll(m => m.GetType().FullName));
		tag.Add(
			nameof(SubMissions),
			SubMissions.ConvertAll(m =>
			{
				TagCompound t = [];
				m.Save(t);
				return t;
			}));
	}

	public override void Load(TagCompound tag)
	{
		tag.TryGet(nameof(TimeMax), out timeMax);
		tag.TryGet(nameof(Name), out name);
		tag.TryGet(nameof(DisplayName), out displayName);
		tag.TryGet(nameof(Description), out description);
		tag.TryGet(nameof(CurrentIndex), out currentIndex);

		SubMissions.Clear();
		if (tag.TryGet<IList<string>>(SubMissionTypesSaveKey, out var smt) &&
				tag.TryGet<IList<TagCompound>>(nameof(SubMissions), out var sm))
		{
			for (int j = 0; j < smt.Count; j++)
			{
				var mission = (MissionBase)Activator.CreateInstance(Type.GetType(smt[j]));
				mission.Load(sm[j]);
				SubMissions.Add(mission);
			}
		}
	}

	public override void UpdateProgress(params object[] objs) => throw new NotImplementedException();
}