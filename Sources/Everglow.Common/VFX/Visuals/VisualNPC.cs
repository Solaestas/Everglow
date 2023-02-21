using Everglow.Common.VFX;
using Everglow.Core.Enums;

namespace Everglow.Core.VFX.Visuals;

public abstract class VisualNPC : ModNPC, IVisual
{
	public bool Active => NPC.active && Main.npc[NPC.whoAmI] == NPC;

	public virtual CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public bool Visible => !NPC.hide && VFXManager.InScreen(NPC.position, 100);

	public int Type => throw new NotImplementedException();

	public string Name => throw new NotImplementedException();

	public virtual void Draw()
	{
	}

	public override void SetDefaults()
	{
		if (!Main.gameMenu)
		{
			VFXManager.Add(this);
		}
	}

	public void Kill()
	{
	}

	public void Update()
	{
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Unload()
	{
		throw new NotImplementedException();
	}
}