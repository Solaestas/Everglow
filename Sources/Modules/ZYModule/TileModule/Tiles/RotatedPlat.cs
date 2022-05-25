using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
{
    internal class RotatedPlat : DynamicTile
    {
        public float rotation;
        public float rotVelocity;

        public override void Move()
        {
            this.position += this.oldVelocity;
            this.rotation += this.rotVelocity;
        }
    }
}
