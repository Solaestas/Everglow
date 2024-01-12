namespace Everglow.Myth.MagicWeaponsReplace.Buffs;

public class WaterBoltII : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
		Main.buffNoTimeDisplay[Type] = false;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		// if the minions exist reset the buff time, otherwise remove the buff from the player.
	}
}