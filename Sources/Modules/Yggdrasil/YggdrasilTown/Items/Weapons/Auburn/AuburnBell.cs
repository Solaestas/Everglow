using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.Auburn;

public class AuburnBell : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 40;
		Item.height = 52;

		Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(silver: 10));

		Item.DamageType = DamageClass.Summon;
		Item.damage = 14;
		Item.knockBack = 0.5f;
		Item.mana = 18;
		Item.crit = 0;

		Item.useTime = 15;
		Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.noMelee = true;

		Item.shootSpeed = 1;
		Item.shoot = ModContent.ProjectileType<Projectiles.AuburnBellSummon>();
	}

	public override bool Shoot(
		Player player,
		EntitySource_ItemUse_WithAmmo source,
		Vector2 position,
		Vector2 velocity,
		int type,
		int damage,
		float knockback)
	{
		if (player.numMinions >= player.maxMinions)
		{
			return false;
		}

		player.AddBuff(ModContent.BuffType<Buffs.AuburnBell>(), 18000);
		Projectile.NewProjectile(
			player.GetSource_ItemUse(Item),
			position,
			velocity,
			type,
			damage,
			knockback,
			player.whoAmI,
			player.ownedProjectileCounts[type] + 1);

		int ai0 = 1;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.type == type)
			{
				if (proj.owner == player.whoAmI)
				{
					proj.ai[0] = ai0;
					ai0++;
				}
			}
		}
		return false;
	}
}