namespace Everglow.Sources.Modules.Food.Buffs
{
    public class BunnyStewBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BunnyStewBuff");
            Description.SetDefault("没有任何兔子在制作过程中受到伤害 \n 自动跳跃，增加跳跃能力");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.autoJump = true;
            player.jumpSpeedBoost += 2;
        }
    }
}

