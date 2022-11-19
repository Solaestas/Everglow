namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class WalnutClip : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nutcracker");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "胡桃夹子");
            //Tooltip.SetDefault("Increases crit damage by 16%\nWhen your Hp is below 50% of the max Hp, increases damage by (50% of max Hp - current Hp)%\n'Besides cracking nuts and crabs, it can crack enemies, too'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "暴击伤害增加16%\n低于半血后伤害增加(半血血量-当前血量)%\n'除了夹碎胡桃和螃蟹,它还能夹碎敌人'");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 20000;
            Item.accessory = true;
            Item.rare = 8;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLifeMax2 / 2f > player.statLife)
            {
                player.GetDamage(DamageClass.Generic) *= (player.statLifeMax2 / 2f - player.statLife) / 100f + 1;
            }

        }
    }
}
