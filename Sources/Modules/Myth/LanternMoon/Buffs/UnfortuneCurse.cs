namespace Everglow.Myth.LanternMoon.Buffs;

public class UnfortuneCurse : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = false;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.luck -= 100;
		player.GetCritChance(DamageClass.Generic) -= 1f;
		base.Update(player, ref buffIndex);
	}
}