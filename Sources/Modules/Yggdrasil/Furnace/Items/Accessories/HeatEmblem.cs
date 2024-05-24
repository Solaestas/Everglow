using Terraria.Enums;

namespace Everglow.Yggdrasil.Furnace.Items.Accessories;

public class HeatEmblem : ModItem
{
	public static readonly double BuffTriggerRate = 0.33d;
	public static readonly int BuffDuration = 300;

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
		Random random = new Random();

		if(random.NextDouble() < HeatEmblem.BuffTriggerRate)
		{
			Player.AddBuff(BuffID.OnFire, HeatEmblem.BuffDuration);
		}

		base.OnHurt(info);
	}
}