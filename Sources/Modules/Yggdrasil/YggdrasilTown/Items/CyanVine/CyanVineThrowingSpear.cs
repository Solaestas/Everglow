using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;

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
		Item.shoot = ModContent.ProjectileType<CyanVineThrowingSpear_Proj>();
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
		//foreach(Projectile proj in Main.projectile)
		//{
		//	if(proj.active)
		//	{
		//		if(proj.owner == player.whoAmI)
		//		{
		//			if(proj.type == type)
		//			{
		//				Projectiles.CyanVineThrowingSpear cvts = proj.ModProjectile as Projectiles.CyanVineThrowingSpear;
		//				if (cvts != null)
		//				{
		//					if(!cvts.Shot)
		//					{
		//						return false;
		//					}
		//				}
		//			}
		//		}
		//	}
		//}
		return false;
	}
	public override void HoldItem(Player player)
	{
		bool hasTarget = player.itemAnimation > 0;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.owner == player.whoAmI)
				{
					if (proj.type == ModContent.ProjectileType<Projectiles.CyanVineThrowingSpear_Proj>())
					{
						Projectiles.CyanVineThrowingSpear_Proj cvts = proj.ModProjectile as Projectiles.CyanVineThrowingSpear_Proj;
						if (cvts != null)
						{
							if (!cvts.Shot)
							{
								hasTarget = true;
							}
						}
					}
				}
			}
		}
		if(!hasTarget && !player.controlUseItem)
		{
			Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.zeroVector, ModContent.ProjectileType<Projectiles.CyanVineThrowingSpear_Proj>(), Item.damage, Item.knockBack, player.whoAmI);
		}
		base.HoldItem(player);
	}
}
