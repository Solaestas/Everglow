namespace Everglow.Commons.Mechanics.Mission.Hooks;

public class WorldMissionGlobalItem : GlobalItem
{
	public static event Action<int> OnItemConsumed;

	public override void OnConsumeItem(Item item, Player player)
	{
		OnItemConsumed?.Invoke(item.type);
	}

	public override void OnConsumeAmmo(Item weapon, Item ammo, Player player)
	{
		OnItemConsumed?.Invoke(ammo.type);
	}
}