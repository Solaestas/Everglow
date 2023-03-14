using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.MiscItems.Weapons;

public class ComingGhost : ModItem
{
	public override void SetStaticDefaults()
	{

		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		ItemGlowManager.AutoLoadItemGlow(this);
	}
	public static short GetGlowMask = 0;
	public override void SetDefaults()
	{
		Item.glowMask = ItemGlowManager.GetItemGlow(this);
		Item.damage = 57;
		Item.DamageType = DamageClass.Melee;
		Item.width = 54;
		Item.height = 58;
		Item.useTime = 17;
		Item.useAnimation = 17;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.knockBack = 6;
		Item.value = 20000;
		Item.rare = ItemRarityID.Pink;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Melee.ComingGhost>();
		Item.shootSpeed = 8;
		Item.crit = 8;
	}
	private int l = 0;
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (l % 4 == 0)
			type = ModContent.ProjectileType<Projectiles.Weapon.Melee.ComingGhost>();
		else if (l % 4 == 1)
		{
			type = ModContent.ProjectileType<Projectiles.Weapon.Melee.ComingGhost2>();
		}
		else if (l % 4 == 2)
		{
			type = ModContent.ProjectileType<Projectiles.Weapon.Melee.ComingGhost>();
		}
		else
		{
			type = ModContent.ProjectileType<Projectiles.Weapon.Melee.ComingGhost2>();
		}
		Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
		l++;
		return false;
	}
}
