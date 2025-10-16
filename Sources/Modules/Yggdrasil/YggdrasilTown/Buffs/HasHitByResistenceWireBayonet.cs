namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class HasHitByResistenceWireBayonet : ModBuff
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