namespace Everglow.Sources.Modules.Food.Buffs
{
    public class CoconutBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CoconutBuff");
            Description.SetDefault("增加4防御，5%减伤\n“我从小啃到大”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 4; // 加4防御
            player.endurance *= 0.95f;// 加5%减伤
        }
    }
}

