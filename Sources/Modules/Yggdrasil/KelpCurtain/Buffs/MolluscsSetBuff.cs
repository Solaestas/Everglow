namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class MolluscsSetBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.breathEffectiveness += 1f; // Increase breath time by 100%.
		player.GetModPlayer<KelpCurtainPlayer>().MolluscsSetBuff = true; // Increase max speed and acceleration by 30% when the player is currently in water.
	}
}