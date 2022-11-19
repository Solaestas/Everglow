using Everglow.Sources.Modules.MythModule;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class CorruptEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scaled Cursed Eye");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "鳞甲诅咒眼");
            //Tooltip.SetDefault("5 defence\nincreases crit chance by 4%\nGrants immunity to Cursed Inferno\nRelease cursed flame when struck\nTough its owner has gone, it's still filled with curse to enemies'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "5防御\n增加4%暴击率\n免疫咒火\n受伤时释放咒火\n'即便它的主人已经死了,它仍充满着对敌人的诅咒'");
            ItemGlowManager.AutoLoadItemGlow(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.width = 32;
            Item.height = 32;
            Item.value = 5000;
            Item.accessory = true;
            Item.rare = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //MythPlayer.ImmuCurseF = 2;
            player.statDefense += 5;
            player.GetCritChance(DamageClass.Melee) += 4;
            player.GetCritChance(DamageClass.Ranged) += 4;
            player.GetCritChance(DamageClass.Magic) += 4;
            player.GetCritChance(DamageClass.Summon) += 4;
            player.buffImmune[39] = true;
        }
    }
}
