using Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class TheGreenSolarBuff : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		// Remove this buff if the Player has more than [?] life.
		if (player.statLife >= TheGreenSolar.DispelBuffLife)
		{
			player.ClearBuff(Type);
			return;
		}

		player.buffTime[buffIndex]++; // Preserve the buff time.
	}
}