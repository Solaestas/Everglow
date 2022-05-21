namespace Everglow.Sources.Modules.Food.Buffs
{
    public class ApplePieBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ApplePieBuff");
            Description.SetDefault("一天一苹果，医生远离我 \n 加10%减伤,1生命回复");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance *= 0.9f;// 加10%减伤
            player.lifeRegen += 1;

        }
    }
}

