namespace Everglow.Commons.Utilities.BuffHelpers;

public class ShortImmune3 : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		if (player.immuneTime > 3)
		{
			player.immuneTime = 3;
		}
	}
}