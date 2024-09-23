using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class CelesteStoneWaistPendant : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 44;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Green2, 9200);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 30 Mana
		player.statManaMax2 += 30;

		// 2. Add mana regen in some case.
		if(player.statMana > player.statManaMax * 0.4 && player.statMana < player.statManaMax * 0.6)
		{
			player.manaRegen += 1;
		}
	}
}