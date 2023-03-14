using Terraria.DataStructures;

namespace Everglow.Myth.MiscItems.Weapons
{
	public class Sunflower : ModItem
	{
		public override void SetStaticDefaults()
		{
			/*DisplayName.SetDefault("Sunflower Disc");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "向日飞盘");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Диск-подсолнух");
            Tooltip.SetDefault("'A weapon made of sunflowers, has a color of hope'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "葵花打造的武器,拥有象征希望的色彩");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Оружие, сделанное из подсолнухов, имеет цвет надежды");*/
			ItemGlowManager.AutoLoadItemGlow(this);
		}
		public static short GetGlowMask = 0;
		public override void SetDefaults()
		{
			Item.glowMask = ItemGlowManager.GetItemGlow(this);
			Item.useStyle = 1;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Melee.Sunflower>();
			Item.DamageType = DamageClass.Melee;
			Item.width = 46;
			Item.height = 46;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.rare = 1;
			Item.damage = 8;
			Item.autoReuse = false;
			Item.knockBack = 2;
		}
		/*public override void UpdateInventory(Player player)
        {
            if(Main.dayTime)
            {
                Item.damage = 12;
            }
            else
            {
                Item.damage = 10;
            }
        }*/
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 v = velocity;
			int u = Projectile.NewProjectile(source, position + velocity * 3f, v, type, damage, knockback, player.whoAmI, 0);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(63, 3)
				.AddTile(16)
				.Register();
		}
	}
}
