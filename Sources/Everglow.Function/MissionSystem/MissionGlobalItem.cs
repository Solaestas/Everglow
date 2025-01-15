namespace Everglow.Commons.MissionSystem;

public class MissionGlobalItem : GlobalItem
{
	public override bool? UseItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			MissionManager.Instance.CountUse(item);
		}

		return null;
	}

	public override void OnConsumeItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			MissionManager.Instance.CountConsume(item);
		}
	}
}