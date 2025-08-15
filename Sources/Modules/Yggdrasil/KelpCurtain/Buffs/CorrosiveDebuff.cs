namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class CorrosiveDebuff : ModBuff
{
	public const float MoveSpeedReduction = 0.15f;
	public const int DefenseReduction = 10;

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.moveSpeed -= MoveSpeedReduction;
		player.statDefense -= DefenseReduction;
	}
}