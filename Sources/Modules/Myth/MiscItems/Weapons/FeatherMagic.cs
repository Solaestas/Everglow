using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.MiscItems.Weapons;

public class FeatherMagic : ModItem
{
	public override void SetStaticDefaults()
	{
		/*DisplayName.SetDefault("Book of Feather");
           //            //             Tooltip.SetDefault("'I love harpies♥'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "'我爱鸟妖♥'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "'Я люблю гарпий♥'");*/
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 12;//26→19→12
		Item.DamageType = DamageClass.Magic; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
		Item.width = 28;
		Item.height = 30;
		Item.useTime = 20;//17→24
		Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
		Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
		Item.knockBack = 1;
		Item.value = 800;
		Item.rare = 3;
		Item.UseSound = SoundID.Item71;
		Item.autoReuse = true;
		Item.shoot = 38; // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
		Item.shootSpeed = 8; // How fast the item shoots the projectile.
		Item.crit = 3; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
		Item.mana = 13; //6→13
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		Vector2 v = velocity;
		for (int k = 0; k < 4; k++)
		{
			Vector2 v2 = v.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.9f, 1.1f);
			int u = Projectile.NewProjectile(source, position + velocity * 2f, v2, type, damage, knockback, player.whoAmI, 0f);
			Main.projectile[u].hostile = false;
			Main.projectile[u].friendly = true;
			Main.projectile[u].penetrate = 2;
		}
		return false;
	}
	// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
}
