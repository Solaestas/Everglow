using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class BloodTearCrystalCrown : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 30;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Green2, 11200);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 30 Mana
		player.statLifeMax2 += 60;

		// 2. Add mana regen in some case.
		if(player.statLife > player.statLifeMax * 0.84)
		{
			player.lifeRegen += 1;
		}
	}
}