namespace Everglow.Sources.Modules.Food.Buffs
{
    public class GrapeJuiceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GrapeJuiceBuff");
            Description.SetDefault("浓缩的多子多福 \n 短时间内加5召唤栏，加100%召唤物伤害，击退，极其幸运");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxMinions += 5; //加5召唤栏
            player.GetKnockback(DamageClass.Summon) += 1f;//加100%召唤物击退
            player.GetDamage(DamageClass.Summon) += 0.5f;//加50%召唤物伤害
            player.luck += 1000;
        }
    }
}

