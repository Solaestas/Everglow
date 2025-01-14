namespace Everglow.Commons.MissionSystem.MissionAbstracts;

public interface IRewardItemMission
{
	public List<Item> RewardItems { get; }

	public static string SourceContext => typeof(IRewardItemMission).FullName;

	public void GiveReward()
	{
		foreach (var item in RewardItems)
		{
			Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(SourceContext), item, item.stack);
		}
	}
}