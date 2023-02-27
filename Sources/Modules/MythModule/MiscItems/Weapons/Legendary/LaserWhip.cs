namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
    public class LaserWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            /*DisplayName.SetDefault("Laser Whip");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "激光鞭");
			Tooltip.SetDefault("Legendary Weapon\nYour summons will focus struck enemies\nHitting enemies will have chance release some laser worm, and add a mark.");
			Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "传说武器\n你召唤的仆从将集中打击被击中的敌人\n击中敌人有概率释放暂时性召唤物激光虫,并施加标记");*/
            //ItemID.Sets.[base.Type] = true;
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            DefaultToWhip(ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Legendary.LaserWhip>(), 30, 2f, 5.4f, 30);
            Item.rare = 6;
            Item.damage = 27;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }
        public override bool? UseItem(Player player)
        {
            if (player.autoReuseGlove)
            {
                Item.autoReuse = true;
                return true;
            }
            Item.autoReuse = false;
            return true;
        }
        private void DefaultToWhip(int projectileId, int dmg, float kb, float shootspeed, int animationTotalTime = 30)
        {
            Player player = Main.LocalPlayer;
            Item.autoReuse = false;
            if (player.autoReuseGlove)
            {
                Item.autoReuse = true;
            }
            Item.useStyle = 1;
            Item.useAnimation = animationTotalTime;
            Item.useTime = animationTotalTime;
            Item.width = 18;
            Item.height = 18;
            Item.shoot = projectileId;
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
            Item.damage = dmg;
            Item.knockBack = kb;
            Item.shootSpeed = shootspeed;
        }
    }
}
