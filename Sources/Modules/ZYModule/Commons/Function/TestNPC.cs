using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Everglow.Sources.Commons.Core;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function
{
    internal class TestNPC : ModNPC
    {
        public override string Texture => $"Terraria/Images/NPC_{NPCID.Zombie}";
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Zombie);
            var handler = new ActionHandler();
            handler.Action = () =>
            {
                if (!NPC.active || NPC != Main.npc[NPC.whoAmI])
                {
                    Everglow.HookSystem.Remove(handler);
                    return;
                }
                Main.NewText("Alive");
            };
            Everglow.HookSystem.AddMethod(handler, CallOpportunity.PostUpdateNPCs);
        }
    }
}
