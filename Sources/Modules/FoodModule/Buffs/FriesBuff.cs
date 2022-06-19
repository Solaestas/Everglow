namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FriesBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FriesBuff");
            Description.SetDefault("加4防御，8%暴击\n“高油高盐”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 4; // 加4防御
            player.GetCritChance(DamageClass.Generic) += 8; // 加8%暴击
            player.wellFed = true;
        }
    }
}

