using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.MidnightBayou;

public class EvilMusicRemnant : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 46;
		Item.height = 44;

		Item.DamageType = DamageClass.Summon;
		Item.damage = 25;
		Item.knockBack = 1.1f;

		Item.rare = ItemRarityID.Green;
		Item.value = 14000;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useAnimation = Item.useTime = 21;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.noMelee = true;

		Item.shoot = ModContent.ProjectileType<EvilMusicRemnant_Projectile>();
		Item.shootSpeed = 6;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		// Player.slotsMinions remain 0 here, so we can only calculate it manually
		var projNum = (int)(player.maxMinions - player.GetSlotsMinions()) + 2;
		for (int i = 0; i < projNum; i++)
		{
			var projectile = Projectile.NewProjectileDirect(source, position + velocity, velocity.RotatedBy(Main.rand.NextFloat(-1, 1)), type, damage, knockback, player.whoAmI);
			projectile.originalDamage = Item.damage;
		}

		return false;
	}
}