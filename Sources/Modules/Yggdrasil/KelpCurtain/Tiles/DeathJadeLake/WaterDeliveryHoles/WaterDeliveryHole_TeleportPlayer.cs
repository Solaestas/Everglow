using ModLiquidLib.Utils;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.WaterDeliveryHoles;

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

	public void GetDestinationAndTeleport(int i, int j)
	{
		Player player = Main.LocalPlayer;
		WaterDeliveryHole_TeleportPlayer modPlayer = player.GetModPlayer<WaterDeliveryHole_TeleportPlayer>();
		if (!modPlayer.Active)
		{
			Vector2 currentPos = new Vector2(i, j);
			int currentDir = -1;
			var currentTile = TileUtils.SafeGetTile(i, j);
			int currentStyle = TileObjectData.GetTileStyle(currentTile);
			TileObjectData currentObjectData = TileObjectData.GetTileData(currentTile.TileType, currentStyle);
			if(!TileAtOriginPos(currentTile, currentStyle, currentObjectData))
			{
				return;
			}
			Vector2 currentWorldPos = currentPos * 16 + new Vector2(8);
			if ((player.Center - currentWorldPos).Length() >= 32 || (player.Center - currentWorldPos).Length() < (player.Center + player.velocity - currentWorldPos).Length())
			{
				return;
			}

			// x, y, direction
			List<(int, int, int)> destinations = new List<(int, int, int)>();
			for (int x = -40; x <= 40; x += 1)
			{
				for (int y = -40; y <= 40; y += 1)
				{
					var tile = TileUtils.SafeGetTile(i + x, j + y);
					int style = TileObjectData.GetTileStyle(tile);
					if (style >= 0)
					{
						int targetType = tile.TileType;
						TileObjectData tileObjectData = TileObjectData.GetTileData(targetType, style);
						int dir = GetDirection(tile, style);
						if (x == 0 && y == 0)
						{
							currentDir = dir;
							continue;
						}
						if (TileAtOriginPos(tile, style, tileObjectData) && dir >= 0)
						{
							if (!destinations.Contains((tile.X(), tile.Y(), dir)))
							{
								destinations.Add((tile.X(), tile.Y(), dir));
							}
						}
					}
				}
			}
			if (destinations.Count > 0)
			{
				Vector2 closestDest = new Vector2(i + 100, j + 100);
				int bestDir = -1;
				float maxDirDis = 0;
				foreach (var dest in destinations)
				{
					Vector2 directionDis = new Vector2(1, 0).RotatedBy(dest.Item3 * MathHelper.PiOver4) - new Vector2(1, 0).RotatedBy(currentDir * MathHelper.PiOver4);
					if (directionDis.Length() > maxDirDis - 0.1f)
					{
						maxDirDis = directionDis.Length();
						bestDir = dest.Item3;
					}
				}
				foreach (var dest in destinations)
				{
					if(dest.Item3 != bestDir)
					{
						continue;
					}
					Vector2 destination = new Vector2(dest.Item1, dest.Item2);
					float dis = (currentPos - destination).Length();
					if (dis < (closestDest - currentPos).Length())
					{
						closestDest = destination;
					}
				}
				if ((currentPos - closestDest).Length() < 40)
				{
					Teleport(player, closestDest * 16 + new Vector2(8), bestDir * MathHelper.PiOver4);
				}
			}
		}
	}

	public int GetDirection(Tile tile, int style)
	{
		int targetType = tile.TileType;
		int dir = -1;
		if (targetType == ModContent.TileType<WaterDeliveryHole>())
		{
			if (style == 0)
			{
				dir = 6;
			}
			else
			{
				dir = 2;
			}
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_V>())
		{
			if (style == 0)
			{
				dir = 4;
			}
			else
			{
				dir = 0;
			}
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_BottomRight>())
		{
			dir = 1;
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_BottomLeft>())
		{
			dir = 3;
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_TopLeft>())
		{
			dir = 5;
		}
		if (targetType == ModContent.TileType<WaterDeliveryHole_TopRight>())
		{
			dir = 7;
		}
		return dir;
	}

	public bool TileAtOriginPos(Tile tile, int style, TileObjectData tileObjectData)
	{
		return tile.TileFrameX - style * tileObjectData.Width * 18 == tileObjectData.Origin.X * 18 && tile.TileFrameY == tileObjectData.Origin.Y * 18;
	}

	public Tile GetCenterTile(Tile checkTile)
	{
		return GetCenterTile(checkTile.X(), checkTile.Y());
	}

	public Tile GetCenterTile(int i, int j)
	{
		Tile tile = TileUtils.SafeGetTile(i, j);
		int currentOffsetX = 0;
		int currentOffsetY = 0;
		bool fail = true;
		if (tile.TileType == ModContent.TileType<WaterDeliveryHole_V>())
		{
			currentOffsetX = -tile.TileFrameX / 18 + 1;
			if (tile.TileFrameX >= 36)
			{
				currentOffsetX = -(tile.TileFrameX % 36) / 18;
			}
			currentOffsetY = -tile.TileFrameY / 18 + 2;
			fail = false;
		}
		else if (tile.TileType == ModContent.TileType<WaterDeliveryHole>())
		{
			currentOffsetX = -(tile.TileFrameX % 90) / 18 + 2;
			currentOffsetY = -tile.TileFrameY / 18 + 1;
			fail = false;
		}
		if (fail)
		{
			Main.NewText("Fail to access target", Color.Red);
		}
		return TileUtils.SafeGetTile(i + currentOffsetX, j + currentOffsetY);
	}

	public void Teleport(Player player, Vector2 destination, float rotation)
	{
		WaterDeliveryHole_TeleportPlayer modPlayer = player.GetModPlayer<WaterDeliveryHole_TeleportPlayer>();
		modPlayer.Active = true;
		modPlayer.Timer = modPlayer.MaxTeleportTime;
		modPlayer.OldPos = Main.screenPosition;

		Vector2 desVel = new Vector2(4, 0).RotatedBy(-rotation);
		player.Center = destination;
		if (desVel.Y <= -2.82f)
		{
			player.Center += new Vector2(player.height / 2, 0).RotatedBy(-rotation) + player.velocity;
		}

		// player.velocity += new Vector2(-4, 0).RotatedBy(rotation);
	}
}