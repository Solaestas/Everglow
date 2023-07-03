using Everglow.EternalResolve.Items.Weapons.StabbingSwords;
using Terraria;

namespace Everglow.EternalResolve.Common;

public class EternalResolveContentGlobalNPC : GlobalNPC
{
	public override void ModifyShop(NPCShop shop)
	{
		if (shop.NpcType == NPCID.Clothier)
		{
			shop.Add<CrutchBayonet>();
		}
		if (shop.NpcType == NPCID.Mechanic)
		{
			shop.Add<MechanicMosquito>();
		}
	}
}