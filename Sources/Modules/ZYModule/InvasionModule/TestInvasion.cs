using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.InvasionModule;

internal class TestInvasion : Invasion
{
    public override string Name => "test";
    public override Asset<Texture2D> Icon => ModContent.Request<Texture2D>("Terraria/Images/UI/Wires_0");
    public override void Update()
    {
        progress++;
        progressMax = 100000;
        base.Update();
    }
}
