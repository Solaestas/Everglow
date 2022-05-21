namespace Everglow.Sources.Modules.Food.Buffs
{
    public class TeacupBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TeacupBuff");
            Description.SetDefault("醇香四溢 \n 加2魔力回复，20魔力上限");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.manaRegen += 2;//加2魔力回复
            player.statManaMax2 += 20;//加20魔力上限
        }
    }
}

