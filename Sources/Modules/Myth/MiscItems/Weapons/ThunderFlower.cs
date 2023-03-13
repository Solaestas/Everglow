using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class ThunderFlower : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Flower of Lighting");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Цветок света");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雷之花");
			Tooltip.SetDefault("Fires thunderstrom orbs\n'Too many projectiles!'");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "射出雷暴球\n'射弹太多了！'");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Стреляет грозовыми шарами \n'Слишком много снарядов!'");*/
        }
        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Magic.ThunderBall>();
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.GetCritChance(DamageClass.Magic) + player.GetCritChance(DamageClass.Generic));
            return false;
        }
    }
}
