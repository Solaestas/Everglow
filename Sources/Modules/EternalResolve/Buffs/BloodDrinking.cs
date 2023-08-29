namespace Everglow.EternalResolve.Buffs;

public class BloodDrinking : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		base.Update(npc, ref buffIndex);
	}
}
