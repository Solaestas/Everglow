namespace Everglow.Sources.Modules.MythModule.LanternMoon.Buffs
{
    public class PinkImmune : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pink Flame Immunity");
            //Description.SetDefault("Spiritual Fiery Core's pink flame cannot hurt you");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "粉色火焰免疫");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "花火幻魂心的粉色火焰无法伤害到你");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}
