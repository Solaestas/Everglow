using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.Weapons
{
    public class ToothBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Tooth Bow");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "獠牙巨弓");
			Tooltip.SetDefault("Legendary Weapon\nRight click to gather tootharrows");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n右键聚集齿箭");*/
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 78;
            Item.rare = 2;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 25;
            Item.knockBack = 5f;
            Item.noMelee = true;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12f, 0f);
        }
        int add = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
            position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
            position.Y -= 100;
            Vector2 heading = target - position;

            if (heading.Y < 0f)
            {
                heading.Y *= -1f;
            }

            if (heading.Y < 20f)
            {
                heading.Y = 20f;
            }
            add += 1;
            heading.Normalize();
            heading *= velocity.Length() * 3f;
            heading.Y += Main.rand.Next(-40, 41) * 0.02f;
            if (add % 2 == 0)
            {
                add = 0;
                Projectile.NewProjectile(source, position, heading, ModContent.ProjectileType<Projectiles.Weapon.TuskArrow>(), damage, knockback, player.whoAmI, 0f, ceilingLimit);
            }
            return true;
        }
    }
}
