namespace Everglow.Sources.Modules.Food.Buffs
{
    public class RoastedDuckBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RoastedDuckBuff");
            Description.SetDefault("别看成烧鸡，是烤鸭 \n 可以在水上行走，增加飞行能力");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.RoastedDuckBuff = true;
            FoodBuffModPlayer.WingTimeModifier = 1.1f;
            player.maxFallSpeed *= 0.5f;
            player.extraFall += 30;
            player.waterWalk = true;
        }
    }
}

