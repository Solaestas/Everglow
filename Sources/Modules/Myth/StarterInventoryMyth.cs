using Everglow.Myth.OmniElementItems;
using static Steamworks.SteamUser;

namespace Everglow.Myth;

public class StarterInventoryMyth : ModPlayer
{
	public ulong SteamID64 = GetSteamID().m_SteamID;
	public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
	{
		//if (mediumCoreDeath)
		//{
		//    return new[]
		//    {
		//        new Item(ItemID.HealingPotion)
		//    };
		//}
		if (SteamID64 is 76561199058565968 /*Omnielement*/)
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