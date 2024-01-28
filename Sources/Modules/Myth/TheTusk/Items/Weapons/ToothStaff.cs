using Terraria.DataStructures;
using Terraria.GameContent.Creative;
namespace Everglow.Myth.TheTusk.Items.Weapons;

public class ToothStaff : ModItem
{
	//TODO:Translate:獠牙召唤杖
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 15;
		Item.width = 52;
		Item.height = 52;
		Item.useTime = 17;
		Item.useAnimation = 17;
		Item.knockBack = 1;
		Item.shootSpeed = 1;
		Item.crit = 8;
		Item.mana = 13;

		Item.staff[Item.type] = true;
		Item.noMelee = true;
		Item.autoReuse = true;
		Item.value = 2054;

		Item.rare = ItemRarityID.Green;
		Item.UseSound = SoundID.Item71;
		Item.DamageType = DamageClass.Summon;
		Item.useStyle = ItemUseStyleID.Swing;

		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.TuskSummon>();
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.numMinions >= player.maxMinions)
			return false;
		player.AddBuff(ModContent.BuffType<Buffs.TuskStaff>(), 18000);
		Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, velocity, type, damage, knockback, player.whoAmI, player.ownedProjectileCounts[type] + 1);
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
