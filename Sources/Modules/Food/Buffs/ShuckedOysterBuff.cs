namespace Everglow.Sources.Modules.Food.Buffs
{
    public class ShuckedOysterBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ShuckedOysterBuff");
            Description.SetDefault("减5防御，加10穿甲，每秒减3生命\n“寄生虫！”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.ShuckedOysterBuff = true;
            player.GetArmorPenetration(DamageClass.Generic) += 10;
            player.statDefense -= 5;
        }
    }
}

