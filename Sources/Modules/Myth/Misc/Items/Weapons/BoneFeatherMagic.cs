using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.Misc.Items.Weapons;

public class BoneFeatherMagic : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 51;
		Item.DamageType = DamageClass.Magic;
		Item.width = 28;
		Item.height = 30;
		Item.useTime = 17;
		Item.useAnimation = 17;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.knockBack = 2;
		Item.value = 2000;
		Item.rare = ItemRarityID.LightRed;
		Item.UseSound = SoundID.Item71;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Magic.BoneFeather>();
		Item.shootSpeed = 8;
		Item.crit = 8;
		Item.mana = 13;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Vector2 v = velocity;
		for (int k = 0; k < 6; k++)
		{
			Vector2 v2 = v.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(0.9f, 1.1f);
			int u = Projectile.NewProjectile(source, position + velocity * 2f, v2, type, damage, knockback, player.whoAmI, 0f);
			Main.projectile[u].hostile = false;
			Main.projectile[u].friendly = true;
		}
		return false;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient<FeatherMagic>()
			.AddIngredient(ItemID.BoneFeather, 3)
			.AddTile(TileID.CrystalBall)
			.Register();
	}
}
