namespace Everglow.Sources.Modules.Food.Buffs
{
    public class SashimiBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SashimiBuff");
            Description.SetDefault("寄生虫！\n 可以游泳，水下呼吸，加10%伤害，20%移速，每秒减3生命");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.SashimiBuff = true;
            player.gills = true;
            player.ignoreWater = true;
            player.accFlipper = true;
            player.maxRunSpeed *= 1.2f;
            player.runAcceleration *= 1.2f;
            player.GetDamage(DamageClass.Generic) += 0.1f;
        }
    }
}

