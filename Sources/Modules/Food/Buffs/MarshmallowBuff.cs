namespace Everglow.Sources.Modules.Food.Buffs
{
    public class MarshmallowBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MarshmallowBuff");
            Description.SetDefault("像云一样 \n 可以二段跳");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.hasJumpOption_Cloud = true;
        }
    }
}

