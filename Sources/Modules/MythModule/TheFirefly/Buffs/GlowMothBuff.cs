namespace Everglow.Sources.Modules.MythModule.TheFirefly.Buffs
{
    public class GlowMothBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // if the minions exist reset the buff time, otherwise remove the buff from the player.
        }
    }
}