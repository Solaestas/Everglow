namespace Everglow.Sources.Modules.Food.Buffs
{
    public class BBQRibsBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BBQRibsBuff");
            Description.SetDefault("滋阴补血 \n 加50血量上限 ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 += 50; // 加50血量上限
        }
    }
}

