using Terraria.GameContent.Creative;
//TODO:Translate:流萤木护胫
namespace Everglow.Myth.TheFirefly.Items.Armors
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class FireflywoodLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 26;
			Item.value = 5200;
			Item.rare = ItemRarityID.White;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<GlowWood>(40);
			recipe.AddIngredient<GlowingPedal>(5);
			recipe.AddIngredient<FireflyMoss>(10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
