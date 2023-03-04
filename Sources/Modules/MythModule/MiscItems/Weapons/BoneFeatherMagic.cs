using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class BoneFeatherMagic : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Book of Bone Feather");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "白骨羽箭");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Книга костяного пера");
            Tooltip.SetDefault("Shoots piercing bone feathers, olden enemies struck, weaken their attack and defence");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "射出穿透性骨羽，使命中的敌人衰老，削弱其攻防");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Russian, "Стреляет пронзающими костяными перьями, поражая старением врагов, ослабляют их атаку и защиту");*/
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.DamageType = DamageClass.Magic; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player use a 'Shoot' use style for the Item.
            Item.noMelee = true; // Makes the item not do damage with it's melee hitbox.
            Item.knockBack = 2;
            Item.value = 2000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Magic.BoneFeather>(); // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.shootSpeed = 8; // How fast the item shoots the projectile.
            Item.crit = 8; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.
            Item.mana = 13; // This is how much mana the item uses.
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 v = velocity;
            for (int k = 0; k < 6; k++)
            {
                Vector2 v2 = v.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(0.9f, 1.1f);
                int u = Projectile.NewProjectile(source, position + velocity * 2f, v2, type, damage, knockback, player.whoAmI, 0f);
                Main.projectile[u].hostile = false;
                Main.projectile[u].friendly = true;
            }
            return false;
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FeatherMagic>()
                .AddIngredient(1517, 3)
                .AddTile(125)
                .Register();
        }
    }
}
