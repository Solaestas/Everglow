using Terraria.DataStructures;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class BloodLightCyanFlame : ModItem
    {
        public override void SetStaticDefaults()
        {//TODO: Localization Needed
			/*DisplayName.SetDefault("Bloody Lasers and Cyan Flames");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血光青炎");
            Tooltip.SetDefault("Legendary Weapon\nAlternately throws boomerblades of laser or cursed flame\n'They should be weapons of the Twins, fortunately you have saved the Mechanic'");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n交替掷出激光旋刃和咒火旋刃\n'它们本应是双子魔眼的武器,幸好你救出了机械师'");*/
			ItemGlowManager.AutoLoadItemGlow(this);
		}
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.useStyle = 1;
            Item.shootSpeed = 17f;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.BloodLightCyanFlame>();
            Item.width = 68;
            Item.height = 68;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = 6;
            Item.damage = 105;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.DamageType = DamageClass.Melee;
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (l % 2 == 0)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.BloodLightCyanFlame>();
            }
            else
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.BloodLightCyanFlame1>();
            }
            Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity, type, damage, knockback, Main.LocalPlayer.whoAmI, 0f, 0f);
            l++;
            return false;
        }
    }
}
