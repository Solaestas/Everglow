using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.LunarFlare
{
    internal class LunarFlareBook : MagicBookProjectile
    {
        int timer;
        public override void SetDef()
        {
            DustType = DustID.WhiteTorch;
            //DustTypeII = DustID.GemSapphire;
            ItemType = ItemID.LunarFlareBook;
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];
            if (MoonNight.Timer < 0)
            {
                MoonNight.Timer = 0;
            }
            MoonNight.Timer+= 10;
            if (player.itemTime == 2)
            {
                timer++;
                if (timer % 2 == 0)
                {
                    int count = Math.Min(timer / 12, 15);
                    MoonNight.GenerateStars(count * 5);
                }
            }
        }
    }
}
