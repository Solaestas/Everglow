namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class Armor15Cracked : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
	}
}

public class Armor15CrackNPC : GlobalNPC
{
	public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
	{
		if(npc.HasBuff(ModContent.BuffType<Armor15Cracked>()))
		{
			modifiers.Defense.Base -= 15;
		}
		base.ModifyIncomingHit(npc, ref modifiers);
	}
}