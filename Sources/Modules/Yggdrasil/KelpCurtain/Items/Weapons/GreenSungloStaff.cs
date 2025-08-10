using Everglow.Yggdrasil.KelpCurtain.Projectiles.Magic;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons;

public class GreenSungloStaff : ModItem
{

	public override void SetStaticDefaults()
	{
		Item.staff[Type] = true;
		base.SetStaticDefaults();
	}

	public override void SetDefaults()
	{
		Item.width = 32;
		Item.height = 32;

		Item.DamageType = DamageClass.Magic;
		Item.damage = 28;
		Item.knockBack = 2f;
		Item.mana = 12;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item20;
		Item.useTime = Item.useAnimation = 20;
		Item.noMelee = true;
		Item.autoReuse = false;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.sellPrice(0, 2, 0, 0);

		Item.shoot = ModContent.ProjectileType<DevilHeartStaff_proj>();
		Item.shootSpeed = 8;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return true;
	}

	public override void HoldItem(Player player)
	{
		player.ListenMouseWorld();

		bool hasTarget = player.itemAnimation > 0;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.owner == player.whoAmI)
				{
					if (proj.type == ModContent.ProjectileType<GreenSungloShield>())
					{
						GreenSungloShield p= proj.ModProjectile as GreenSungloShield;
						if (p != null)
						{
							hasTarget = true;
							break;
						}
					}
				}
			}
		}
		if (!hasTarget)
		{
			Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.zeroVector, ModContent.ProjectileType<GreenSungloShield>(), Item.damage, Item.knockBack, player.whoAmI);
		}
		base.HoldItem(player);
	}

	
}