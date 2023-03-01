using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.LunarFlare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles
{
    internal class MagicBookGlobalProjectile : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.type == ProjectileID.LunarFlare)
            {
                if(!crit)
                {
                    //crit = Main.rand.NextFloat() < MoonNight.stars.Count * 0.002f;
                }
                else
                {
                    //damage += (int)(MoonNight.stars.Count * 0.006f * damage);
                }
            }
        }
    }
}
