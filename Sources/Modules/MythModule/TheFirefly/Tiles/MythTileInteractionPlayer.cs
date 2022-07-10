using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    internal class MythTileInteractionPlayer : ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                ModContent.GetInstance<FireflyTree>().PrepareForNewWorld();
            }
            base.OnEnterWorld(player);
        }
    }
}
