using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class RingOfMatter : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 12;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Orange3, 100000);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetCritChance(DamageClass.Generic) += 4;
	}
}

public class RingOfMatterPlayer : ModPlayer
{
	public bool HasRingOfMatter = false;
	public float CoinValue = 0;

	public override void ResetEffects()
	{
		HasRingOfMatter = false;
	}
}