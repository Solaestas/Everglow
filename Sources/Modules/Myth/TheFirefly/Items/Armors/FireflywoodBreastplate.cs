using Terraria.GameContent.Creative;

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
			player.buffImmune[BuffID.OnFire] = true;
			player.statManaMax2 += 20;
			player.maxMinions++;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<GlowWood>(44);
			recipe.AddIngredient<GlowingPedal>(22);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
