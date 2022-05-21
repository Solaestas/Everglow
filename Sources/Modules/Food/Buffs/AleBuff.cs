namespace Everglow.Sources.Modules.Food.Buffs
{
    public class AleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AleBuff");
            Description.SetDefault("酿跄 \n 防御减8，伤害、暴击率和近战攻速各增加8%");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 8; // 减8防御
            player.GetCritChance(DamageClass.Melee) += 8; // 加8%暴击
            player.GetDamage(DamageClass.Melee).Base += 0.08f; // 加8%伤害
            player.GetAttackSpeed(DamageClass.Generic) += 0.08f; // 加8%攻速

        }
    }
}

