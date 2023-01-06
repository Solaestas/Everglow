using Terraria.DataStructures;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class MachineFit : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("MachineFit");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "统帅之拳");
            Tooltip.SetDefault("Legendary Weapon\nA terrible killer fit, on hit enemy will split to 5 chasing missile");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n无情的灭杀之拳,击中敌人后分裂成5发追踪导弹");*/
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.useStyle = 1;
            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.MachineFit>();
            Item.width = 58;
            Item.height = 100;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 36;
            Item.useTime = 36;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = 6;
            Item.damage = 72;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.DamageType = DamageClass.Magic;
            Item.mana = 28;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, player.GetCritChance(DamageClass.Melee) + Item.crit, 0f);

            return false;
        }
    }
}
