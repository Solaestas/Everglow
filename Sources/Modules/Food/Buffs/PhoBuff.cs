namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PhoBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PhoBuff");
            Description.SetDefault("异域风情 \n 加10%召唤物伤害");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }
    }
}

