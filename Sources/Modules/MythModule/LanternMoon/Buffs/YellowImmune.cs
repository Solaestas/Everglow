﻿namespace Everglow.Sources.Modules.MythModule.LanternMoon.Buffs
{
    public class YellowImmune : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yellow Flame Immuniy");
            //Description.SetDefault("Spiritual Fiery Core's yellow flame cannot hurt you");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "黄色火焰免疫");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "花火幻魂心的黄色火焰无法伤害到你");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}
