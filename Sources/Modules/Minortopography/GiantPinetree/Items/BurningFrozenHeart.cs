using Terraria.Localization;

namespace Everglow.Minortopography.GiantPinetree.Items;

public class BurningFrozenHeart : ModItem
{
	public static readonly int ResourceBoost = 100;

	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ResourceBoost);

	public override void SetDefaults()
	{
		Item.width = 26;
		Item.height = 52;
		Item.value = 4090;
		Item.accessory = true;
		Item.rare = ItemRarityID.Blue;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		Lighting.AddLight(player.Center, 0, 0.4f, 0.8f);
		player.buffImmune[BuffID.Frostburn] = true;
		player.buffImmune[BuffID.Frostburn2] = true;
		player.buffImmune[BuffID.Frozen] = true;
		player.buffImmune[BuffID.Chilled] = true;
	}
}
