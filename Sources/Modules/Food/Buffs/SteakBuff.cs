namespace Everglow.Sources.Modules.Food.Buffs
{
    public class SteakBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SteakBuff");
            Description.SetDefault("减少33%魔力消耗\n“上流 ”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost *= 0.66f;//减33%魔力消耗
        }
    }
}

