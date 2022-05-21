namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PinaColadaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PinaColadaBuff");
            Description.SetDefault("从不添加香精当生榨 \n 加5%减伤，2防御，33%反伤");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance *= 0.95f;//5%减伤
            player.statDefense += 2; // 加2防御
            player.thorns += 0.33f;//33%反伤
        }
    }
}

