namespace Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs
{
    public class LonelyJellyfishBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("LonelyJellyfishBuff");
            //Description.SetDefault("短时间内不断于鼠标处生成水母电圈，并且获得隐身能力\n“观赏完就一饮而尽吧”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.stealth = 100;
            if (Main.time % 0.5f == 0)
            {
                int projectile = Projectile.NewProjectile(null, Main.MouseWorld, Vector2.Zero, ProjectileID.Electrosphere, Math.Clamp(player.HeldItem.damage, 25, 150), 10, player.whoAmI);
            }
        }
    }
}

