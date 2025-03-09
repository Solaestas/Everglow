namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class BloodTearCrystalCrown : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 30;
		Item.accessory = true;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 1, silver: 12);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 60 Life
		player.statLifeMax2 += 60;

		// 2. + 1 life regen (if life is more than 84%)
		if (player.statLife > player.statLifeMax * 0.84f)
		{
			player.lifeRegen += 1;
		}
	}
}