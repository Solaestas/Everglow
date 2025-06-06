using Everglow.Yggdrasil.YggdrasilTown.Items.Materials;
using Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.CyanVine;

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
		Item.noUseGraphic = true;
		Item.noMelee = true;
		Item.shootSpeed = 5f;
		Item.shoot = ModContent.ProjectileType<Projectiles.CyanVineSword_Projectile>();

		Item.value = 3600;
	}

	public override bool CanUseItem(Player player)
	{
		Item.useTime = (int)(18f / player.meleeSpeed);
		Item.useAnimation = (int)(18f / player.meleeSpeed);
		return player.ownedProjectileCounts[Item.shoot] < 1;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.ownedProjectileCounts[Item.shoot] < 1)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
		}
		return false;
	}

	public override bool? UseItem(Player player)
	{
		if (!Main.dedServ)
		{
			SoundEngine.PlaySound(Item.UseSound, player.Center);
		}

		return null;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 12)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 8)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}