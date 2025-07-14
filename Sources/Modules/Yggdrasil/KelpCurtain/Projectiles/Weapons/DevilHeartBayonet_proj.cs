using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Weapons;

public class DevilHeartBayonet_proj : StabbingProjectile
{
	public const int HealAmountOnStaminaDepleted = 5;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Color = new Color(255, 107, 171);
		TradeLength = 4;
		TradeShade = 0.3f;
		Shade = 0.2f;
		FadeShade = 0.64f;
		FadeScale = 1;
		TradeLightColorValue = 1f;
		FadeLightColorValue = 0.4f;
		MaxLength = 1.05f;
		DrawWidth = 0.4f;
	}

	public override void OnStaminaDepleted(Player player)
	{
		player.Heal(HealAmountOnStaminaDepleted);
	}
}