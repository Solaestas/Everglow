using Everglow.Resources.ItemList.Weapons.Ranged;

namespace Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs
{
    public class B_52Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("B_52Buff");
            //Description.SetDefault("短时间内大幅提升发射器速度、暴击、伤害\n“轰炸机”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Launchers.vanillaLaunchers.Contains(player.HeldItem.type))
            {
                player.GetAttackSpeed(DamageClass.Ranged) *= 2;
                player.GetCritChance(DamageClass.Ranged) += 25;
                player.GetDamage(DamageClass.Ranged) *= 1.5f;
            }
            player.wellFed = true; //TODO other buffs should have this
        }
    }
}

