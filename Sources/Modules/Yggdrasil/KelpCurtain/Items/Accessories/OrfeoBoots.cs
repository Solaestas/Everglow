namespace Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

public class OrfeoBoots : ModItem
{
	public override string Texture => Commons.ModAsset.White_Mod;

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;

		Item.accessory = true;

		Item.value = Item.buyPrice(gold: 10);
		Item.rare = ItemRarityID.Orange;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		player.moveSpeed += 0.12f; // Increases movement speed by 12%.
		player.accRunSpeed = 6f; // Enable running, same as Hermes Boots.

		if (Main.npc.Any(n => n.active && Vector2.Distance(n.Center, player.Center) < 1000))
		{
			player.accRunSpeed = 6.75f; // Enable running, same as Lightning Boots.
			player.jumpSpeedBoost += 1f; // 20% increased jump speed
		}
	}
}