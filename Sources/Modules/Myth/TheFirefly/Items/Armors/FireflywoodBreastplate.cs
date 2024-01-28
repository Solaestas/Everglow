using Terraria.GameContent.Creative;

//TODO:Translate:流萤木板甲
namespace Everglow.Myth.TheFirefly.Items.Armors
{
	[AutoloadEquip(EquipType.Body)]
	public class FireflywoodBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = 6100;
			Item.rare = ItemRarityID.White; 
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.buffImmune[BuffID.ManaSickness] = true; // Was BuffID.OnFire
			player.statManaMax2 += 20;
			player.maxMinions++;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<GlowWood>(50);
			recipe.AddIngredient<BlackStarShrub>(10);
			recipe.AddIngredient<GlowingPedal>(15);
			recipe.AddIngredient<FireflyMoss>(5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
