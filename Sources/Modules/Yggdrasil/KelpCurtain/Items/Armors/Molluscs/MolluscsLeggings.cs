using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;

[AutoloadEquip(EquipType.Legs)]
public class MolluscsLeggings : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 20;
		Item.value = Item.buyPrice(silver: 60);
		Item.rare = ItemRarityID.Green;
		Item.defense = 4;
	}

	override public void UpdateEquip(Player player)
	{
		player.moveSpeed += 0.08f; // Increases movement speed by 5%.

		// Increases max speed and acceleration by 35% when the player is currently in water.
		player.GetModPlayer<KelpCurtainPlayer>().MolluscsLeggings = true;
	}
}