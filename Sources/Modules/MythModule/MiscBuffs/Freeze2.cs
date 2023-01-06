namespace Everglow.Sources.Modules.MythModule.MiscBuffs
{
	public class Freeze2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Icebounded Ⅱ");
            //Description.SetDefault("");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "冰封Ⅱ");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
