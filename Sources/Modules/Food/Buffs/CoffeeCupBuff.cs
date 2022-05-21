namespace Everglow.Sources.Modules.Food.Buffs
{
    public class CoffeeCupBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CoffeeCupBuff");
            Description.SetDefault("神清气爽 \n 加25%铺墙铺砖速度，5%攻速");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.tileSpeed *= 1.25f;
            player.wallSpeed *= 1.25f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.05f; // 加5%攻速
        }
    }
}

