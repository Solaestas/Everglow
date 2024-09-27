using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LampWood;

[AutoloadEquip(EquipType.Body)]
public class LampWoodBreastplate : ModItem
{
	private const int LifeRegenBonus = 1;

	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 18;
		Item.value = Item.sellPrice(silver: 20);
		Item.rare = ItemRarityID.Green;
		Item.defense = 3;
	}

	public override void UpdateEquip(Player player)
	{
		player.lifeRegen += LifeRegenBonus;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient<LampWood_Wood>(50);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}