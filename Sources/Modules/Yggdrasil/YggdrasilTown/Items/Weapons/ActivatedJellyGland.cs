using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class ActivatedJellyGland : ModItem
{
	public override void SetStaticDefaults()
	{
		// TODO: Replace sprite
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 40;
		Item.height = 52;

		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(gold: 1, silver: 20));

		Item.DamageType = DamageClass.Summon;
		Item.damage = 12;
		Item.knockBack = 2.4f;
		Item.mana = 24;
		Item.crit = 0;

		Item.useTime = 27;
		Item.useAnimation = 27;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
		Item.noMelee = true;

		Item.shootSpeed = 1;
		Item.shoot = ModContent.ProjectileType<Projectiles.ActivatedJellyGlandMinion>();
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
			Owner: player.whoAmI,
			ai0: player.ownedProjectileCounts[type] + 1);

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