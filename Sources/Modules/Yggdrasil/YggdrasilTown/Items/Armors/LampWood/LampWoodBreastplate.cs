using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;
using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LampWood
{
	[AutoloadEquip(EquipType.Body)]
	public class LampWoodBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 18;
			Item.value = 1562;
			Item.rare = ItemRarityID.White;
			Item.defense = 7;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<LampWood_Wood>(50);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
