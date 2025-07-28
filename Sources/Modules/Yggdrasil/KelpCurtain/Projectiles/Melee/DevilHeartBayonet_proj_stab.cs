using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Melee;

public class DevilHeartBayonet_proj_stab : StabbingProjectile_Stab
{
	public float Power = 0;

	public override void SetDefaults()
	{
		base.SetDefaults();

		Color = new Color(255, 107, 171);
		TradeShade = 0.7f;
		Shade = 0.2f;
		FadeShade = 0.44f;
		FadeScale = 1;
		TradeLightColorValue = 1f;
		FadeLightColorValue = 0.4f;
		MaxLength = 0.70f;
		DrawWidth = 0.4f;
	}

	public override void OnStaminaDepleted(Player player)
	{
		player.Heal(5);
	}
}