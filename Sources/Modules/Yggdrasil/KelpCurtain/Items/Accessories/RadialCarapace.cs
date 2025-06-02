namespace Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

public class RadialCarapace : ModItem
{
	public override string Texture => Commons.ModAsset.White_Mod;

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;

		Item.defense = 4;

		Item.accessory = true;

		Item.value = Item.buyPrice(gold: 1);
		Item.rare = ItemRarityID.Green;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		if (player.wet)
		{
			// Increase movement speed by 35%, when player is currently in water.
			player.moveSpeed += 0.35f;
		}
		else
		{
			// Increase defense by 4 and decrease movement speed by 5%, when player is not in water.
			player.statDefense += 4;
			player.moveSpeed -= 0.05f;
		}
	}
}