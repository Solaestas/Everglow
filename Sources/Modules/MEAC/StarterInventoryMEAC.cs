using Everglow.MEAC.Items;
using static Steamworks.SteamUser;

namespace Everglow.Myth;

public class StarterInventoryMEAC : ModPlayer
{
	public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
	{
		//if (mediumCoreDeath)
		//{
		//    return new[]
		//    {
		//        new Item(ItemID.HealingPotion)
		//    };
		//}
		if (Main.LocalPlayer.name is "FelixYang777" or "Felix Yang")
		{
			return new[]
			{
			new Item(ModContent.ItemType<VortexVanquisherItem>()),
			};
		}
		return new[]
		{
		new Item(ItemID.CopperCoin, 0), // stack of 0 means you don't start with it. This code is needed.
        };
	}
}