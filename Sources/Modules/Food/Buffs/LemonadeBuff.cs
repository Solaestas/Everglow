namespace Everglow.Sources.Modules.Food.Buffs
{
    public class LemonadeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("LemonadeBuff");
            Description.SetDefault("消炎美容 \n 短时间内远程击退加倍,仇恨值减2400  ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetKnockback(DamageClass.Ranged) *= 2f; // 击退加倍
            player.aggro -= 2400;//仇恨值减2400
        }
    }
}

