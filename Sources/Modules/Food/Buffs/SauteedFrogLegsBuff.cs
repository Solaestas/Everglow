namespace Everglow.Sources.Modules.Food.Buffs
{
    public class SauteedFrogLegsBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SauteedFrogLegsBuff");
            Description.SetDefault("钓不到蛙腿？那就吃吧 \n 自动跳跃，增加跳跃能力");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.autoJump = true;
            player.jumpSpeedBoost += 1f;
            player.maxFallSpeed += 5f;
            player.jumpBoost = true;
        }
    }
}

