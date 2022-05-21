namespace Everglow.Sources.Modules.Food.Buffs
{
    public class ChristmasPuddingBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ChristmasPuddingBuff");
            Description.SetDefault("美容养颜  \n 仇恨值减600");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.aggro -= 600;//仇恨值减600
        }
    }
}

