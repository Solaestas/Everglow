namespace Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs
{
    public class NachosBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("NachosBuff");
            //Description.SetDefault("攻击造成涂油以及所有火焰减益\n“爆米花”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.NachosBuff = true;
            
        }
    }
}

