using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class RingOfMatter : ModItem
{
	private float Bonus { get; set; } = 0;

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 12;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.sellPrice(gold: 10));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		if (player.GetModPlayer<RingOfMatterPlayer>().HasNotEquipped)
		{
			int goldCoinNum = player.CountItem(ItemID.GoldCoin);
			int platinumCoinNum = player.CountItem(ItemID.PlatinumCoin);
			Bonus = (goldCoinNum + platinumCoinNum * 100) / 50;
			if (Bonus > 10)
			{
				Bonus = 10;
			}
			Bonus /= 100;
		}

		player.GetDamage(DamageClass.Generic) += Bonus;
		player.statDefense *= 1 + Bonus;
		player.statLifeMax2 -= (int)(player.statLifeMax2 * (2 * Bonus));

		player.GetModPlayer<RingOfMatterPlayer>().HasRingOfMatter = true;
	}
}

public class RingOfMatterPlayer : ModPlayer
{
	public bool HasNotEquipped { get; private set; } = false;

	public bool HasRingOfMatter { get; set; } = false;

	public override void ResetEffects()
	{
		HasNotEquipped = HasRingOfMatter ? false : true;
		HasRingOfMatter = false;
	}
}