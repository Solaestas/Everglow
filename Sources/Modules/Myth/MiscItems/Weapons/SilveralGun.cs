using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
	public class SilveralGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			/*DisplayName.SetDefault("Fox Hunter Fx01");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "银猎之狐Fx01");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Охотник на лис Fx01");
            Tooltip.SetDefault("5% increased movement speed and damage after hitting enemies");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "命中后移速和伤害提升5%,持续5秒");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "на 5% увеличена скорость передвижения и урон после попадания во врагов");*/
		}

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Ranged; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
			Item.width = 54;
			Item.height = 32;
			Item.useTime = 11;
			Item.useAnimation = 11;
			Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
			Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
			Item.knockBack = 0;
			Item.value = 500;
			Item.rare = 1;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 12f; // The speed of the projectile (measured in pixels per frame.)
			Item.useAmmo = AmmoID.Bullet;
			Item.crit = 0; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
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
				.AddIngredient(ItemID.SilverBar, 9)
				.AddIngredient(ItemID.Ruby, 6)
				.AddTile(TileID.Anvils)
				.Register();
			CreateRecipe()
				.AddIngredient(ItemID.TungstenBar, 9)
				.AddIngredient(ItemID.Ruby, 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
