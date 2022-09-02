using Everglow.Sources.Commons.Core.VFX.Interfaces;

namespace Everglow.Sources.Commons.Core.VFX.Base;

public class VisualNPC : ModNPC, IVisual
{
    public bool Active => NPC.active && Main.npc[NPC.whoAmI] == NPC;

    public virtual CallOpportunity DrawLayer => CallOpportunity.PostDrawProjectiles;

    public bool Visible => !NPC.hide && VFXManager.InScreen(NPC.position, 100);

    public virtual void Draw()
    {
    }

    public override void SetDefaults()
    {
        if (!Main.gameMenu)
        {
            VFXManager.Instance.Add(this);
        }
    }

    public void Kill()
    {
    }

    public void Update()
    {
    }
}