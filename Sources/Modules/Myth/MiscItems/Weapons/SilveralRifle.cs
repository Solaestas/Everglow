using Terraria.DataStructures;

namespace Everglow.Myth.MiscItems.Weapons
{
	public class SilveralRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			/*DisplayName.SetDefault("Silver Shadow Ct01");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "银影幽豹Ct01");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Серебряная Тень Ct 01");
            Tooltip.SetDefault("5% increased movement speed and damage after hitting enemies");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "命中后移速和伤害提升5%,持续5秒");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "на 5% увеличена скорость передвижения и урон после попадания во врагов");*/
		}

		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Ranged; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
			Item.width = 66;
			Item.height = 24;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
			Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
			Item.knockBack = 0;
			Item.value = 500;
			Item.rare = 1;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 24f; // The speed of the projectile (measured in pixels per frame.)
			Item.useAmmo = AmmoID.Bullet;
			Item.crit = 16; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int f = Projectile.NewProjectile(source, position + velocity * 2 + new Vector2(0, -2), velocity, type, damage, knockback, player.whoAmI, 0);
			return false;
		}
		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6f, 0);
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SilverBar, 12)
				.AddIngredient(ItemID.Ruby, 4)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.TungstenBar, 12)
				.AddIngredient(ItemID.Ruby, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
