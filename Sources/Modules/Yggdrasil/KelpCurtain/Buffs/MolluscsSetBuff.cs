namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class MolluscsSetBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.breathEffectiveness += 1f; // Increases breath time by 100%.

		if (player.wet)
		{
			player.moveSpeed += 0.35f; // Increases movement speed by 35% when the player is currently in water.
		}
	}
}