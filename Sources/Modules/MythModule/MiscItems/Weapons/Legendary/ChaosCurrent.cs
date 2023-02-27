using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class ChaosCurrent : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Chaos Current");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "混沌爆流");
            Tooltip.SetDefault("Legendary Weapon\nSummon a explosion that can make enemis in chaos");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n爆炸出一圈扰乱神经的乱流");*/
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 60;
            Item.width = 74;
            Item.height = 90;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = 5;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.ChaosCurrent>();
            Item.shootSpeed = 8;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int h = Projectile.NewProjectile(source, position + velocity * 6.4f, velocity, type, damage, knockback, player.whoAmI, velocity.X * 10f, velocity.Y * 10f - 5);
            return false;
        }
    }
}
