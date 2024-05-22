using Everglow.Commons.Utilities;

namespace Everglow.IIID.Projectiles.PlanetBefall;

// TODO 改用Shaker
public class PlanetBeFallScreenMovePlayer : ModPlayer
{
	public int AnimationTimer = 0;
	public bool PlanetBeFallAnimation = false;
	public Projectile proj;
	private const float MaxTime = 135;

	public override void ModifyScreenPosition()
	{
		if (proj == null || proj.owner != Player.whoAmI)
		{
			return;
		}

		if (!PlanetBeFallAnimation)
		{
			return;
		}

		Vector2 target = proj.Center - Main.ScreenSize.ToVector2() / 2;
		Player.immune = true;
		Player.immuneTime = 60;
		AnimationTimer += 1;
		float Value = (1 - MathF.Cos(AnimationTimer * MathF.PI / 45)) / 2f;
		if (AnimationTimer >= 45 && AnimationTimer < 90)
		{
			Value = 1;
		}
		if (AnimationTimer >= 90)
		{
			Value = (1 + MathF.Cos((AnimationTimer - 90) * MathF.PI / 45)) / 2f;
		}

		if (AnimationTimer >= MaxTime)
		{
			AnimationTimer = (int)MaxTime;
			PlanetBeFallAnimation = false;
		}

		Main.screenPosition = Value.Lerp(Main.screenPosition, target);
	}
}