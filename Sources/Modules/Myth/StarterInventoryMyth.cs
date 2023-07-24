using Everglow.Myth.OmniElementItems;
using static Steamworks.SteamUser;

namespace Everglow.Myth;

public class StarterInventoryMyth : ModPlayer
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
		if (Main.LocalPlayer.name is "Omnielement" or "万象元素")
		{
			return new[]
			{
			new Item(ModContent.ItemType<LilyHarp>()),
			};
		}
		return new[]
		{
		new Item(ItemID.CopperCoin, 0), // stack of 0 means you don't start with it. This code is needed.
        };
	}
}