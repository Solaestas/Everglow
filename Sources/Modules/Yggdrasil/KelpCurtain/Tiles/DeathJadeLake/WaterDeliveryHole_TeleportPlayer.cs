namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class WaterDeliveryHole_TeleportPlayer : ModPlayer
{
	public bool Active = false;

	public Vector2 OldPos;

	public float Timer;

	public float MaxTeleportTime = 30;

	public override void ModifyScreenPosition()
	{
		if (Active)
		{
			if (Timer > 0)
			{
				Timer--;
				float value = 1f - Timer / MaxTeleportTime;
				Main.screenPosition = value.Lerp(OldPos, Main.screenPosition);
			}
			else
			{
				Timer = 0;
				Active = false;
			}
		}
		base.ModifyScreenPosition();
	}
}