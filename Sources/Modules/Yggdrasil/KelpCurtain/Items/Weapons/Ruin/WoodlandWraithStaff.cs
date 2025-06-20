using Everglow.Commons.FeatureFlags;
using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;
using Everglow.Yggdrasil.KelpCurtain.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Ruin;

public class WoodlandWraithStaff : ModItem
{
	public override void SetDefaults()
	{
		Item.DamageType = DamageClass.Summon;
		Item.damage = 8;
		Item.mana = 10;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = Item.useAnimation = 20;
		Item.noMelee = true;
		Item.UseSound = SoundID.Item1;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 1);

		Item.shoot = ModContent.ProjectileType<WoodlandWraithStaff_FungiBall>();
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse == 2)
		{
			Vector2 vel = Main.MouseWorld - player.Center;
			vel = vel.NormalizeSafe() * 15f;
			Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<WoodlandWraithStaff_SporeBeam>(), damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
		else
		{
			if (player.numMinions >= player.maxMinions)
			{
				return false;
			}

			player.AddBuff(ModContent.BuffType<WoodlandWraithStaffBuff>(), 2);
			Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
		}

		return false;
	}

	public override void HoldItem(Player player)
	{
		base.HoldItem(player);
	}
}