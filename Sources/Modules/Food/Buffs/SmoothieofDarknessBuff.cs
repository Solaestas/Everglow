namespace Everglow.Sources.Modules.Food.Buffs
{
    public class SmoothieofDarknessBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SmoothieofDarknessBuff");
            Description.SetDefault("腹黑（字面意义上）\n 短时间内80%闪避");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();
            FoodModPlayer.SmoothieofDarknessBuff = true;
        }
    }
}

