using Everglow.Yggdrasil.YggdrasilTown.Buffs;

namespace Everglow.Yggdrasil.Common;

public class YggdrasilGlobalNPC : GlobalNPC
{
	public bool CharredActive { get; set; }

	public override bool InstancePerEntity => true;

	public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
	{
		if (CharredActive)
		{
			modifiers.Defense.Flat -= npc.defense * Charred.DefenseReduction;
		}
	}
}