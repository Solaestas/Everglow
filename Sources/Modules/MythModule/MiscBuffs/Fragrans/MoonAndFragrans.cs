namespace Everglow.Sources.Modules.MythModule.MiscBuffs.Fragrans
{
    public class MoonAndFragrans : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Osmanthus Blossom Over the Moon");
            //Description.SetDefault("Blooms and moon, await for you\n25% increased damage, 15% increased crit chance and overall damage multiplier takes effect twice");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "花开冠月");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "有花又月,等你归来\n伤害提升25%, 暴击提升15%, 总伤害乘数生效两次");
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false; // The time remaining won't display on this buff
        }
    }
}
