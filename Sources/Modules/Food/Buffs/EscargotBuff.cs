namespace Everglow.Sources.Modules.Food.Buffs
{
    public class EscargotBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("EscargotBuf");
            Description.SetDefault("这不是神龟药水 \n 大大减速，加50%减伤 ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxRunSpeed *= 0.2f;//减速
            player.runAcceleration *= 0.2f;
            player.endurance *= 0.5f;//加50%减伤
        }
    }
}

