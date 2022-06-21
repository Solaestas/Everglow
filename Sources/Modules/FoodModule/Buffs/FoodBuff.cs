namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FoodBuff");
            Description.SetDefault("你现在状态很好");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
           
        }
    }
}

