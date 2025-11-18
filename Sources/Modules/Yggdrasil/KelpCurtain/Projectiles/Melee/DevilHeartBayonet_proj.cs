using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Melee;

public class DevilHeartBayonet_proj : StabbingProjectile
{
	public const int HealAmountOnStaminaDepleted = 5;

	public override void SetDefaults()
	{
		base.SetDefaults();

		AttackColor = new Color(255, 107, 171);
		MaxOldAttackUnitCount = 4;
		OldShade = 0.3f;
		Shade = 0.2f;
		ShadeMultiplicative_Modifier = 0.64f;
		ScaleMultiplicative_Modifier = 1;
		OldLightColorValue = 1f;
		LightColorValueMultiplicative_Modifier = 0.4f;
		AttackLength = 1.05f;
		AttackEffectWidth = 0.4f;
	}

	public override void OnStaminaDepleted(Player player)
	{
		player.Heal(HealAmountOnStaminaDepleted);
	}
}