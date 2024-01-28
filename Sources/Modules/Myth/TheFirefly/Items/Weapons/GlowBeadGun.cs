using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Items.Weapons;

public class GlowBeadGun : ModItem
{
	//TODO:Translate:萤火连珠炮\n根据蓄力时间打出一连串魔法弹
	public override void SetDefaults()
	{
		
		Item.damage = 34;
		Item.DamageType = DamageClass.Magic;
		Item.mana = 4;
		Item.width = 104;
		Item.height = 38;
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.knockBack = 5f;
		Item.value = Item.sellPrice(0, 5, 0, 0);
		Item.rare = ItemRarityID.LightRed;
		Item.UseSound = SoundID.Item132 with { MaxInstances = 3 };
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.GlowBeadGun>();
		Item.shootSpeed = 8;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.ownedProjectileCounts[type] > 0)
		{
			foreach(Projectile projectile in Main.projectile)
			{
				if (player.ownedProjectileCounts[type] > 1)
				{
					if(projectile.type == type && projectile.owner == player.whoAmI)
					{
						projectile.Kill();
					}
				}
				else
				{
					break;
				}
			}
			return false;
		}		
		return true;
	}
}