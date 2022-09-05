using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    internal class FireflyTreeEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            return Main.tile[x, y].TileType == ModContent.TileType<FireflyTree>();
        }
       
    }
}
