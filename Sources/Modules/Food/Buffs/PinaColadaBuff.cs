namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PinaColadaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PinaColadaBuff");
            Description.SetDefault("短时间内十倍反伤（上限1000）,防御增加20\n“从不添加香精当生榨 。 ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.thorns += 10f;//十倍反伤
            player.statDefense += 20;
        }
    }
}

