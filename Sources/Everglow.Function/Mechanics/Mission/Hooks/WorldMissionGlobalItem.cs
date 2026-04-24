namespace Everglow.Commons.Mechanics.Mission.Hooks;

public class WorldMissionGlobalItem : GlobalItem
{
	public static event Action<Item, Player> OnItemConsumed;

	public override void OnConsumeItem(Item item, Player player)
	{
		OnItemConsumed?.Invoke(item, player);
	}

	public override void OnConsumeAmmo(Item weapon, Item ammo, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			OnItemConsumed?.Invoke(ammo, player);
		}
	}
}