namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PinaColadaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PinaColadaBuff");
            Description.SetDefault("从不添加香精当生榨 \n 短时间内五倍反伤,但防御为负值");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.thorns += 5f;//五倍反伤
            player.immuneTime += 300;
            player.statDefense *= -1;
        }
    }
}

