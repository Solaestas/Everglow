using Terraria.GameContent;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

public class KillEnemyMission : MissionBase
{
	private string name = string.Empty;
	private string displayName = string.Empty;
	private string description = string.Empty;
	private float progress = 0f;
	private long timeMax = -1;

	public override string Name => name;

	public override string DisplayName => displayName;

	public override string Description => description;

	public override Texture2D Icon => DemandEnemies.Count > 0 ? TextureAssets.NpcHead[DemandEnemies.First().Value].Value : null;

	public override float Progress => progress;

	public override long TimeMax => timeMax;

	public string SourceContext => $"{nameof(Everglow)}.{nameof(GainItemMission)}.{Name}";

	public Dictionary<int, int> DemandEnemies { get; init; } = [];

	public List<Item> RewardItems { get; init; } = [];

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

	public override void OnComplete()
	{
		base.OnComplete();

		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(SourceContext), item, item.stack);
		}
	}

	public override void Update()
	{
		base.Update();

		UpdateProgress();
	}

	public override void UpdateProgress(params object[] objs)
	{
		if (PoolType != MissionManager.PoolType.Accepted)
		{
			return;
		}

		if (DemandEnemies.Count == 0)
		{
			progress = 1f;
			return;
		}

		var killedEnemies = new Dictionary<int, int>();
		foreach (var type in DemandEnemies.Keys)
		{
			killedEnemies.Add(type, NPC.killCount[type]);
		}

		// The final progress is calculated as the average of the individual progress
		// for each enemy type, where individual progress is the ratio of killed enemies
		// to required enemies (capped at 1 and floored at 0) for that enemy type.
		float IndividualProgress(KeyValuePair<int, int> requiredEnemy)
		{
			var killedCount = killedEnemies.GetValueOrDefault(requiredEnemy.Key, 0);
			var requiredCount = (float)requiredEnemy.Value;
			return Math.Min(1f, Math.Max(0f, killedCount / requiredCount));
		}

		progress = DemandEnemies.Select(IndividualProgress).Average();
	}
}