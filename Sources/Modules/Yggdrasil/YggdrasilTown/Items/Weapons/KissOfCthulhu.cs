using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class KissOfCthulhu : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 94;
		Item.height = 32;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 21;
		Item.knockBack = 1.8f;
		Item.crit = 4;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 22;
		Item.autoReuse = false;
		Item.noMelee = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 1, silver: 70);

		Item.shoot = ProjectileID.Bullet;
		Item.shootSpeed = 12;
		Item.useAmmo = AmmoID.Bullet;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		position += new Vector2((Item.width - 40) * Item.scale, 0).RotatedBy(velocity.ToRotation());

		// Shoot 2-4 normal bullet once
		int projNum = Main.rand.Next(2, 5);
		for (int i = 0; i < projNum; i++)
		{
			var projVelocity = velocity.RotatedBy((projNum / 2f - i) / 3f);
			Projectile.NewProjectile(source, position, projVelocity, type, damage, knockback, player.whoAmI);
		}
		SoundEngine.PlaySound(SoundID.Item38);

		// The weapon has 1/3 chance to shoot a special projectile
		if (Main.rand.NextBool(3))
		{
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<KissOfCthulhu_Projectile>(), 1, knockback, player.whoAmI);
		}

		return false;
	}

	public override Vector2? HoldoutOffset() => new Vector2(-26, -2);

	public override Vector2? HoldoutOrigin() => new Vector2(0, 0);
}