using Everglow.Commons.Templates.Pylon;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Pylon;

public class ShabbyPylonUpdate_NPC : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if (npc.type == NPCID.BrainofCthulhu || ((npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail) && NPC.CountNPCS(NPCID.EaterofWorldsBody) + NPC.CountNPCS(NPCID.EaterofWorldsHead) + NPC.CountNPCS(NPCID.EaterofWorldsTail) == 1))
        {
            do
            {
                PylonSystem.Instance.shabbyPylonEnable = true;
                PylonSystem.Instance.firstEnableAnimation = true;
                Main.NewText(Language.GetTextValue("Mods.Everglow.Common.PylonSystem.ShabbyPylonRepairedTip"));
            }
            while (!PylonSystem.Instance.shabbyPylonEnable && NPC.downedBoss2);
        }
    }
}