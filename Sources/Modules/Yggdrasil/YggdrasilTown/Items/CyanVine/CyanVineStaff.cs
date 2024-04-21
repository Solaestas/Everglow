using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.CyanVine;

public class CyanVineStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 60;
		Item.height = 68;
		Item.useAnimation = 26;
		Item.useTime = 26;
		Item.knockBack = 4f;
		Item.damage = 13;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.value = 4000;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 7;
		Item.channel = true;

		Item.noMelee = true;
		Item.noUseGraphic = true;


		Item.shoot = ModContent.ProjectileType<CyanVineStaff_proj>();
		Item.shootSpeed = 12;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 16)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 13)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
	public override bool CanUseItem(Player player)
	{
		return player.ownedProjectileCounts[ModContent.ProjectileType<CyanVineStaff_proj>()] == 0;
	}
}
