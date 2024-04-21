using Everglow.Yggdrasil.Common.Blocks;
using Everglow.Yggdrasil.YggdrasilTown.Items;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.CyanVine;

public class CyanAmberStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.width = 56;
		Item.height = 56;
		Item.useAnimation = 23;
		Item.useTime = 23;
		Item.knockBack = 2f;
		Item.damage = 25;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.value = 4000;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 12;
		Item.channel = true;

		Item.noMelee = true;
		Item.noUseGraphic = true;


		Item.shoot = ModContent.ProjectileType<CyanVineStaff_proj>();
		Item.shootSpeed = 12;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineStaff>())
			.AddIngredient(ModContent.ItemType<YggdrasilAmber>(), 3)
			.AddIngredient(ModContent.ItemType<YggdrasilGrayRock_Item>(), 20)
			.AddTile(TileID.Anvils)
			.Register();
	}
	public override bool CanUseItem(Player player)
	{
		return player.ownedProjectileCounts[ModContent.ProjectileType<CyanVineStaff_proj>()] == 0;
	}
}
