using Everglow.Commons.Templates.Weapons.StabbingSwords;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Melee;

public class DarkMassacreDagger_Projectile_thrust : StabbingProjectile_Stab
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		Color = new Color(209, 55, 7);
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

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		Player player = Main.player[Projectile.owner];
		float distanceValue = (player.Center - target.Center).Length();
		modifiers.FinalDamage += Math.Max(0, 180 - distanceValue) / 180f;
	}

	public override void AI()
	{
		base.AI();
	}

	public override void HitTile()
	{
		// SoundEngine.PlaySound(SoundID.Dig.WithPitchOffset(Main.rand.NextFloat(0.6f, 1f)), Projectile.Center);
		base.HitTile();
	}
}