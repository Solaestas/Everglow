namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons
{
	public class CyanVineHammer : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 38;
			Item.height = 42;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.knockBack = 4.5f;
			Item.damage = 9;
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Melee;

			Item.value = 3600;

			Item.hammer = 68;
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
