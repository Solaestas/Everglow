namespace Everglow.Commons.TileHelper;

public class HangingTile_Player : ModPlayer
{
	public int SwitchVineCoolTimer = 0;

	public override void PreUpdate()
	{
		base.PreUpdate();
	}

	public override void PostUpdate()
	{
		if (SwitchVineCoolTimer > 0)
		{
			SwitchVineCoolTimer--;
		}
		else
		{
			SwitchVineCoolTimer = 0;
		}
		base.PostUpdate();
	}
}