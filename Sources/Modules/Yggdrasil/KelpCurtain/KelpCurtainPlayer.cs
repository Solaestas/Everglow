using Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;

namespace Everglow.Yggdrasil.KelpCurtain;

public class KelpCurtainPlayer : ModPlayer
{
	/// <summary>
	/// <see cref="Items.Armors.Molluscs.MolluscsLeggings"/>
	/// </summary>
	public bool MolluscsLeggings { get; set; }

	/// <summary>
	/// Set buff symbol of <see cref="Items.Armors.Molluscs.MolluscsLeggings"/>.
	/// </summary>
	public bool MolluscsSetBuff { get; set; }

	/// <summary>
	/// <see cref="Items.Accessories.RadialCarapace"/>
	/// </summary>
	public bool RadialCarapace { get; set; }

	/// <summary>
	/// <see cref="Items.Accessories.CorrodedPearl"/>
	/// </summary>
	public bool CorrodedPearl { get; set; }

	public override void ResetEffects()
	{
		MolluscsLeggings = false;
		MolluscsSetBuff = false;
		RadialCarapace = false;
		CorrodedPearl = false;
	}

	public override void FrameEffects()
	{
		if(Player.head == EquipLoader.GetEquipSlot(Mod, nameof(MossyMolluscsHelmet), EquipType.Head)
			&& Player.body == EquipLoader.GetEquipSlot(Mod, nameof(ShellMolluscsBreastPlate), EquipType.Body))
		{
			Player.body = EquipLoader.GetEquipSlot(Mod, ShellMolluscsBreastPlate.AltTextureKey, EquipType.Body);
		}
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