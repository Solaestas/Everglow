namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PadThaiBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PadThaiBuff");
            Description.SetDefault("异域风情 \n 加50%召唤物击退");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetKnockback(DamageClass.Generic) += 0.50f; // 加50%击退
        }
    }
}

