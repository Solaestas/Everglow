using ModLiquidLib.Utils;

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

	public void GetDestinationAndTeleport(int i, int j)
	{
		Player player = Main.LocalPlayer;
		WaterDeliveryHole_TeleportPlayer modPlayer = player.GetModPlayer<WaterDeliveryHole_TeleportPlayer>();
		if (!modPlayer.Active)
		{
			Tile tile = TileUtils.SafeGetTile(i, j);
			if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)) && (tile.TileFrameX == 18 || tile.TileFrameX == 36) && tile.TileFrameY is >= 18 and < 72)
			{
				Tile currentCenterTile = GetCenterTile(tile);
				float closestDistance = 1000;
				Vector2 destination = Vector2.Zero;
				Vector2 currentPos = new Vector2(currentCenterTile.X(), currentCenterTile.Y()) * 16 + new Vector2(8);
				Main.NewText(currentPos, Color.Yellow);
				float destRotation = 0;
				for (int x = -40; x <= 40; x += 2)
				{
					for (int y = -40; y <= 40; y += 2)
					{
						var checkTile = TileUtils.SafeGetTile(i + x, j + y);
						if (checkTile.TileType == ModContent.TileType<WaterDeliveryHole_V>() || checkTile.TileType == ModContent.TileType<WaterDeliveryHole>())
						{
							var checkCenterTile = GetCenterTile(checkTile);
							if (checkCenterTile != null && checkCenterTile.HasTile)
							{
								Vector2 dest = new Vector2(checkCenterTile.X(), checkCenterTile.Y()) * 16 + new Vector2(8);
								float distance = Vector2.Distance(currentPos, dest);
								if (distance < closestDistance && distance > 32)
								{
									closestDistance = distance;
									destination = dest;
									if (checkCenterTile.TileType == ModContent.TileType<WaterDeliveryHole_V>())
									{
										destRotation = -MathHelper.Pi;
										if (checkCenterTile.TileFrameX == 36)
										{
											destRotation = 0;
										}
									}

									if (checkCenterTile.TileType == ModContent.TileType<WaterDeliveryHole>())
									{
										destRotation = -MathHelper.PiOver2;
										if (checkCenterTile.TileFrameX == 36)
										{
											destRotation = MathHelper.PiOver2;
										}
									}
								}
							}
						}
					}
				}
				if (destination != Vector2.zeroVector)
				{
					Teleport(player, destination, destRotation);
					Main.NewText(destination);
				}
			}
		}
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

		player.Center = destination;
		player.velocity += new Vector2(4, 0).RotatedBy(rotation);
	}
}