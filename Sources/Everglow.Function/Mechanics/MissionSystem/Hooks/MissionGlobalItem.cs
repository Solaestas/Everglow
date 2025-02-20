using Everglow.Commons.Mechanics.MissionSystem;

namespace Everglow.Commons.Mechanics.MissionSystem.Hooks;

public class MissionGlobalItem : GlobalItem
{
	public override bool? UseItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			MissionManager.CountUse(item);
		}

		return null;
	}

	public override void OnConsumeItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			MissionManager.CountConsume(item);
		}
	}
}