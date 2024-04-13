using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class CyanVineThrowingSpear : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 54;
		Item.height = 108;
		Item.useAnimation = 26;
		Item.useTime = 26;
		Item.knockBack = 4f;
		Item.damage = 13;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.value = 3600;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Ranged;
		Item.channel = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.CyanVineThrowingSpear>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 14)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 26)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		foreach(Projectile proj in Main.projectile)
		{
			if(proj.active)
			{
				if(proj.owner == player.whoAmI)
				{
					if(proj.type == type)
					{
						Projectiles.CyanVineThrowingSpear cvts = proj.ModProjectile as Projectiles.CyanVineThrowingSpear;
						if (cvts != null)
						{
							if(!cvts.Shot)
							{
								return false;
							}
						}
					}
				}
			}
		}
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
}
