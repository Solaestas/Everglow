namespace Everglow.Sources.Modules.Food.Buffs
{
    public class MangoBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MangoBuff");
            Description.SetDefault("清肺 \n 减缓因食物中毒而产生的持续减血效果");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
            FoodModPlayer.MangoBuff = true;
        }
    }
}

