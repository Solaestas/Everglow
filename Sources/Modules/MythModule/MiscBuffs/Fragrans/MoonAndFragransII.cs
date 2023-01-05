namespace Everglow.Sources.Modules.MythModule.MiscBuffs.Fragrans
{
	public class MoonAndFragransII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Osmanthus Blossom Over the Moon Ⅱ");
            //Description.SetDefault("Blooms and moon, await for you\n50% increased damage, 30% increased crit chance and overall damage multiplier takes effect three times");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "一树金秋");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "有花又月,等你归来\n伤害提升50%, 暴击提升30%, 总伤害乘数生效三次");
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false; // The time remaining won't display on this buff
        }
    }
}
