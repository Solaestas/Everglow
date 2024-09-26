using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LampWood;
[AutoloadEquip(EquipType.Head)]
public class LampWoodHelmet : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 24;
		Item.value = 1340;
		Item.rare = ItemRarityID.White;
		Item.defense = 5;
	}
	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<LampWoodBreastplate>() && legs.type == ModContent.ItemType<LampWoodLeggings>();
	}
	public override void UpdateArmorSet(Player player)
	{
		player.setBonus = "Increases defense damage by 6";
		player.statDefense += 6;
	}
	public override void UpdateEquip(Player player)
	{
	}
	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient<LampWood_Wood>(30);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}
