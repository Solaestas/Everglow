namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class DevilHeartGyroscopeBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
	}
}