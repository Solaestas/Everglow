using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;
public class CyanVineShortsword : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 10;
		Item.knockBack = 4f;
		Item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
		Item.useAnimation = 12;
		Item.useTime = 12;
		Item.width = 32;
		Item.height = 32;
		Item.UseSound = SoundID.Item1;
		Item.DamageType = DamageClass.MeleeNoSpeed;
		Item.autoReuse = false;
		Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
		Item.noMelee = true; // The projectile will do the damage and not the item

		Item.rare = ItemRarityID.White;
		Item.value = Item.buyPrice(0, 0, 12, 0);

		Item.shoot = ModContent.ProjectileType<CyanVineShortsword_Proj>(); // The projectile is what makes a shortsword work
		Item.shootSpeed = 2.1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
	}

	// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 6)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 4)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
