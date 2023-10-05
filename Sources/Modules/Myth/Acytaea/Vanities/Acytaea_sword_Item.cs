using Everglow.Myth.Misc.Projectiles.Weapon.Melee.Hepuyuan;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.Acytaea.Vanities;

public class Acytaea_sword_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.rare = ItemRarityID.Purple;
		Item.value = 999999;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useAnimation = 18;
		Item.useTime = 18;
		Item.autoReuse = true;
		Item.damage = 120;
		Item.crit = 22;
		Item.knockBack = 6.5f;
		Item.noUseGraphic = true;
		Item.DamageType = DamageClass.Melee;
		Item.noMelee = true;
		Item.shootSpeed = 5f;
		Item.shoot = ModContent.ProjectileType<Projectiles.AcytaeaSword_projectile>();
	}
	public override bool CanUseItem(Player player)
	{
		Item.useTime = (int)(18f / player.meleeSpeed);
		Item.useAnimation = (int)(18f / player.meleeSpeed);
		return player.ownedProjectileCounts[Item.shoot] < 1;
	}
	bool CanDown;
	public override void HoldItemFrame(Player player)
	{
		if (player.mount.Active)
		{
			CanDown = false;
			return;
		}
		for (int h = 0; h < 21; h++)
		{
			Vector2 pos = player.Center + new Vector2(0, h * 16 * player.gravDir);
			Vector2 coord = pos / 16f;
			Tile tile = Main.tile[(int)coord.X, (int)coord.Y];
			if (TileID.Sets.Platforms[tile.TileType] || Collision.SolidCollision(pos, 1, 1))
			{
				CanDown = false;
				return;
			}
		}
		for (int h = 21; h < 120; h++)
		{
			Vector2 pos = player.Center + new Vector2(0, h * 16 * player.gravDir);
			Vector2 coord = pos / 16f;
			Tile tile = Main.tile[(int)coord.X, (int)coord.Y];
			if (TileID.Sets.Platforms[tile.TileType] || Collision.SolidCollision(pos, 1, 1))
			{
				CanDown = true;
				return;
			}
		}
		CanDown = false;
		base.HoldItemFrame(player);
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
			SoundEngine.PlaySound(Item.UseSound, player.Center);

		return null;
	}
}
