namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionGlobalItem : GlobalItem
{
	[Obsolete("This event is broken, don't use it.", true)]
	public static event Action<Item> OnUseItemEvent;

	public static event Action<Item> OnConsumeItemEvent;

	public override bool? UseItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			OnUseItemEvent?.Invoke(item);
		}

		return null;
	}

	public override void OnConsumeItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			OnConsumeItemEvent?.Invoke(item);
		}
	}

	public static void InvokeOnConsumeItemEvent(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			OnConsumeItemEvent?.Invoke(item);
		}
	}
}