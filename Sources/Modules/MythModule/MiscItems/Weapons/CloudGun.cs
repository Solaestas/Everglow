using Everglow.Sources.Modules.MythModule.MiscItems.Ammos;
using Everglow.Sources.Modules.MythModule.MiscProjectiles;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class CloudGun : ModItem
    {
        /*public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloud Cannon");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "云导炮");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Облачная пушка");
            Tooltip.SetDefault("Launches cloud clusters");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "发射云团");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Запускает облачные кластеры");
        }*/

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 42;
            Item.height = 46;
            Item.useTime = 30;
            Item.useAnimation = 30;
            /*Item.reuseDelay = 18;*/
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.knockBack = 5;
            Item.value = 700;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.CloudBall>();
            Item.shootSpeed = 8f; // The speed of the projectile (measured in pixels per frame.)
            Item.useAmmo = ModContent.ItemType<Ammos.CloudBall>();
            Item.crit = 16; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
        }

        public override void HoldItem(Player player)
        {
            /*for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.PiOver4)
            {
                Vector2 fx = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
                Vector2 pos = player.Center + i.ToRotationVector2() * 20f + fx * 33;
                Vector2 vel = (player.Center + fx * 33f - pos).SafeNormalize(Vector2.Zero);
                int r = Dust.NewDust(pos, 0, 0, DustID.Cloud, 0, 0, 0, default, 2f);
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = vel * 2;
            }*/
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item11);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0);
            return false;
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-24f, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 100)
                .AddTile(TileID.SkyMill)
                .Register();
        }
    }
}
