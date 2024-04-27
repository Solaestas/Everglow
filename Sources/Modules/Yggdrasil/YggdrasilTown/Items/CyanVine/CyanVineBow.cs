using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;

public class CyanVineBow : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 20;
		Item.height = 34;
		Item.rare = ItemRarityID.White;
		Item.noMelee = true;
		Item.DamageType = DamageClass.Ranged;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.value = 1200;
		Item.useTime = 26;
		Item.useAnimation = 26;
		Item.damage = 10;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.shoot = ProjectileID.WoodenArrowFriendly;
		Item.shootSpeed = 12f;
		Item.useAmmo = AmmoID.Arrow;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
	public override Vector2? HoldoutOffset()
	{
		return new Vector2(0, 0);
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 12)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 6)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
