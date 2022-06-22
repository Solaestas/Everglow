using Terraria.DataStructures;
using Terraria.Localization;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class ShadowWingBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Occulting Wings");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "影翼巨弓");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Крылья затмения");
            Tooltip.SetDefault("Hold to charge to  increase damage, capped at +200%, and additionally fires Blue Butterfly Arrows.\nNeeds 1.5s to be fully charged");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "长按可以蓄力,使伤害提升最大倍率200%并释放蓝蝶箭\n满蓄时间1.50秒");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Удерживайте, чтобы зарядить и увеличить урон, ограниченный на + 200%, и дополнительно стреляет синими стрелами-бабочками.\nТребуется 1,5 секунды для полной зарядки");
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }

        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.width = 46;
            Item.height = 82;
            Item.rare = ItemRarityID.Green;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item5;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.knockBack = 5f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.ShadowWingBow>()] <= 0)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.ShadowWingBow>(), (int)(damage * 0.75f), knockback, player.whoAmI, type, Item.useAnimation);
            }
            return false;
        }
    }
}
