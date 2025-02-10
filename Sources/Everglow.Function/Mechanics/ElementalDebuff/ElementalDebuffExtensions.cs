namespace Everglow.Commons.Mechanics.ElementalDebuff;

public static class ElementalDebuffExtensions
{
	/// <summary>
	/// Add build-up to the specific elemental debuff instance of this NPC. This accounts for if NPC has resistance to this type of elemental debuff.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="source"></param>
	/// <param name="type"></param>
	/// <param name="buildUp"></param>
	/// <param name="penentration"></param>
	public static bool AddElementalDebuffBuildUp(this NPC npc, Player source, ElementalDebuffType type, int buildUp, float penentration = 0)
	{
		if (source != null)
		{
			penentration += source.GetModPlayer<ElementalDebuffPlayer>().ElementPenetration;
		}

		return AddElementalDebuffBuildUp(npc, type, buildUp, penentration);
	}

	/// <summary>
	/// Add build-up to the specific elemental debuff instance of this NPC. This accounts for if NPC has resistance to this type of elemental debuff.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="type"></param>
	/// <param name="buildUp"></param>
	/// <param name="penentration"></param>
	/// <returns></returns>
	public static bool AddElementalDebuffBuildUp(this NPC npc, ElementalDebuffType type, int buildUp, float penentration = 0)
	{
		// Add to real target of the npc
		if (npc.realLife == -1 || npc.realLife == npc.whoAmI)
		{
			return npc.GetGlobalNPC<ElementalDebuffGlobalNPC>().ElementalDebuffs[type].AddBuildUp(buildUp, penentration);
		}
		else
		{
			var realLife = Main.npc[npc.realLife];
			return realLife.active
				? realLife.GetGlobalNPC<ElementalDebuffGlobalNPC>().ElementalDebuffs[type].AddBuildUp(buildUp, penentration)
				: false;
		}
	}

	/// <summary>
	/// Get the specific elemental debuff instance of this NPC.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static ElementalDebuff GetElementalDebuff(this NPC npc, ElementalDebuffType type)
	{
		return npc.GetGlobalNPC<ElementalDebuffGlobalNPC>().ElementalDebuffs[type];
	}
}