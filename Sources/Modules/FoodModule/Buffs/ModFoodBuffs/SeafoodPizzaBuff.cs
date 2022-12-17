namespace Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs
{
    public class SeafoodPizzaBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("SeafoodPizzaBuff");
            //Description.SetDefault("提升大部分属性\n“一个人吃完这个可不容易”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 10; // 加10%暴击
            player.GetDamage(DamageClass.Generic) *= 1.1f; // 加10%伤害
            player.GetAttackSpeed(DamageClass.Generic) *= 1.1f; // 加10%攻速
            FoodBuffModPlayer.AddCritDamage += 0.1f;
            player.statDefense += 3; // 加3防御
            player.lifeRegen += 2; // 加2生命回复
            player.manaRegen += 2; // 魔力再生加2

        }
    }
}

