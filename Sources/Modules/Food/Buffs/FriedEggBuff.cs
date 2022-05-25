namespace Everglow.Sources.Modules.Food.Buffs
{
    public class FriedEggBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FriedEggBuff");
            Description.SetDefault("加8%伤害\n“蛋白质”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) *= 1.08f; // 加8%伤害

        }
    }
}

