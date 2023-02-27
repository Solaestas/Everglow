using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class MachineSkeGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Machine Skeleton Gun");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "杀戮之轮");
            Tooltip.SetDefault("Divine Weapon\nThe hided sunday punch of a powerful mochine");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "神话武器\n钢铁骷髅未来得及的终极杀器");*/
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 74;
            Item.height = 20;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.knockBack = 0;
            Item.value = 20000;
            Item.rare = 7;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(1));
            newVelocity *= 1f - Main.rand.NextFloat(0.3f);
            Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear>()] < 1)
            {
                for (int i = 0; i < 230; i++)
                {
                    Projectile.NewProjectile(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Null>(), 0, 1, Main.myPlayer, 0, 0f);
                }
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear>(), damage, knockback, player.whoAmI, -player.direction);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear9>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear9>(), damage, knockback, player.whoAmI, -player.direction);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear2>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear2>(), damage, knockback, player.whoAmI, -player.direction);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear4>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear4>(), damage, knockback, player.whoAmI, -player.direction);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear3>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear3>(), damage, knockback, player.whoAmI, -player.direction);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear7>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear7>(), damage, knockback, player.whoAmI, -player.direction);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear8>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.Gear8>(), damage, knockback, player.whoAmI, -player.direction);
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, -2f);
        }
    }
}
