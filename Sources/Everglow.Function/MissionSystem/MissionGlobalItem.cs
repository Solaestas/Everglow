namespace Everglow.Commons.MissionSystem;

public class MissionGlobalItem : GlobalItem
{
	public override bool? UseItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			Console.WriteLine(item);
		}

		return null;
	}

	public override void OnConsumeItem(Item item, Player player)
	{
		if (player.whoAmI == Main.myPlayer)
		{
			Console.WriteLine(item);
		}
	}
}