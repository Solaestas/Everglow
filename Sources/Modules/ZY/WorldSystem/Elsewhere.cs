using Everglow.Sources.Modules.ZY.WorldSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;

namespace ZYMod.WorldSystem;

internal class Elsewhere : World
{
    public Elsewhere()
    {
    }

    public Elsewhere(WorldFileData data) : base(data)
    {
    }

    public override string WorldName => "Elsewhere";
    
    public override uint Version => 1;

    //public override Asset<Texture2D> WorldIcon => ModContent.Request<Texture2D>("ZYMod/IconOcean");
    public override void GenerateWorld()
    {
        
    }
}
