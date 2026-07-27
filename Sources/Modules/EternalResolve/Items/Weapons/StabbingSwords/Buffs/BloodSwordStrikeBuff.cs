namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Buffs;

public class BloodSwordStrikeBuff : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.buffTime[buffIndex] += 1;
		player.lifeRegen += 2;
	}
}