namespace Everglow.Sources.Modules.Food.Buffs
{
    public class FroggleBunwichBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FroggleBunwichBuff");
            Description.SetDefault("你能真正尝到沼泽的味道 \n 自动跳跃，增加伤害和跳跃能力");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic).Base += 0.04f; // 加8%伤害
            player.autoJump = true;
            player.jumpSpeedBoost += 2;
        }
    }
}

