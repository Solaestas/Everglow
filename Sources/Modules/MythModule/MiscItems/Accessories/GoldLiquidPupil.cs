using Everglow.Sources.Modules.MythModule;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class GoldLiquidPupil : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Arrogating Eye");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "僭越之眼");
            //Tooltip.SetDefault("Your attacks ignore 35% of enemies' defense\nGenerates a Ring of Ichor after struck, lasts 12s and unstackable\nDeals high damage while it's generating\nDamages enemies on the ring continuously\nInflicts Ichor to enemies inside the ring");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "你的攻击无视敌人35%的防御\n受伤之后生成灵液之环,持续12秒,不可叠加\n灵液之环生成时造成高额伤害\n持续伤害环上的敌人\n对环内的敌人施加灵液");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.width = 26;
            Item.height = 22;
            Item.value = 5500;
            Item.accessory = true;
            Item.rare = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //MythPlayer.GoldLiquidPupil = 2;
        }
    }
}
