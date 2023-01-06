using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class RainbowLight : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("RainbowLight");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "天赐神光");
            Tooltip.SetDefault("Legendary Weapon\nSummon 7 color of light that with different effects\n[c/FF0000:Red]: Increase crit\n[c/FF8E00:Orange]: Increase defense\n[c/FFFF00:Yellow]: Increase damage\n[c/00FF00:Green]: Increase life regen\n[c/00FFFF:Cyan]: Increase evasion\n[c/0000FF:Blue]: Increase mana regen\n[c/B600FF:Purple]: Increase moving speed and fly speed\nEffect strength depends on damage, critting double, advice to cast");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n在头顶释放7种不同颜色的光罩中的一种,每种效果不同,因Buff槽位有限,此武器为直接辅助,不占用Buff\n[c/FF0000:红]:提升暴击\n[c/FF8E00:橙]:提升防御\n[c/FFFF00:黄]:提升攻击\n[c/00FF00:绿]:加速恢复生命\n[c/00FFFF:青]:增加闪避\n[c/0000FF:蓝]:迅速回复法力\n[c/B600FF:紫]:加快速度\n效果强度由伤害决定,暴击翻倍,建议蓄力");*/
        }
        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.crit = 4;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 100;
            Item.width = 72;
            Item.height = 72;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = 10;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shoot = ModContent.ProjectileType<MiscProjectiles.Weapon.Legendary.RainbowCircle>();
            Item.shootSpeed = 0.0001f;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item71;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        private bool Release = true;
        private float Ptime = 0;
    }
}
