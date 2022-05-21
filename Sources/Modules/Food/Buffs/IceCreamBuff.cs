namespace Everglow.Sources.Modules.Food.Buffs
{
    public class IceCreamBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("IceCreamBuff");
            Description.SetDefault("吃雪（bushi \n 免疫着火和火块");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.OnFire] = true;// 免疫着火
            player.fireWalk = true;// 免疫火块
        }
    }
}

