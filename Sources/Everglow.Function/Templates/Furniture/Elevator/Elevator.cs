using Everglow.Commons.CustomTiles.Core;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class Elevator : BoxEntity
{
	public enum State
	{
		Stop,
		Move,
	}

	/// <summary>
	/// Winch tile type. Set by <see cref="WinchTile{TElevator}"/> automatically.
	/// </summary>
	public int WinchTileType;

	/// <summary>
	/// Coordinate of winch tile in <see cref="Main.tile"/>. Set by <see cref="WinchTile{TElevator}"/> automatically.
	/// </summary>
	public Point WinchCoord;

	/// <summary>
	/// Move state.
	/// <br/>Default to <see cref="State.Stop"/>.
	/// </summary>
	public State MoveState = State.Stop;

	/// <summary>
	/// Minimum distance to winch tile.
	/// <br/>When the elevator move exceed this value to the winch, elevator will return forcefully.
	/// <br/>Defaults to <see cref="float.PositiveInfinity"/>.
	/// </summary>
	public float LengthRestrict = float.PositiveInfinity;

	public float CurrentSpeed = 0f;

	public float MaxSpeed = 5f;

	/// <summary>
	/// During accelerating / decelerating, the <see cref="CurrentSpeed"/> will +/- this value per tick.
	/// </summary>
	public float Acceleration = 0.1f;

	/// <summary>
	/// The direction this elevator is moving. A value of 1 means the elevator is moving downward. -1 means moving upward.
	/// </summary>
	public int CurrentMoveDirection { get; protected set; } = 1;

	public int NextStopTileY { get; protected set; } = -1;

	/// <summary>
	/// Timer for a stop elevator.
	/// </summary>
	public int StopTimer { get; protected set; } = 0;

	public abstract string ElevatorTexture { get; }

	public abstract string ElevatorCableTexture { get; }

	public virtual int ElevatorCableJointOffset => 0;

	public override Color MapColor => new Color(122, 91, 79);

	#region Behavior

	public override void AI()
	{
		if (TileUtils.SafeGetTile(WinchCoord).TileType != WinchTileType)
		{
			Kill();
			return;
		}

		if (MoveState == State.Stop)
		{
			if (StopTimer > 0)
			{
				StopTimer--;
			}
			if (StopTimer <= 0)
			{
				StopTimer = 0;
				MoveState = State.Move;
				if (!CanMove(CurrentMoveDirection))
				{
					CurrentMoveDirection *= -1;
					NextStopTileY = GetNextStopY(CurrentMoveDirection);
				}
				else
				{
					NextStopTileY = GetNextStopY(CurrentMoveDirection);
				}
			}
			Velocity = Vector2.Zero;
		}
		else if (MoveState == State.Move)
		{
			var distToTargetY = Math.Abs(NextStopTileY * 16 - (CurrentMoveDirection == 1 ? Box.Bottom : Box.Top));
			if (distToTargetY <= 1f)
			{
				StopElevator(300);

				return;
			}

			float requiredDecelerateDistance = (CurrentSpeed * CurrentSpeed) / (2 * Acceleration);
			if (distToTargetY <= requiredDecelerateDistance)
			{
				// Decelerate
				CurrentSpeed -= Acceleration;

				if (CurrentSpeed < 0f)
				{
					CurrentSpeed = 0f;
				}
			}
			else if (CurrentSpeed < MaxSpeed)
			{
				// Accelerate
				CurrentSpeed += Acceleration;

				if (CurrentSpeed > MaxSpeed)
				{
					CurrentSpeed = MaxSpeed;
				}
			}
			else
			{
				// Max Speed Uniform Motion
				CurrentSpeed = MaxSpeed;
			}

			Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);

			Point nextPosTileCoord = Position.ToTileCoordinates();
			if (CurrentMoveDirection == 1)
			{
				nextPosTileCoord.Y++;
			}
			Point size = Size.ToTileCoordinates();
			if (TileUtils.AreaHasTile(nextPosTileCoord.X, nextPosTileCoord.Y, size.X, size.Y,
					tile => Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]))
			{
				StopElevator(120);
			}
		}
		else
		{
			throw new NotImplementedException($"New state {MoveState} is not handled.");
		}
	}

	public void StopElevator(int time)
	{
		StopTimer = time;
		CurrentSpeed = 0;
		Velocity = Vector2.Zero;
		MoveState = State.Stop;
	}

	private int GetNextTileOnPathY(int dir)
	{
		int startTileX = Position.X.ToTileCoordinate();
		int startTileY = dir == 1 ? Box.Bottom.ToTileCoordinate() : Box.Top.ToTileCoordinate();
		var a = Box.Bottom.ToTileCoordinate();
		var b = Box.Top.ToTileCoordinate();
		int scanRange = 2000;
		if (LengthRestrict is not float.PositiveInfinity && LengthRestrict > 0)
		{
			scanRange = Math.Min(scanRange, LengthRestrict.ToTileCoordinate());
		}
		for (int i = 0; i < scanRange; i++)
		{
			int y = startTileY + (i * dir);

			// Check world bounds
			if (y < 0 || y >= Main.maxTilesY)
			{
				return y;
			}

			// 1. Check Path Collision
			for (int j = 0; j < Size.ToTileCoordinates().X; j++)
			{
				Tile pathTile = Framing.GetTileSafely(startTileX + j, dir == 1 ? y : y);
				if (pathTile.HasTile && (Main.tileSolid[pathTile.TileType] || Main.tileSolidTop[pathTile.TileType]))
				{
					return dir == 1 ? y : y + 1;
				}
			}

			// 2. Check Winch Tile
			var winchLimitation = WinchCoord.Y * 16 + 8 + 224;
			var predictY = y * 16;
			if (dir == -1 && predictY < winchLimitation)
			{
				return y;
			}
		}

		return startTileY + scanRange * dir;
	}

	/// <summary>
	/// Scans the tile map in the movement direction to find the Y pixel coordinate of the next stop.
	/// Stops are caused by: Solid tiles on path OR Side indicators.
	/// </summary>
	/// <returns>The Y tile coordinate of the next stop.</returns>
	private int GetNextStopY(int dir)
	{
		int startTileX = Position.X.ToTileCoordinate();
		int startTileY = dir == 1 ? Box.Bottom.ToTileCoordinate() : Box.Top.ToTileCoordinate();

		int scanRange = 2000;
		if (LengthRestrict is not float.PositiveInfinity && LengthRestrict > 0)
		{
			scanRange = Math.Min(scanRange, LengthRestrict.ToTileCoordinate());
		}
		for (int i = 1; i < scanRange; i++)
		{
			int y = startTileY + (i * dir);

			// Check world bounds
			if (y < 0 || y >= Main.maxTilesY)
			{
				return y;
			}

			// 1. Check Path Collision
			for (int j = 0; j < Size.ToTileCoordinates().X; j++)
			{
				Tile pathTile = Framing.GetTileSafely(startTileX + j, dir == 1 ? y : y);
				if (pathTile.HasTile && (Main.tileSolid[pathTile.TileType] || Main.tileSolidTop[pathTile.TileType]))
				{
					return dir == 1 ? y : y;
				}
			}

			// 2. Check Winch Tile
			var winchLimitation = WinchCoord.Y * 16 + 8 + 224;
			var predictY = y * 16;
			if (dir == -1 && predictY < winchLimitation)
			{
				return y;
			}

			// 3. Check Floor Indicators (Left or Right of path)
			int leftX = Box.Left.ToTileCoordinate() - 1;
			int rightX = Box.Right.ToTileCoordinate();

			Tile leftTile = Framing.GetTileSafely(leftX, y);
			Tile rightTile = Framing.GetTileSafely(rightX, y);

			bool isFloor = (leftTile.HasTile && leftTile.IsType<FloorIndicatorTile>()) ||
						   (rightTile.HasTile && rightTile.IsType<FloorIndicatorTile>());

			if (isFloor)
			{
				return y + (dir == 1 ? Size.Y.ToTileCoordinate() : 0);
			}
		}

		return startTileY + scanRange * dir;
	}

	/// <summary>
	/// Checks if the elevator can begin moving in a specific direction without immediately colliding.
	/// </summary>
	private bool CanMove(int dir)
	{
		int target = GetNextTileOnPathY(dir);
		float dist = Math.Abs(target * 16 - (dir == 1 ? Box.Bottom : Box.Top));
		return dist > 4f;
	}

	#endregion

	#region Draw

	public override void Draw()
	{
		if (Position.X / 16f < Main.maxTilesX - 28 && Position.Y / 16f < Main.maxTilesY - 28 && Position.X / 16f > 28 && Position.Y / 16f > 28)
		{
			Color lightColor = Lighting.GetColor(Box.Center.ToTileCoordinates());

			if (PreDrawElevatorCable(lightColor))
			{
				DrawElevatorCable(lightColor);
				PostDrawElevatorCable(lightColor);
			}

			if (PreDrawElevator(lightColor))
			{
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>(ElevatorTexture).Value, Position - Main.screenPosition, new Rectangle(0, 0, (int)Size.X, (int)Size.Y), lightColor);
				PostDrawElevator(lightColor);
			}
		}
	}

	public virtual bool PreDrawElevator(Color lightColor) => true;

	public virtual void PostDrawElevator(Color lightColor)
	{
	}

	public virtual void DrawElevatorCable(Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		for (int f = 0; f < 1000; f++)
		{
			var jointPos = new Vector2(0, -ElevatorCableJointOffset - f * 12);
			Vector2 drawPos = Box.Center - Main.screenPosition;
			Color drawcRope = Lighting.GetColor((int)(Position.X / 16f) + 2, (int)((Position.Y - f * 12) / 16f) - 7);

			bars.Add(drawPos + jointPos + new Vector2(-4, 0), drawcRope, new Vector3(0, f, 0));
			bars.Add(drawPos + jointPos + new Vector2(4, 0), drawcRope, new Vector3(1, f, 0));
			if ((jointPos + Box.Center).Y < WinchCoord.Y * 16 + 8)
			{
				break;
			}
		}
		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(ElevatorCableTexture).Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public virtual bool PreDrawElevatorCable(Color lightColor) => true;

	public virtual void PostDrawElevatorCable(Color lightColor)
	{
	}

	#endregion

	public void StepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
	{
		int dir = 0;
		if (velocity.X < 0f)
		{
			dir = -1;
		}
		if (velocity.X > 0f)
		{
			dir = 1;
		}
		Vector2 nextPos = position;
		nextPos.X += velocity.X;
		int tilePosFront = (int)((nextPos.X + width / 2 + (width / 2 + 1) * dir) / 16f);
		int tilePosY = (int)((nextPos.Y + 0.1) / 16.0);
		if (gravDir == 1)
		{
			tilePosY = (int)((nextPos.Y + height - 1f) / 16f);
		}
		int heightToTilePos = height / 16 + ((height % 16 != 0) ? 1 : 0);
		bool flag = true;
		bool flag2 = true;
		if (Main.tile[tilePosFront, tilePosY] == null)
		{
			return;
		}
		for (int i = 1; i < heightToTilePos + 2; i++)
		{
			if (!WorldGen.InWorld(tilePosFront, tilePosY - i * gravDir, 0) || Main.tile[tilePosFront, tilePosY - i * gravDir] == null)
			{
				return;
			}
		}
		if (!WorldGen.InWorld(tilePosFront - dir, tilePosY - heightToTilePos * gravDir, 0) || Main.tile[tilePosFront - dir, tilePosY - heightToTilePos * gravDir] == null)
		{
			return;
		}
		Tile tile;
		for (int j = 2; j < heightToTilePos + 1; j++)
		{
			if (!WorldGen.InWorld(tilePosFront, tilePosY - j * gravDir, 0) || Main.tile[tilePosFront, tilePosY - j * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[tilePosFront, tilePosY - j * gravDir];

			// No tile or non-solid tile or solid top tile (platform) is ok
			flag = flag && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		}
		tile = Main.tile[tilePosFront - dir, tilePosY - heightToTilePos * gravDir];

		// No tile or non-solid tile or solid top tile (platform) is ok
		flag2 = flag2 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		Tile tile2;
		if (gravDir == 1)
		{
			if (Main.tile[tilePosFront, tilePosY - gravDir] == null || Main.tile[tilePosFront, tilePosY - (heightToTilePos + 1) * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[tilePosFront, tilePosY - gravDir];
			tile2 = Main.tile[tilePosFront, tilePosY - (heightToTilePos + 1) * gravDir];

			// No tile or non-solid tile or solid top tile (platform) or sloped tile that doesn't block the path is ok. If it's a half brick, then the tile below it also needs to be non-solid or solid top tile (platform).
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || (tile.slope() == 1 && position.X + width / 2 > tilePosFront * 16) || (tile.slope() == 2 && position.X + width / 2 < tilePosFront * 16 + 16) || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[tilePosFront, tilePosY];
			tile2 = Main.tile[tilePosFront, tilePosY - 1];
			if (specialChecksMode == 1)
			{
				flag5 = !TileID.Sets.IgnoredByNpcStepUp[tile.type];
			}

			// Active tile that is solid and not a solid top tile (platform) is not ok. A sloped tile is not ok if the slope would block the path. If it's a solid top tile (platform) that can be stood on, then the tile below it needs to be non-solid or non-active. If it's a half brick, then it's not ok.
			flag4 = flag4 && ((tile.nactive() && (!tile.topSlope() || (tile.slope() == 1 && position.X + width / 2 < tilePosFront * 16) || (tile.slope() == 2 && position.X + width / 2 > tilePosFront * 16 + 16)) && (!tile.topSlope() || position.Y + height > tilePosY * 16) && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && ((Main.tileSolidTop[tile.type] && tile.frameY == 0) || TileID.Sets.Platforms[tile.type]) && (!Main.tileSolid[tile2.type] || !tile2.nactive()) && flag5))) || (tile2.halfBrick() && tile2.nactive()));
			flag4 &= !Main.tileSolidTop[tile.type] || !Main.tileSolidTop[tile2.type];
		}
		else
		{
			tile = Main.tile[tilePosFront, tilePosY - gravDir];
			tile2 = Main.tile[tilePosFront, tilePosY - (heightToTilePos + 1) * gravDir];
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || tile.slope() != 0 || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[tilePosFront, tilePosY];
			tile2 = Main.tile[tilePosFront, tilePosY + 1];
			flag4 = flag4 && ((tile.nactive() && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && Main.tileSolidTop[tile.type] && tile.frameY == 0 && (!Main.tileSolid[tile2.type] || !tile2.nactive())))) || (tile2.halfBrick() && tile2.nactive()));
		}
		if (tilePosFront * 16 >= nextPos.X + width || tilePosFront * 16 + 16 <= nextPos.X)
		{
			return;
		}
		if (gravDir == 1)
		{
			if (!flag4 || !flag3 || !flag || !flag2)
			{
				return;
			}
			float tilePosY_World = tilePosY * 16;
			if (Main.tile[tilePosFront, tilePosY - 1].halfBrick())
			{
				tilePosY_World -= 8f;
			}
			else if (Main.tile[tilePosFront, tilePosY].halfBrick())
			{
				tilePosY_World += 8f;
			}
			if (tilePosY_World >= nextPos.Y + height)
			{
				return;
			}
			float MoveUp = nextPos.Y + height - tilePosY_World;
			if ((double)MoveUp <= 16.1)
			{
				gfxOffY += position.Y + height - tilePosY_World;
				position.Y = tilePosY_World - height;
				if (MoveUp < 9f)
				{
					stepSpeed = 1f;
					return;
				}
				stepSpeed = 2f;
				return;
			}
		}
		else
		{
			if (!flag4 || !flag3 || !flag || !flag2 || Main.tile[tilePosFront, tilePosY].bottomSlope() || TileID.Sets.Platforms[tile2.type])
			{
				return;
			}
			float tilePosY_World_1_Below = tilePosY * 16 + 16;
			if (tilePosY_World_1_Below <= nextPos.Y)
			{
				return;
			}
			float MoveUp_Grav = tilePosY_World_1_Below - nextPos.Y;
			if ((double)MoveUp_Grav <= 16.1)
			{
				gfxOffY -= tilePosY_World_1_Below - position.Y;
				position.Y = tilePosY_World_1_Below;
				velocity.Y = 0f;
				if (MoveUp_Grav < 9f)
				{
					stepSpeed = 1f;
					return;
				}
				stepSpeed = 2f;
			}
		}
	}
}