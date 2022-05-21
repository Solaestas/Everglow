namespace Everglow.Sources.Modules.Food.Buffs
{
    public class GrubSoupBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GrubSoupBuff");
            Description.SetDefault("吃啥补啥，就是有点恶心 \n 加25渔力，每秒减2生命");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.fishingSkill += 25;
            FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
            FoodModPlayer.GrubSoupBuff = true;
        }
    }
}

