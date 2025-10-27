using Everglow.Commons.CustomTiles;
using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Yggdrasil.WorldGeneration;
using Terraria.Audio;

namespace Everglow.Yggdrasil.Common.Elevator;

public class YggdrasilElevator : BoxEntity
{
	/// <summary>
	/// 1 for down, -1 for up.
	/// </summary>
	public int CurrentMoveDirection = 1;

	/// <summary>
	/// Next move direction, assign to CurrentMoveDirection when start acclerating.
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
	/// Timer for decelerating.
	/// </summary>
	public int DecelerateTimer = 0;

	/// <summary>
	/// Detention time due to malfunction
	/// </summary>
	public int DetentionTime = 0;

	/// <summary>
	/// MoveState: 0: Stop, 1: Normal Move, 2: Accelerating, 3: Decelerating, -1: Malfunction.
	/// </summary>
	public int MoveState = 0;

	public float CurrentSpeed = 0f;

	public float Acceleration = 0.1f;

	/// <summary>
	/// Anchor at a winch tile.
	/// </summary>
	public Point WinchCoord;

	public bool LampOn = false;

	public bool Initialized = false;

	public override Color MapColor => new Color(122, 91, 79);

	public void OnSpawn()
	{
		Size = new Vector2(96, 16);
	}

	public override void AI()
	{
		if (!Initialized)
		{
			OnSpawn();
		}
		var winch = YggdrasilWorldGeneration.SafeGetTile(WinchCoord);
		if (winch.TileType != ModContent.TileType<Winch>())
		{
			Kill();
			return;
		}

		CheckState();
		switch (MoveState)
		{
			case -1:
				if(DetentionTime > 0)
				{
					DetentionTime--;
				}
				if (DetentionTime <= 0)
				{
					DetentionTime = 0;
				}
				break;
			case 0:
				if (StopTimer > 0)
				{
					StopTimer--;
				}
				if (StopTimer <= 0)
				{
					StopTimer = 0;
					AccelerateTimer = 30;
					CurrentMoveDirection = NextMoveDirection;
				}
				Velocity *= 0;
				break;
			case 1:
				CheckRunningDirection();
				if(CurrentSpeed < 5)
				{
					AccelerateTimer = (int)((5 - CurrentSpeed) * 10f);
				}
				Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);
				break;
			case 2:
				if (AccelerateTimer > 0)
				{
					AccelerateTimer--;
				}
				if (AccelerateTimer <= 0)
				{
					AccelerateTimer = 0;
				}
				CurrentSpeed += Acceleration;
				if(CurrentSpeed > 5)
				{
					CurrentSpeed = 5;
					AccelerateTimer = 0;
				}
				Velocity = new Vector2(0, CurrentSpeed * CurrentMoveDirection);
				break;
			case 3:
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
				if(CurrentSpeed <= 0)
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
			MoveState = -1;
			return;
		}
		if (StopTimer > 0)
		{
			MoveState = 0;
			return;
		}
		if (AccelerateTimer > 0)
		{
			MoveState = 2;
			return;
		}
		if (DecelerateTimer > 0)
		{
			MoveState = 3;
			return;
		}
		MoveState = 1;
	}

	/// <summary>
	/// Only update when MoveState == 1
	/// </summary>
	public void CheckRunningDirection()
	{
		if ((Box.Center.Y > WinchCoord.Y * 16 + 8 + 2560 || Terraria.Collision.SolidCollision(Position + new Vector2(0, Size.Y + Velocity.Y + 100), (int)Size.X, 1)) && CurrentMoveDirection == 1)
		{
			NextMoveDirection = -1;
			DecelerateTimer = 30;
		}

		if (Box.Center.Y < WinchCoord.Y * 16 + 8 + 300 && CurrentMoveDirection == -1)
		{
			NextMoveDirection = 1;
			DecelerateTimer = 30;
		}
	}

	public override void Draw()
	{
		if (Position.X / 16f < Main.maxTilesX - 28 && Position.Y / 16f < Main.maxTilesY - 28 && Position.X / 16f > 28 && Position.Y / 16f > 28)
		{
			Color drawc = Lighting.GetColor(Box.Center.ToTileCoordinates());
			Main.spriteBatch.Draw(ModAsset.SkyTreeLift.Value, Position - Main.screenPosition, new Rectangle(0, 0, (int)Size.X, (int)Size.Y), drawc);
			Texture2D LiftFramework = ModAsset.SkyTreeLiftShellLightOff.Value;

			if (LampOn)
			{
				Lighting.AddLight((int)(Position.X / 16f) + 1, (int)(Position.Y / 16f) - 3, 1f, 0.8f, 0f);
			}

			var drawcLampGlow = new Color(255, 255, 255, 0);

			Texture2D LiftLampOff = ModAsset.SkyTreeLiftShellLightOff.Value;
			Texture2D LiftLampOn = ModAsset.SkyTreeLiftShellLampOn.Value;
			Texture2D LiftLampGlow = ModAsset.SkyTreeLiftShellLampOnGlow.Value;
			Texture2D LiftRopeTop = ModAsset.SkyTreeLiftRope.Value;
			Texture2D LiftRope = ModAsset.Rope.Value;

			Main.spriteBatch.Draw(LiftFramework, Box.Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(LiftRopeTop, Box.Center - Main.screenPosition + new Vector2(0, -110), null, drawc, 0, new Vector2(48, 15), 1, SpriteEffects.None, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var bars = new List<Vertex2D>();
			for (int f = 0; f < 1000; f++)
			{
				Vector2 jointPos = new Vector2(0, -125 - f * 12);
				Vector2 drawPos = Box.Center - Main.screenPosition;
				Color drawcRope = Lighting.GetColor((int)(Position.X / 16f) + 2, (int)((Position.Y - f * 12) / 16f) - 7);

				bars.Add(drawPos + jointPos + new Vector2(-4, 0), drawcRope, new Vector3(0, f, 0));
				bars.Add(drawPos + jointPos + new Vector2(4, 0), drawcRope, new Vector3(1, f, 0));
				if((jointPos + Box.Center).Y < WinchCoord.Y * 16 + 8)
				{
					break;
				}
			}
			if (bars.Count > 2)
			{
				Main.graphics.GraphicsDevice.Textures[0] = LiftRope;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			if (!LampOn)
			{
				Main.spriteBatch.Draw(LiftLampOff, Box.Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
			}
			else
			{
				Main.spriteBatch.Draw(LiftLampOn, Box.Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
				Main.spriteBatch.Draw(LiftLampGlow, Box.Center - Main.screenPosition + new Vector2(0, -46), null, drawcLampGlow, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
			}
			Vector2 ButtomPosition = new Vector2(-11, -33) + Box.Center;
			if ((Main.MouseWorld - ButtomPosition).Length() < 10)
			{
				if (Main.SmartCursorIsUsed)
				{
					Texture2D LiftButtomHighLight = ModAsset.SkyTreeLiftShellMiddleButtomHightLight.Value;
					if (LampOn)
					{
						LiftButtomHighLight = ModAsset.SkyTreeLiftShellMiddleButtomOnHightLight.Value;
					}

					Main.spriteBatch.Draw(LiftButtomHighLight, Box.Center - Main.screenPosition + new Vector2(0, -46), null, Color.White, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
				}
				if (Main.mouseRight && Main.mouseRightRelease)
				{
					SoundEngine.PlaySound(SoundID.Unlock, ButtomPosition);
					LampOn = !LampOn;
				}
			}
		}
	}
}