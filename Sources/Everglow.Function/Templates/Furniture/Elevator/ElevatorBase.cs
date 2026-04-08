using Everglow.Commons.CustomTiles;
using Everglow.Commons.CustomTiles.Core;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using ReLogic.Content;
using Terraria.Audio;

namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class ElevatorBase : BoxEntity
{
	public enum State
	{
		Stop,
		Move,
	}

	/// <summary>
	/// Winch tile type. Set by <see cref="WinchTileBase{TElevator}"/> automatically.
	/// </summary>
	public int WinchTileType;

	/// <summary>
	/// Coordinate of winch tile in <see cref="Main.tile"/>. Set by <see cref="WinchTileBase{TElevator}"/> automatically.
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

	/// <summary>
	/// No more than 50f.
	/// </summary>
	public float MaxSpeed = 5f;

	/// <summary>
	/// During accelerating / decelerating, the <see cref="CurrentSpeed"/> will +/- this value per tick.<br/>
	/// No less than 0.15f.
	/// </summary>
	public float Acceleration = 0.25f;

	/// <summary>
	/// The direction this elevator is moving. A value of 1 means the elevator is moving downward. -1 means moving upward.
	/// </summary>
	public int CurrentMoveDirection { get; protected set; } = 1;

	public int NextStopTileY { get; protected set; } = -1;

	public int AccelerateDirection = 0;

	/// <summary>
	/// Time that the elevator will stop when it reaches a stop point. Default to 300 (5 seconds).
	/// </summary>
	public int StopTimeMax = 300;

	public override Color MapColor => new Color(122, 91, 79);

	public Texture2D AuxiliaryStructureTexture;

	public Texture2D AuxiliaryStructureTextureHighlight;

	/// <summary>
	/// If <see cref="LightSourceOn"/> is true, a Lighting.AddLight will called in <see cref="DrawAuxiliaryStructure"/> to emit light. This is used for elevator with lamps. The position of the light source is relative to the center of the elevator, and can be set by this variable. Default to (0, -60).
	/// </summary>
	public Vector2 LightSourceRelativePos = Vector2.zeroVector;

	/// <summary>
	/// If mouse world inside the box defined by <see cref="HighlightBoxRelativeTopLeft"/> and <see cref="HighlightBoxRelativeBottomRight"/> relative to the center of the elevator, the <see cref="AuxiliaryStructureTextureHighlight"/> will be drawn.
	/// </summary>
	public Vector2 HighlightBoxRelativeTopLeft;

	/// <summary>
	/// If mouse world inside the box defined by <see cref="HighlightBoxRelativeTopLeft"/> and <see cref="HighlightBoxRelativeBottomRight"/> relative to the center of the elevator, the <see cref="AuxiliaryStructureTextureHighlight"/> will be drawn.
	/// </summary>
	public Vector2 HighlightBoxRelativeBottomRight;

	public Vector3 LightSourceColor;

	/// <summary>
	/// Relative to Box.Center.
	/// </summary>
	public Vector2 TopRopePos;

	/// <summary>
	/// In most cases, there is a light source in an elevator.
	/// </summary>
	public bool LightSourceOn = true;

	/// <summary>
	/// If mouse hover the elevator, it will be highlighted. This is used to indicate that the elevator can be interacted with(RightClick).
	/// </summary>
	public bool Highlighted = false;

	public ElevatorHelper LocalElevatorHelper;

	/// <summary>
	/// Timer for a stop elevator.
	/// </summary>
	public int StopTimer { get; protected set; } = 0;

	public Texture2D ElevatorTexture { get; set; }

	public Texture2D ElevatorCableTexture { get; set; }

	public virtual int ElevatorCableJointOffset => 125;

	public override void SetDefaults()
	{
		Size = new Vector2(96, 16);
		HighlightBoxRelativeTopLeft = new Vector2(-56, -94);
		HighlightBoxRelativeBottomRight = new Vector2(56, 8);
		TopRopePos = new Vector2(0, -110);
		LightSourceColor = new Vector3(1f, 0.8f, 0f);
		LightSourceOn = true;
		LocalElevatorHelper = null;
		SetTextures();
	}

	public void SetTextures()
	{
		var mappings = new List<(string Suffix, Action<Texture2D> SetTexture, Texture2D DefaultTexture)>()
		{
			(string.Empty, t => ElevatorTexture = t, ModAsset.DefaultElevator.Value),
			("_Cable", t => ElevatorCableTexture = t, ModAsset.DefaultElevator_Cable.Value),
			("_AuxiliaryStructure", t => AuxiliaryStructureTexture = t, ModAsset.DefaultElevator_AuxiliaryStructure.Value),
			("_AuxiliaryStructure_Highlight", t => AuxiliaryStructureTextureHighlight = t, ModAsset.DefaultElevator_AuxiliaryStructure_Highlight.Value),
		};

		string basePath = GetType().FullName.Replace('.', '/');
		foreach (var (suffix, setTexture, defaultTexture) in mappings)
		{
			var texName = basePath + suffix;
			var path = texName + ".png";
			if (ModContent.FileExists(path))
			{
				setTexture(ModContent.Request<Texture2D>(texName, AssetRequestMode.ImmediateLoad).Value);
			}
			else
			{
				setTexture(defaultTexture);
			}
		}
	}

	#region Behavior

	public override void AI()
	{
		if (TileUtils.SafeGetTile(WinchCoord).TileType != WinchTileType)
		{
			Kill();
			return;
		}
		Highlighted = ShouldHighlight();
		if (Highlighted)
		{
			if (Main.mouseRight && Main.mouseRightRelease)
			{
				SoundEngine.PlaySound(SoundID.Unlock, Main.MouseWorld);
				if (LocalElevatorHelper is null || !LocalElevatorHelper.Active)
				{
					LocalElevatorHelper = new ElevatorHelper
					{
						AnimationTimer = 0,
						ParentElevator = this,
						Owner = Main.LocalPlayer,
						RelativePos = new Vector2(60, -80),
						Visible = true,
						Active = true,
					};
					Ins.VFXManager.Add(LocalElevatorHelper);
					foreach (var customTile in ColliderManager.Instance.OfType<ElevatorBase>())
					{
						if (customTile.LocalElevatorHelper is not null && customTile.LocalElevatorHelper.Active && customTile.LocalElevatorHelper.Owner == Main.LocalPlayer && !customTile.LocalElevatorHelper.Closing && customTile.LocalElevatorHelper != LocalElevatorHelper)
						{
							customTile.LocalElevatorHelper.Closing = true;
						}
					}
				}
				else
				{
					LocalElevatorHelper.Closing = !LocalElevatorHelper.Closing;
				}
			}
		}
		if (!CanClick())
		{
			if (LocalElevatorHelper is not null)
			{
				LocalElevatorHelper.Closing = true;
			}
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
			var distToTargetY = (NextStopTileY * 16 - (CurrentMoveDirection == 1 ? Box.Bottom : Box.Top)) * CurrentMoveDirection;
			if (distToTargetY <= Math.Max(CurrentSpeed, 1))
			{
				if (CurrentSpeed <= CollisionUtils.Epsilon)
				{
					StopElevator(StopTimeMax);
					AccelerateDirection = 0;
				}
				else
				{
					CurrentSpeed = distToTargetY;
					Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);
				}
				return;
			}

			float requiredDecelerateDistance = (CurrentSpeed * CurrentSpeed) / (2 * Acceleration);
			if (AccelerateDirection == 0)
			{
				if (distToTargetY <= requiredDecelerateDistance)
				{
					// Decelerate
					AccelerateDirection = -1;
				}
				else if (CurrentSpeed < MaxSpeed)
				{
					// Accelerate
					AccelerateDirection = 1;
				}
			}
			else
			{
				// Max Speed Uniform Motion
				if (AccelerateDirection == 1)
				{
					if (distToTargetY <= requiredDecelerateDistance)
					{
						// Decelerate
						AccelerateDirection = -1;
					}
				}
				if (CurrentSpeed <= 0.05f)
				{
					AccelerateDirection = 0;
					CurrentSpeed = 0;
				}
				if (CurrentSpeed >= MaxSpeed)
				{
					CurrentSpeed = MaxSpeed;
				}
			}

			switch (AccelerateDirection)
			{
				case -1:
					CurrentSpeed -= Acceleration;
					if (CurrentSpeed < 0f)
					{
						CurrentSpeed = 0f;
					}
					break;
				case 0:
					// Uniform motion, no need to update target
					break;
				case 1:
					// Accelerating, keep target updated in case of unexpected obstacle
					CurrentSpeed += Acceleration;
					if (CurrentSpeed > MaxSpeed)
					{
						CurrentSpeed = MaxSpeed;
					}
					break;
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
				StopElevator(StopTimeMax);
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

		// Debug Code
		// float fixed_NextStopTileY = NextStopTileY;
		// if (CurrentMoveDirection == 1)
		// {
		// fixed_NextStopTileY -= 1;
		// }
		// float delatY = fixed_NextStopTileY * 16 - Position.Y;
		// Main.NewText(MathF.Abs(delatY));
	}

	private int GetNextTileOnPathY(int dir)
	{
		int startTileX = Position.X.ToTileCoordinate();
		int startTileY = dir == 1 ? Box.Bottom.ToTileCoordinate() : Box.Top.ToTileCoordinate();
		int scanRange = 10000;
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

		int scanRange = 10000;
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

			bool isFloor = IsTileFloor(leftTile) || IsTileFloor(rightTile);

			if (isFloor)
			{
				int destinationY = y + 1 + (dir == 1 ? Size.Y.ToTileCoordinate() : 0);
				if (destinationY != startTileY)
				{
					return destinationY;
				}
			}
		}
		return startTileY + scanRange * dir;
	}

	public bool IsTileFloor(Tile tile)
	{
		if (tile.HasTile && TileLoader.GetTile(tile.TileType) is FloorIndicatorTileBase fIT)
		{
			if (tile.TileFrameY == fIT.IdenticalFrameY)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Checks if the elevator can begin moving in a specific direction without immediately colliding.
	/// </summary>
	private bool CanMove(int dir)
	{
		int target = GetNextTileOnPathY(dir);
		float dist = (target * 16 - (dir == 1 ? Box.Bottom : Box.Top)) * CurrentMoveDirection;
		return dist > 4f;
	}

	#endregion

	#region Draw

	public override void Draw()
	{
		if (Position.X / 16f < Main.maxTilesX - 28 && Position.Y / 16f < Main.maxTilesY - 28 && Position.X / 16f > 28 && Position.Y / 16f > 28)
		{
			// SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			// bool changeState = false;
			// if(sBS.SamplerState != SamplerState.PointClamp)
			// {
			// 	 Main.spriteBatch.End();
			// 	 Main.spriteBatch.Begin(sBS.SortMode, sBS.BlendState, SamplerState.PointClamp, sBS.DepthStencilState, sBS.RasterizerState, sBS.Effect, sBS.TransformMatrix);
			// 	 changeState = true;
			// }
			Color lightColor = Lighting.GetColor(Box.Center.ToTileCoordinates());

			if (PreDrawElevatorCable(lightColor))
			{
				DrawElevatorCable(lightColor);
				PostDrawElevatorCable(lightColor);
			}

			if (PreDrawElevator(lightColor))
			{
				Main.spriteBatch.Draw(ElevatorTexture, new Vector2(Box.Center.X, Box.Bottom) - Main.screenPosition, null, lightColor, 0, new Vector2(ElevatorTexture.Width / 2f, ElevatorTexture.Height), 1, SpriteEffects.None, 0);
				DrawAuxiliaryStructure(lightColor);
				PostDrawElevator(lightColor);
			}

			// if (changeState)
			// {
			// 	 Main.spriteBatch.End();
			// 	 Main.spriteBatch.Begin(sBS);
			// }
		}
	}

	public virtual bool PreDrawElevator(Color lightColor) => true;

	public virtual void DrawAuxiliaryStructure(Color lightColor)
	{
		Vector2 drawPos = new Vector2(Box.Center.X, Box.Top) - Main.screenPosition;
		int unitFrameWidth = AuxiliaryStructureTexture.Width / 2;
		int unitFrameHeight = AuxiliaryStructureTexture.Height / 3;
		Rectangle auxiliaryFrame = new Rectangle(0, 0, unitFrameWidth, unitFrameHeight);
		if (LightSourceOn)
		{
			auxiliaryFrame.X += AuxiliaryStructureTexture.Width / 2;
		}
		AddLight();
		Vector2 auxiliaryOffset = new Vector2(auxiliaryFrame.Width * 0.5f, auxiliaryFrame.Height - 16);
		Main.spriteBatch.Draw(AuxiliaryStructureTexture, drawPos, auxiliaryFrame, lightColor, 0, auxiliaryOffset, 1, SpriteEffects.None, 0);
		if (LightSourceOn)
		{
			Rectangle auxiliaryFrame_glow = new Rectangle(auxiliaryFrame.X, unitFrameHeight, unitFrameWidth, unitFrameHeight);
			Rectangle auxiliaryFrame_bloom = new Rectangle(auxiliaryFrame.X, unitFrameHeight * 2, unitFrameWidth, unitFrameHeight);
			Main.spriteBatch.Draw(AuxiliaryStructureTexture, drawPos, auxiliaryFrame_glow, new Color(1f, 1f, 1f, 0), 0, auxiliaryOffset, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(AuxiliaryStructureTexture, drawPos, auxiliaryFrame_bloom, new Color(1f, 1f, 1f, 0) * 0.7f, 0, auxiliaryOffset, 1, SpriteEffects.None, 0);
		}
		if (Highlighted)
		{
			if (Main.SmartCursorIsUsed)
			{
				Vector2 highlight_Offset = new Vector2(AuxiliaryStructureTextureHighlight.Width * 0.5f, AuxiliaryStructureTextureHighlight.Height - 16);
				Main.spriteBatch.Draw(AuxiliaryStructureTextureHighlight, drawPos, null, Color.White, 0, highlight_Offset, 1, SpriteEffects.None, 0);
			}
		}
	}

	public virtual void AddLight()
	{
		if (LightSourceOn)
		{
			Lighting.AddLight(Box.Center + LightSourceRelativePos, LightSourceColor);
		}
	}

	public virtual void PostDrawElevator(Color lightColor)
	{
	}

	public virtual void DrawElevatorCable(Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		for (int f = 0; f < 10000; f++)
		{
			var jointPos = new Vector2(0, -ElevatorCableJointOffset - f * 12);
			Vector2 elevaorPos = Box.Center;
			Vector2 drawPos = elevaorPos + jointPos;
			if (drawPos.Y < Main.screenPosition.Y - 256)
			{
				break;
			}
			if (drawPos.Y > Main.screenPosition.Y + Main.screenHeight + 256)
			{
				continue;
			}
			Color drawRopeColor = Lighting.GetColor(drawPos.ToTileCoordinates());
			drawPos -= Main.screenPosition;
			bars.Add(drawPos + new Vector2(-4, 0), drawRopeColor, new Vector3(0, f, 0));
			bars.Add(drawPos + new Vector2(4, 0), drawRopeColor, new Vector3(1, f, 0));
			if ((jointPos + Box.Center).Y < WinchCoord.Y * 16 + 8)
			{
				break;
			}
		}
		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = ElevatorCableTexture;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public virtual bool PreDrawElevatorCable(Color lightColor) => true;

	public virtual bool ShouldHighlight()
	{
		Vector2 amended_mouseWorld = Main.MouseWorld;
		if (!CanClick())
		{
			return false;
		}
		float boxMinX = Box.Center.X + HighlightBoxRelativeTopLeft.X;
		float boxMaxX = Box.Center.X + HighlightBoxRelativeBottomRight.X;
		float boxMinY = Box.Center.Y + HighlightBoxRelativeTopLeft.Y;
		float boxMaxY = Box.Center.Y + HighlightBoxRelativeBottomRight.Y;
		if (amended_mouseWorld.X >= boxMinX && amended_mouseWorld.X <= boxMaxX)
		{
			if (amended_mouseWorld.Y >= boxMinY && amended_mouseWorld.Y <= boxMaxY)
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool CanClick()
	{
		return (Main.LocalPlayer.MountedCenter - (Box.Center + new Vector2(0, -50))).Length() <= 200;
	}

	public virtual void PostDrawElevatorCable(Color lightColor)
	{
	}

	#endregion
}