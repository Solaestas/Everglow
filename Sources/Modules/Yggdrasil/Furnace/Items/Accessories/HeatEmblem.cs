using Terraria.Enums;

namespace Everglow.Yggdrasil.Furnace.Items.Accessories;

public class HeatEmblem : ModItem
{
	public const float BuffTriggerRate = 0.33f;
	public const int BuffDuration = 300;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 1, silver: 50));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 2/s Life Regeneration
		player.lifeRegen += 4;

		// 2. Gain "On Fire" debuff when get hurt
		player.GetModPlayer<HeatEmblemPlayer>();
	}
}

internal class HeatEmblemPlayer : ModPlayer
{
	public override void OnHurt(Player.HurtInfo info)
	{
		if (Main.rand.NextFloat() < HeatEmblem.BuffTriggerRate)
		{
			Player.AddBuff(BuffID.OnFire, HeatEmblem.BuffDuration);
		}
	}
}