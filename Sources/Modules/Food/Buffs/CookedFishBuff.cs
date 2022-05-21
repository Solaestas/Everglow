namespace Everglow.Sources.Modules.Food.Buffs
{
    public class CookedFishBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CookedFishBuff");
            Description.SetDefault("益气健脾 \n 加40魔力上限,8%魔法暴击率");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Magic) += 0.08f;//加8%魔法暴击率
            player.statManaMax2 += 40;//加40魔力上限
        }
    }
}

