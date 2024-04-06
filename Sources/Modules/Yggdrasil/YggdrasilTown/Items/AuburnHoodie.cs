using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;
[AutoloadEquip(EquipType.Head)]
public class AuburnHoodie : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 22;
		Item.value = 2500;
		Item.rare = ItemRarityID.White;
		Item.defense = 1;
	}
	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<AuburnBreastplate>() && legs.type == ModContent.ItemType<AuburnBoots>();
	}
	public override void UpdateArmorSet(Player player)
	{
	}
	public override void UpdateEquip(Player player)
	{
		player.minionDamage += 0.08f;
	}
	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		//recipe.AddIngredient<LampWood_Wood>(30);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}
