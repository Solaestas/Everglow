namespace Everglow.Commons.TileHelper;

public class HangingTile_Player : ModPlayer
{
	public int SwitchVineCoolTimer = 0;

	public bool Grasping = false;

	public Vector2 OldGraspPos = default;

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
		GraspHangingTile();
		PreventMapFalling();
		if (Grasping)
		{
			OldGraspPos = Player.Center;
		}
		base.PostUpdate();
	}

	public void PreventMapFalling()
	{
		if(Main.mapFullscreen && Grasping)
		{
			if(OldGraspPos != default)
			{
				Player.Center = OldGraspPos;
			}
		}
	}

	public void GraspHangingTile()
	{
		if (!Player.mount.Active && Player.controlUp && !HangingTile.RopeGraspingPlayer.ContainsValue(Player))
		{
			foreach (var hangingTile in TileLoader.tiles.OfType<HangingTile>())
			{
				if (hangingTile.CanGrasp)
				{
					foreach (var rope in hangingTile.RopesOfAllThisTileInTheWorld.Values)
					{
						Vector2 tipPos = rope.Masses.Last().Position;
						if (Vector2.Distance(Player.Center, tipPos) <= hangingTile.GraspDetectRange)
						{
							Point targetPoint = hangingTile.RopesOfAllThisTileInTheWorld.FirstOrDefault(kv => kv.Value == rope).Key;
							hangingTile.AddPlayerToRope(Player, rope, targetPoint);
							Grasping = true;
							break;
						}
					}
					if (HangingTile.RopeGraspingPlayer.ContainsValue(Player))
					{
						break;
					}
				}
			}
		}
	}
}