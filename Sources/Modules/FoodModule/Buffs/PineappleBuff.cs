namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class PineappleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PineappleBuff");
            Description.SetDefault("增加6防御，50%反伤\n“菠萝碱”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 6; // 加6防御
            player.thorns += 0.5f;//50%反伤
            player.wellFed = true;
        }
    }
}

