using Everglow.Commons.CustomTiles.Core;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class Elevator : BoxEntity
{
	public enum State
	{
		Malfunction = -1,
		Stop = 0,
		NormalMove = 1,
		Accelerating = 2,
		Decelerating = 3,
	}

	/// <summary>
	/// 1 for down, -1 for up.
	/// </summary>
	public int CurrentMoveDirection = 1;

	/// <summary>
	/// Next move direction, assign to <see cref="CurrentMoveDirection"/> when start acclerating.
	/// </summary>
	public int NextMoveDirection = 1;

	/// <summary>
	/// Timer for a stop elevator.
	/// </summary>
	public int StopTimer = 0;

	/// <summary>
	/// Timer for accelerating.
	/// </summary>
	public int AccelerateTimer = 0;

	/// <summary>
	/// Total accelerate time when start moving.<br/>
	/// Default to 30.
	/// </summary>
	public int NormalAccelerateTime = 30;

	/// <summary>
	/// Timer for decelerating.
	/// </summary>
	public int DecelerateTimer = 0;

	/// <summary>
	/// Total decelerate time when start stopping.<br/>
	/// Default to 30.
	/// </summary>
	public int NormalDecelerateTime = 30;

	/// <summary>
	/// Detention time due to malfunction
	/// </summary>
	public int DetentionTime = 0;

	/// <summary>
	/// Move state.
	/// <br/>Default to <see cref="State.Stop"/>.
	/// </summary>
	public State MoveState = State.Stop;

	/// <summary>
	/// When the elevator move exceed this value to the winch, elevator will return forcefully.<br/>
	/// Default to ∞.
	/// </summary>
	public float LengthRestrict = float.PositiveInfinity;

	public float CurrentSpeed = 0f;

	/// <summary>
	/// During <see cref="State.Accelerating"/> / <see cref="State.Decelerating"/> The velocity will +/- this value per tick.
	/// </summary>
	public float Acceleration = 0.1f;

	/// <summary>
	/// ↑v<br/>
	/// │*<br/>
	/// │****<br/>
	/// │*******<br/>
	/// │**********<br/>
	/// │*************<br/>
	/// │****************<br/>
	/// │*******************  t<br/>
	/// └─────────────→<br/>
	/// Calculate the move distance when doing uniformly varying motion.
	/// </summary>
	/// <returns></returns>
	public float UniformlyVaryingMotionDistance(float speed, float a, float t)
	{
		return speed * t + 0.5f * a * t * t;
	}

	/// <summary>
	/// Anchor at a winch tile.
	/// </summary>
	public Point WinchCoord;

	public override Color MapColor => new Color(122, 91, 79);

	public override void AI()
	{
		var winch = SafeGetTile(WinchCoord);
		if (TileLoader.GetTile(winch.TileType) is WinchTile)
		{
			Kill();
			return;
		}

		CheckState();
		switch (MoveState)
		{
			case State.Malfunction:
				if (DetentionTime > 0)
				{
					DetentionTime--;
				}
				if (DetentionTime <= 0)
				{
					DetentionTime = 0;
				}
				break;
			case State.Stop:
				if (StopTimer > 0)
				{
					StopTimer--;
				}
				if (StopTimer <= 0)
				{
					StopTimer = 0;
					AccelerateTimer = NormalAccelerateTime;
					CurrentMoveDirection = NextMoveDirection;
				}
				Velocity *= 0;
				break;
			case State.NormalMove:
				CheckRunningDirection();
				if (CurrentSpeed < 5)
				{
					AccelerateTimer = (int)((5 - CurrentSpeed) * 10f);
				}
				Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);
				break;
			case State.Accelerating:
				if (AccelerateTimer > 0)
				{
					AccelerateTimer--;
				}
				if (AccelerateTimer <= 0)
				{
					AccelerateTimer = 0;
				}
				CurrentSpeed += Acceleration;
				if (CurrentSpeed > 5)
				{
					CurrentSpeed = 5;
					AccelerateTimer = 0;
				}
				Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);
				break;
			case State.Decelerating:
				if (DecelerateTimer > 0)
				{
					DecelerateTimer--;
				}
				if (DecelerateTimer <= 0)
				{
					DecelerateTimer = 0;
					StopElevator(120);
				}
				CurrentSpeed -= Acceleration;
				if (CurrentSpeed <= 0)
				{
					DecelerateTimer = 0;
					CurrentSpeed = 0;
					StopElevator(120);
				}
				Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);
				break;
		}
	}

	public void StopElevator(int time)
	{
		StopTimer = time;
	}

	public void CheckState()
	{
		if (DetentionTime > 0)
		{
			MoveState = State.Malfunction;
		}
		else if (StopTimer > 0)
		{
			MoveState = State.Stop;
		}
		else if (AccelerateTimer > 0)
		{
			MoveState = State.Accelerating;
		}
		else if (DecelerateTimer > 0)
		{
			MoveState = State.Decelerating;
		}
		else
		{
			MoveState = State.NormalMove;
		}
	}

	/// <summary>
	/// Only update when <see cref="MoveState"/> is <see cref="State.NormalMove"/>.
	/// Calculate
	/// </summary>
	public void CheckRunningDirection()
	{
		// Current move direction = 1, downward.
		float checkDistanceDown = UniformlyVaryingMotionDistance(Velocity.Y, -Acceleration, NormalDecelerateTime);
		if ((Box.Center.Y > WinchCoord.Y * 16 + 8 + LengthRestrict/* Exceed length restriction */ || Terraria.Collision.SolidCollision(Position + new Vector2(0, Size.Y + checkDistanceDown), (int)Size.X, 1)/* Collision with tile */) && CurrentMoveDirection == 1)
		{
			NextMoveDirection = -1;
			DecelerateTimer = NormalDecelerateTime;
		}

		// Current move direction = -1, upward.
		float checkDistanceUp = UniformlyVaryingMotionDistance(Velocity.Y, Acceleration, NormalDecelerateTime);
		if ((Box.Center.Y < WinchCoord.Y * 16 + 8 + 400 + checkDistanceUp/* Hit the winch */ || Terraria.Collision.SolidCollision(Position + new Vector2(0, checkDistanceDown), (int)Size.X, 1)/* Collision with tile */) && CurrentMoveDirection == -1)
		{
			NextMoveDirection = 1;
			DecelerateTimer = NormalDecelerateTime;
		}
	}

	public Texture2D ElevatorCableTexture;

	public Texture2D ElevatorTexture;

	public override void Draw()
	{
		if (Position.X / 16f < Main.maxTilesX - 28 && Position.Y / 16f < Main.maxTilesY - 28 && Position.X / 16f > 28 && Position.Y / 16f > 28)
		{
			Color drawc = Lighting.GetColor(Box.Center.ToTileCoordinates());
			Main.spriteBatch.Draw(ElevatorTexture, Box.Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, ElevatorTexture.Size() * 0.5f, 1, SpriteEffects.None, 0);
			DrawElevatorCable();
		}
	}

	/// <summary>
	/// TODO: Update the code.It's legacy.
	/// </summary>
	public virtual void DrawElevatorCable()
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var bars = new List<Vertex2D>();
		for (int f = 0; f < 1000; f++)
		{
			var jointPos = new Vector2(0, -125 - f * 12);
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
			Main.graphics.GraphicsDevice.Textures[0] = ElevatorCableTexture;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public static Tile SafeGetTile(int i, int j)
	{
		return Main.tile[Math.Clamp(i, 20, Main.maxTilesX - 20), Math.Clamp(j, 20, Main.maxTilesY - 20)];
	}

	public static Tile SafeGetTile(Point point)
	{
		return Main.tile[Math.Clamp(point.X, 20, Main.maxTilesX - 20), Math.Clamp(point.Y, 20, Main.maxTilesY - 20)];
	}
}