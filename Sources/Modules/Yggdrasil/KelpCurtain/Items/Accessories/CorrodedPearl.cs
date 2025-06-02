namespace Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

public class CorrodedPearl : ModItem
{
	public override string Texture => Commons.ModAsset.Point_Mod;

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;

		Item.accessory = true;

		Item.value = Item.sellPrice(gold: 1);
		Item.rare = ItemRarityID.Green;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.GetDamage<SummonDamageClass>() += 0.05f;
		player.breathEffectiveness += 0.5f;
		if (player.wet)
		{
			player.moveSpeed += 0.1f;
		}
	}
}