using Everglow.Yggdrasil.YggdrasilTown.Items;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons
{
	public class CyanVineSword : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 52;
			Item.height = 56;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.knockBack = 3f;
			Item.damage = 15;
			Item.rare = ItemRarityID.White;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;

			Item.value = 3600;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<CyanVineBar>(), 12)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
