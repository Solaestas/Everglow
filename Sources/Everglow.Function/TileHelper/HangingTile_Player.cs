using static Terraria.Utilities.NPCUtils;

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
		if(!Player.mount.Active && Player.controlUp && !HangingTile.RopeGraspingPlayer.ContainsValue(Player))
		{
			foreach (var hangingTile in TileLoader.tiles.OfType<HangingTile>())
			{
				foreach (var rope in hangingTile.RopesOfAllThisTileInTheWorld.Values)
				{
					Vector2 tipPos = rope.Masses.Last().Position;
					if(Vector2.Distance(Player.Center, tipPos) <= hangingTile.GraspDetectRange)
					{
						Point targetPoint = hangingTile.RopesOfAllThisTileInTheWorld.FirstOrDefault(kv => kv.Value == rope).Key;
						hangingTile.AddPlayerToRope(Player, rope, targetPoint);
						break;
					}
				}
				if (HangingTile.RopeGraspingPlayer.ContainsValue(Player))
				{
					break;
				}
			}
		}
		base.PostUpdate();
	}
}