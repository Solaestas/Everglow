namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PizzaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PizzaBuff");
            Description.SetDefault("会让意大利人破防的菠萝披萨 \n 加8穿甲 ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 8;//加8穿甲
        }
    }
}

