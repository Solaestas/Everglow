namespace Everglow.Sources.Modules.Food.Buffs
{
    public class ApricotBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ApricotBuff");
            Description.SetDefault("止渴润肺 \n 魔力再生加4");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.manaRegen += 4; // 魔力再生加4
        }
    }
}

