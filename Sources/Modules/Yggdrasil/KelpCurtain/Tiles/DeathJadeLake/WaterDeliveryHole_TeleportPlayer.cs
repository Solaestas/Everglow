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
			bool horizontal = tile.TileType == ModContent.TileType<WaterDeliveryHole>();
			bool vertical = tile.TileType == ModContent.TileType<WaterDeliveryHole_V>();
			bool towardsTop = tile.TileFrameY == 18 && tile.TileFrameX < 90;
			bool towardsDown = tile.TileFrameY == 0 && tile.TileFrameX >= 90;
			bool canTeleport_H = (towardsTop || towardsDown) && (tile.TileFrameX % 90) is >= 18 and < 72 && horizontal;
			bool towardsLeft = tile.TileFrameX == 18;
			bool towardsRight = tile.TileFrameX == 36;
			bool canTeleport_V = (towardsLeft || towardsRight) && tile.TileFrameY is >= 18 and < 72 && vertical;
			float currentRot = 0;
			if (horizontal)
			{
				if (towardsTop)
				{
					currentRot = -MathHelper.PiOver2;
				}
				if (towardsDown)
				{
					currentRot = MathHelper.PiOver2;
				}
			}
			if(vertical)
			{
				if(towardsLeft)
				{
					currentRot = -MathHelper.Pi;
				}
			}
			Vector2 moveDirection = new Vector2(1, 0).RotatedBy(currentRot);
			Rectangle hitBox_vel = player.Hitbox;
			hitBox_vel.X += (int)player.velocity.X;
			hitBox_vel.Y += (int)player.velocity.Y;
			Rectangle hitBox = player.Hitbox;
			Rectangle tileBox = new Rectangle(i * 16, j * 16, 16, 16);
			if ((hitBox.Intersects(tileBox) || hitBox_vel.Intersects(tileBox)) && (canTeleport_H || canTeleport_V))
			{
				Tile currentCenterTile = GetCenterTile(tile);
				float closestDistance = 640;
				Vector2 destination = Vector2.Zero;
				Vector2 currentPos = new Vector2(currentCenterTile.X(), currentCenterTile.Y()) * 16 + new Vector2(8);
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
								float cosTheta = Vector2.Dot(moveDirection, new Vector2(1, 0).RotatedBy(destRotation));
								Vector2 dest = new Vector2(checkCenterTile.X(), checkCenterTile.Y()) * 16 + new Vector2(8);
								float distance = Vector2.Distance(currentPos, dest);
								if (cosTheta > 0.707f)
								{
									distance *= 4;
									distance += 400;
								}
								else if (cosTheta > -0.707f)
								{
									distance *= 2;
									distance += 400;
								}
								if (distance < closestDistance && distance > 32)
								{
									closestDistance = distance;
									destination = dest;
								}
							}
						}
					}
				}
				if (destination != Vector2.zeroVector)
				{
					Teleport(player, destination, destRotation);
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

		Vector2 desVel = new Vector2(4, 0).RotatedBy(-rotation);
		player.Center = destination;
		if (desVel.Y <= -2.82f)
		{
			player.Center += new Vector2(player.height / 2, 0).RotatedBy(-rotation);
		}

		// player.velocity += new Vector2(-4, 0).RotatedBy(rotation);
	}
}