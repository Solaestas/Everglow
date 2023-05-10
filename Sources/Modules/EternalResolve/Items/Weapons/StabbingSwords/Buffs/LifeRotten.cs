namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Buffs;

public class LifeRotten : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		npc.buffTime[buffIndex] += 1;
	}
}