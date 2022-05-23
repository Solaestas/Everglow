namespace Everglow.Sources.Modules.Food.Buffs
{
    public class DragonfruitBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DragonfruitBuff");
            Description.SetDefault("红红火火恍恍惚惚 \n 攻击造成着火");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.DragonfruitBuff = true;
        }
    }
}

