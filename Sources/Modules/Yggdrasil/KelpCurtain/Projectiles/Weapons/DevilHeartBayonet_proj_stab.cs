using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Weapons
{
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

		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}

		public override void AI()
		{
			base.AI();
		}

		public override void OnKill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if (player.GetModPlayer<PlayerStamina>().staminaRecovery)
			{
				player.Heal(5);
			}
			base.OnKill(timeLeft);
		}
	}
}