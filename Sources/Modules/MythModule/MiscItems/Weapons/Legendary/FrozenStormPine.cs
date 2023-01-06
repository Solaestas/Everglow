using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class FrozenStormPine : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Frozen Storm Pine");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "冰凌雪松");*/
        }
        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 108;
            Item.height = 112;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = 5;
            //Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 2.4f;
            Item.value = 50000;
            Item.rare = 11;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = 336;
            Item.shootSpeed = 11f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 v = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f) * Math.PI) * Main.rand.NextFloat(0.8f, 1.2f);
            Projectile.NewProjectile(source, position + v * 8, v, 336, damage, knockback, player.whoAmI, 1f);
            v = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f) * Math.PI) * Main.rand.NextFloat(0.8f, 1.2f);
            Projectile.NewProjectile(source, position + v * 8, v, 336, damage, knockback, player.whoAmI, 1f);
            v = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f) * Math.PI) * Main.rand.NextFloat(0.8f, 1.2f);
            Projectile.NewProjectile(source, position + v * 8, v, 336, damage, knockback, player.whoAmI, 1f);
            v = velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f) * Math.PI) * Main.rand.NextFloat(0.8f, 1.2f);
            Projectile.NewProjectile(source, position + v * 8, v, 336, damage, knockback, player.whoAmI, 1f);
            v = velocity.RotatedBy(Main.rand.NextFloat(-0.02f, 0.02f) * Math.PI) * Main.rand.NextFloat(0.8f, 1.2f);
            int f = Projectile.NewProjectile(source, position + v * 8, v, 337, (int)damage, (float)knockback, player.whoAmI, 0f, 0f);
            Main.projectile[f].rotation = (float)(Math.Atan2(v.Y, v.X) + Math.PI / 2d);
            Main.projectile[f].timeLeft = 1200;
            if (Main.mouseRight)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.PineVortex>()] < 1)
                {
                    v = velocity;
                    Projectile.NewProjectile(source, position + v * 10, v, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.PineVortex>(), (int)damage, (float)knockback, player.whoAmI, 0f, 0f);
                }
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.PineHalo>()] < 1)
            {
                v = velocity;
                Projectile.NewProjectile(source, position + v * 10, Vector2.Zero, ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.PineHalo>(), (int)damage, (float)knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
