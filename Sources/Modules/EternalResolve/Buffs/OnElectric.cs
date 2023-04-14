namespace Everglow.EternalResolve.Buffs;

public class OnElectric : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		int BuffDamage = 35;
		npc.lifeRegenExpectedLossPerSecond = 7;
		npc.lifeRegen = -BuffDamage;
		Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric);
		base.Update(npc, ref buffIndex);
	}
}
