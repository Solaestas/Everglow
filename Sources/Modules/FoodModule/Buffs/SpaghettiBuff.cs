namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class SpaghettiBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SpaghettiBuff");
            Description.SetDefault("增加1召唤栏\n“异域风情”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxMinions += 1;//加1召唤栏
            player.wellFed = true;
        }
    }
}

