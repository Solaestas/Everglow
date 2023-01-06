using Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class BlueRain : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("BlueRain");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "雨落蓝池");
			Tooltip.SetDefault("Legendary Weapon\nRight click to make a heavy hit");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n诞生于被风暴搅动的深海\n雨滴为保持纯净,抗拒与海水融合,最终在深海的乱流中汇聚成这个形状\n右键蓄力重击");*/
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.width = 34;
            Item.height = 112;
            Item.rare = 10;

            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 98;
            Item.knockBack = 5f;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<RainArrowDrop>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12f, 0f);
        }
        int add = 0;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 36;
                Item.useAnimation = 36;
                Item.autoReuse = false;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.useTime = 9;
                Item.useAnimation = 9;
                Item.autoReuse = true;
                Item.noUseGraphic = false;
            }
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            /*for(int h = -3;h < 4;h++)
            {
				Vector2 v = velocity.RotatedBy(h / 7d);
				Projectile.NewProjectile(source, position, v, ModContent.ProjectileType<Projectiles.Ranged.RainArrow>(), damage, knockback, player.whoAmI, 1);
			}*/
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position + new Vector2(0, -24), velocity * 3.4f, ModContent.ProjectileType<RainArrowDrop2>(), damage * 2, knockback, player.whoAmI, 0f, player.GetCritChance(DamageClass.Ranged) + player.GetCritChance(DamageClass.Generic) + 15 + Item.crit);
                return false;
            }
            Vector2 v = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.95f, 1.05f);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<RainArrowDrop>(), damage, knockback, player.whoAmI, 1, Item.crit + player.GetCritChance(DamageClass.Ranged) + player.GetCritChance(DamageClass.Generic));
            for (int h = 0; h < 3; h++)
            {
                Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X + Main.rand.NextFloat(-120f, 120f), position.Y - 500), new Vector2(Main.rand.NextFloat(-1f, 1f), 6f) + player.velocity, ModContent.ProjectileType<RainArrow>(), damage / 2, knockback, player.whoAmI, 0);
            }
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
