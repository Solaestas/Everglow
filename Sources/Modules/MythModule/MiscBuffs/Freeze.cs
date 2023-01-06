namespace Everglow.Sources.Modules.MythModule.MiscBuffs
{
    public class Freeze : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Icebounded");
            //Description.SetDefault("");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "冰封");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }
}
