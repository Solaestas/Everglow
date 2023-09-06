using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Buffs;

public class StaminaEnergyBuff : ModBuff
{
	//加快体力恢复速度的20%
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = false;
		Main.buffNoTimeDisplay[Type] = false;
	}

	public override void Update(Player player, ref int buffIndex)
	{
		PlayerStamina staminaPlayer = Main.LocalPlayer.GetModPlayer<PlayerStamina>();
		staminaPlayer.mulStaminaRecoveryValue += 0.2f;
	}
}
