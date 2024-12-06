using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class ActivatedJellyGland : ModItem
{
	public const int MaxMinionAmount = 4;

	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 40;
		Item.height = 52;

		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(platinum: 0, gold: 1, silver: 20));

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

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.numMinions >= player.maxMinions)
		{
			Main.PrintTimedMessage("No available minion slots");
			return false;
		}
		else if (player.ownedProjectileCounts[type] != 0)
		{
			Main.PrintTimedMessage("Minions already exists");
			return false;
		}

		position = Main.MouseWorld;
		int summonAmount = Math.Min(MaxMinionAmount, player.maxMinions - player.numMinions);
		if (summonAmount == 1)
		{
			Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, damage, knockback, Owner: player.whoAmI, ai0: player.ownedProjectileCounts[type] + 1);
		}
		else
		{
			int summonRadiusOffset = 40 + summonAmount * 5;
			for (int i = 0; i < summonAmount; i++)
			{
				var positionOffset = new Vector2(summonRadiusOffset * MathF.Cos(MathF.PI * 2 * i / summonAmount), summonRadiusOffset * MathF.Sin(MathF.PI * 2 * i / summonAmount));
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), position + positionOffset, velocity.Length() * positionOffset.NormalizeSafe(), type, damage, knockback, Owner: player.whoAmI, ai0: player.ownedProjectileCounts[type] + 1);
			}
		}
		return false;
	}
}