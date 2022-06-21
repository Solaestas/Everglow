namespace Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs
{
    public class SashimiBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SashimiBuff");
            Description.SetDefault("可以游泳，水下呼吸，增加10%伤害，20%移速，但每秒减3生命\n“寄生虫！”");
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
            player.GetDamage(DamageClass.Generic) *= 1.1f;
            player.wellFed = true;
        }
    }
}

