namespace Everglow.Sources.Modules.MythModule.MiscBuffs
{
    public class LaserWormBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Laser Worm");
            //Description.SetDefault("Laser worms fight for you");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "激光虫");
            //Description.AddTranslation((int)GameCulture.CultureName.Chinese, "激光虫将为你而战");
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false; // The time remaining won't display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // if the minions exist reset the buff time, otherwise remove the buff from the player.
        }
    }
}
