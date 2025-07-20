using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Ruin;

public class WoodlandWraithStaff : ModItem
{
	public const int LeftManaCost = 10;
	public const int RightManaCost = 20;
	public const float SpecialProjectileSpeed = 15f;

	public override void SetStaticDefaults()
	{
		Item.staff[Type] = true;
	}

	public override void SetDefaults()
	{
		Item.DamageType = DamageClass.Summon;
		Item.damage = 8;
		Item.knockBack = 1.1f;
		Item.mana = LeftManaCost;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = Item.useAnimation = 20;
		Item.noMelee = true;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 1);

		Item.shoot = ModContent.ProjectileType<WoodlandWraithStaff_FungiBall>();
		Item.shootSpeed = 12f;
	}

	public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
	{
		if (player.altFunctionUse == 2)
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			mult *= RightManaCost / (float)LeftManaCost; // Right click mana cost.
		}
		else
		{
			Item.useStyle = ItemUseStyleID.Swing;
		}
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse != 2)
		{
			// Left click: Summon minion.
			if (player.slotsMinions >= player.maxMinions)
			{
				return false;
			}
			Vector2 summonVec = Main.MouseWorld - player.Center;
			if (summonVec.Length() > 150)
			{
				summonVec = summonVec.NormalizeSafe() * 150f;
			}
			player.AddBuff(ModContent.BuffType<WoodlandWraithStaffBuff>(), 2);
			Projectile.NewProjectile(source, player.Center + summonVec, velocity, type, damage, knockback, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item44, position);
		}
		else
		{
			// Right click: Shoot a white projectile that can create a domain, where the attack of item's minions will be enhanced.
			Vector2 initialVelo = Vector2.Normalize(Main.MouseWorld - player.Center) * SpecialProjectileSpeed;
			Projectile.NewProjectile(source, position + velocity * 3, initialVelo, ModContent.ProjectileType<WoodlandWraithStaff_SporeBeam>(), damage, knockback, player.whoAmI, 0f, 0f);
			SoundEngine.PlaySound(SoundID.Item20, position);
		}

		return false;
	}

	public override bool AltFunctionUse(Player player) => true;
}