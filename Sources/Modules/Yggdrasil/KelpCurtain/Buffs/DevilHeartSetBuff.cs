namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class DevilHeartSetBuff : ModBuff
{
	public override string Texture => Commons.ModAsset.BuffTemplate_Mod;

	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		player.GetDamage<MagicDamageClass>() += 0.06f; // Increases magic damage by 6%
		player.GetCritChance<MagicDamageClass>() += 0.06f; // Increases summon damage by 6%
		player.slotsMinions += 1; // Increases the number of minions the player can summon by 1

		// Bans mana regeneration
		player.manaRegenBonus = -100;
		player.manaRegen = 0;
		player.manaRegenCount = 0;

		player.statDefense *= 1f - 0.15f; // Reduces defense by 15%
	}
}