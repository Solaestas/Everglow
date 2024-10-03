using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LampWood;

[AutoloadEquip(EquipType.Legs)]
public class LampWoodLeggings : ModItem
{
	private const int MoveSpeedBonus = 5;

	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 16;
		Item.value = Item.sellPrice(silver: 20);
		Item.rare = ItemRarityID.Green;
		Item.defense = 2;
	}

	public override void UpdateEquip(Player player)
	{
		player.moveSpeed += MoveSpeedBonus / 100f;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient<LampWood_Wood>(40);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}