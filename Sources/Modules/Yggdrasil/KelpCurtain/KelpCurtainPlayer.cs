namespace Everglow.Yggdrasil.KelpCurtain;

public class KelpCurtainPlayer : ModPlayer
{
	public bool MolluscsLeggings { get; set; }

	public bool MolluscsSetBuff { get; set; }

	public bool RadialCarapace { get; set; }

	public bool CorrodedPearl { get; set; }

	public override void ResetEffects()
	{
		MolluscsLeggings = false;
		MolluscsSetBuff = false;
		RadialCarapace = false;
		CorrodedPearl = false;
	}

	public override void UpdateEquips()
	{
		if (Player.wet)
		{
			float multiplier = 1f
				+ (MolluscsSetBuff ? 0.3f : 0f)
				+ (MolluscsLeggings ? 0.35f : 0f)
				+ (RadialCarapace ? 0.35f : 0f)
				+ (CorrodedPearl ? 0.2f : 0f);

			Player.runAcceleration *= multiplier;
			Player.maxRunSpeed *= multiplier;
		}
	}
}