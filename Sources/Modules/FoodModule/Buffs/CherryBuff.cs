namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class CherryBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CherryBuff");
            Description.SetDefault("增加移速与跳跃高度\n“你所热爱的，就是你的生活”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxRunSpeed *= 1.2f;
            player.runAcceleration *= 1.2f;
            player.jumpSpeedBoost += 1;
            player.wellFed = true;
        }
    }
}

