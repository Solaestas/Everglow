namespace Everglow.Yggdrasil.YggdrasilTown.Buffs;

public class Charred : ModBuff
{
	public const int DefenseDecrease = 25;
	public const int DotDamage = 36;

	public override void SetStaticDefaults()
	{
		Main.pvpBuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		npc.defense -= DefenseDecrease;
		if (npc.defense < 0)
		{
			npc.defense = 0;
		}

		npc.lifeRegen -= DotDamage;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.statDefense -= DefenseDecrease;
		player.lifeRegen -= DotDamage;
	}
}