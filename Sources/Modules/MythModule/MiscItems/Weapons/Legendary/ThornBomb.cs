using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class ThornBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("ThornBomb");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "刺球炸弹");
            Tooltip.SetDefault("Legendary Weapon\nAlternately Pink bomb ＆ Cyan bomb");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n交替射出双色炸弹");*/
        }
        public override void SetDefaults()
        {
            Item.damage = 118;//伤害 原200→现140
            Item.DamageType = DamageClass.Ranged;
            Item.crit = 16;
            Item.width = 20;
            Item.height = 38;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.GreenThornBomb>();
            Item.noUseGraphic = true;
            Item.rare = 7;
            Item.UseSound = SoundID.Item5;
            Item.shootSpeed = 17f;
        }
        private int l = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (l % 2 == 0)
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.GreenThornBomb>();
            }
            else
            {
                type = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.PinkThornBomb>();
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, Item.crit + player.GetCritChance(DamageClass.Ranged), 0f);
            l++;
            return false;
        }
    }
}
