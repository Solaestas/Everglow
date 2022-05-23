namespace Everglow.Sources.Modules.Food.Buffs
{
    public class CookedMarshmallowBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CookedMarshmallowBuff");
            Description.SetDefault("轻飘飘 \n 减75%最大掉落速度,但无法操控下落速度");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxFallSpeed *= 0.25f;
            player.noFallDmg = true;
        }
    }
}

