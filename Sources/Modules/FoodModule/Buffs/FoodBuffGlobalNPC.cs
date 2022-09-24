using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Modules.FoodModule.Buffs;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffGlobalNPC : GlobalNPC
    {
        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (crit)
            {
                damage *= (FoodBuffModPlayer.CritDamage + 1) / 2f;
            }
            return base.StrikeNPC(npc, ref damage, defense, ref knockback, hitDirection, ref crit);
        }
    }
}
