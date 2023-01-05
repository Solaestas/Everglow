namespace Everglow.Sources.Modules.MythModule.MiscBuffs.Fragrans
{
	public class MoonAndFragransIII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Osmanthus Blossom Over the Moon Ⅲ");
            //Description.SetDefault("Blooms and moon, await for you\n75% increased damage, 45% increased crit chance and overall damage multiplier takes effect four times");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "金流遍野");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "有花又月,等你归来\n伤害提升75%, 暴击提升45%, 总伤害乘数生效四次");
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false; // The time remaining won't display on this buff
        }
    }
}
