using Everglow.Yggdrasil.YggdrasilTown.Items.Materials;
using Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.CyanVine;

public class CyanVineShortsword : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 10;
		Item.knockBack = 4f;
		Item.useStyle = ItemUseStyleID.Rapier;
		Item.useAnimation = 12;
		Item.useTime = 12;
		Item.width = 32;
		Item.height = 32;
		Item.UseSound = SoundID.Item1;
		Item.DamageType = DamageClass.MeleeNoSpeed;
		Item.autoReuse = false;
		Item.noUseGraphic = true;
		Item.noMelee = true;

		Item.rare = ItemRarityID.White;
		Item.value = Item.buyPrice(0, 0, 12, 0);

		Item.shoot = ModContent.ProjectileType<CyanVineShortsword_Proj>();
		Item.shootSpeed = 2.1f;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 6)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 4)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}