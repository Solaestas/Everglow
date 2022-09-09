using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MEACModule
{
    public class MEACPlayer : ModPlayer
    {
        public bool isUsingMeleeProj = false;
        public override void PreUpdate()
        {
            if(isUsingMeleeProj)
            {
                Player.itemAnimation = 2;
            }
        }
    }
}
