namespace Everglow.Sources.Modules.Food.Buffs
{
    public class AppleJuiceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AppleJuiceBuff");
            Description.SetDefault("一天一苹果，医生远离我 \n 短时间内60%减伤");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance *= 0.4f;// 加60%减伤
        }
    }
}
