namespace Everglow.Sources.Modules.MythModule.TheFirefly.Buffs
{
    public class GlowMothBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // if the minions exist reset the buff time, otherwise remove the buff from the player.
        }
    }
}
