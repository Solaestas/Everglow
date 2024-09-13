using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Auburn
{
	[AutoloadEquip(EquipType.Legs)]
	public class AuburnBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 12;
			Item.value = 2500;
			Item.rare = ItemRarityID.White;
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.08f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			//recipe.AddIngredient<LampWood_Wood>(40);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
