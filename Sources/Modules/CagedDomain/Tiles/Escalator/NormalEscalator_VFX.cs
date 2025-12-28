using Everglow.Commons.Enums;
using Everglow.Commons.TileHelper;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Everglow.Commons.VFX.Scene;

namespace Everglow.CagedDomain.Tiles.Escalator;

/// <summary>
/// An interactive escalator.
/// It connect the NormalEscalator and NormalEscalator_Top tiles by an oblique escalator.
/// Player should control 'Up' to attach to the escalator, and 'Down' to exit the escalator.
/// </summary>
[Pipeline(typeof(WCSPipeline))]
public class NormalEscalator_VFX : TileVFX
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawTiles;

	public Rectangle HanddrailStrap_Bottom = new Rectangle(0, 0, 88, 54);
	public Rectangle HanddrailStrap_Medium = new Rectangle(92, 0, 44, 44);
	public Rectangle HanddrailStrap_Top = new Rectangle(140, 0, 70, 44);

	public Rectangle ExteriorPanel_Bottom = new Rectangle(0, 60, 58, 16);
	public Rectangle ExteriorPanel_Medium_Bottom = new Rectangle(136, 44, 30, 32);
	public Rectangle ExteriorPanel_Medium = new Rectangle(80, 48, 26, 28);
	public Rectangle ExteriorPanel_Medium_Top = new Rectangle(108, 50, 24, 24);
	public Rectangle ExteriorPanel_Top = new Rectangle(166, 46, 44, 16);

	public Rectangle StepBlock = new Rectangle(60, 60, 18, 16);

	public List<Vector2> StepPositions = new List<Vector2>();

	public List<Vector2> OldStepPositions = new List<Vector2>();

	public List<Vector2> StepVelocities = new List<Vector2>();

	public List<Player> AttachingPlayers = new List<Player>();

	public float MoveingSpeed = 16f / 30f;

	public override void Update()
	{
		if (TotalLength == 0)
		{
			for (int step = 0; step < 100; step++)
			{
				var tile = WorldGenMisc.SafeGetTile(OriginTilePos + new Point(Direction * step, -step));
				if (tile.TileType == ModContent.TileType<NormalEscalator_Top>())
				{
					TotalLength = step;
					break;
				}
			}
		}
		UpdateSteps();
		UpdatePlayerAttachment();
		base.Update();
	}

	public void UpdatePlayerAttachment()
	{
		foreach (var player in Main.player)
		{
			if (player is not null && player.active)
			{
				// TODO: Optimization, check player collide with the exterior rectangle.
				foreach (var blockPos in StepPositions)
				{
					Rectangle stepBox = new Rectangle((int)blockPos.X, (int)blockPos.Y, 16, 16);

					// Control 'Up';Not exist in the list;No mount;Collision;
					if (player.controlUp && !AttachingPlayers.Contains(player) && player.mount.Type == -1 && stepBox.Intersects(player.Hitbox))
					{
						AttachingPlayers.Add(player);
						break;
					}
				}
			}
		}

		foreach (var player in AttachingPlayers)
		{
			if (player is null || !player.active)
			{
				AttachingPlayers.Remove(player);
				break;
			}

			// Control 'Down' to exit the escalator..
			if (player.controlDown)
			{
				AttachingPlayers.Remove(player);
				break;
			}

			bool safe = false;
			for (int i = 0; i < StepPositions.Count; i++)
			{
				Vector2 blockPos = StepPositions[i];
				if (player.Bottom.X >= blockPos.X && player.Bottom.X < blockPos.X + 16)
				{
					// The speed rate constant: depends on the escalator speed (Updated by Main.time / 30f * 16)
					// TODO: Player frame
					float relativeMoveX = MoveingSpeed;
					if (player.Bottom.Y > blockPos.Y)
					{
						float deltaY = blockPos.Y - player.Bottom.Y;
						if (MathF.Abs(deltaY) > 15)
						{
							player.gfxOffY -= deltaY;
						}
						if (MathF.Abs(player.gfxOffY) > 16)
						{
							player.gfxOffY = 16;
						}
						player.Bottom = new Vector2(player.Bottom.X + relativeMoveX, blockPos.Y);
						if (player.bodyFrame.Y == 280)
						{
							player.bodyFrame.Y = 0;
							player.legFrame.Y = 0;
						}
					}
					safe = true;
					break;
				}
			}
			if (!safe)
			{
				AttachingPlayers.Remove(player);
				break;
			}
		}
	}

	public int TotalLength = 0;

	public override void Draw()
	{
		if (TotalLength == 0)
		{
			return;
		}
		if (Texture == null)
		{
			Texture = ModAsset.NormalEscalator_Atlas.Value;
		}

		SpriteEffects spDir = SpriteEffects.None;
		if (Direction == -1)
		{
			spDir = SpriteEffects.FlipHorizontally;
		}

		// Handdrail Straps
		Vector2 totalOffset = new Vector2(0);
		Vector2 drawPos = Position + totalOffset;

		DrawAPiece(drawPos + new Vector2(-62, -50), HanddrailStrap_Bottom);
		Vector2 piece_drawPos = drawPos + new Vector2(-18, -50);
		for (int i = 1; i < TotalLength; i++)
		{
			piece_drawPos += new Vector2(Direction * 16, -16);
			Ins.Batch.Draw(Texture, piece_drawPos, HanddrailStrap_Medium, Lighting.GetColor(piece_drawPos.ToTileCoordinates()), 0, Vector2.zeroVector, 1f, spDir);
		}
		DrawAPiece(drawPos + new Vector2(Direction, -1) * 16 * TotalLength + new Vector2(-18, -42), HanddrailStrap_Top);

		// Steps
		float stepValue = (float)(Main.time / 30f * 16);
		stepValue %= 16;

		if (StepPositions != null && StepPositions.Count > 0)
		{
			for (int i = 0; i < StepPositions.Count; i++)
			{
				Rectangle stepFrame = StepBlock;
				if (i == 0)
				{
					stepFrame = new Rectangle((int)(78 - stepValue), 60, (int)(stepValue + 2), 16);
				}
				DrawAPiece(StepPositions[i], stepFrame);
			}
		}

		// Exterior Panels
		DrawAPiece(drawPos + new Vector2(-48, 0), ExteriorPanel_Bottom);
		piece_drawPos = drawPos + new Vector2(-12, -4);
		for (int i = 0; i < TotalLength; i++)
		{
			Vector2 offset = Vector2.zeroVector;
			piece_drawPos += new Vector2(Direction * 16, -16);
			Rectangle frame = ExteriorPanel_Medium;
			if (i == 0)
			{
				frame = ExteriorPanel_Medium_Bottom;
				offset = new Vector2(-4, 0);
			}
			if (i == TotalLength - 1)
			{
				frame = ExteriorPanel_Medium_Top;
				offset = new Vector2(-2, 4);
			}
			DrawAPiece(piece_drawPos + offset, frame);
		}
		DrawAPiece(drawPos + new Vector2(Direction, -1) * 16 * TotalLength + new Vector2(-2, -2), ExteriorPanel_Top);
	}

	public void DrawAPiece(Vector2 position, Rectangle frame, Vector2 origin = default)
	{
		SpriteEffects spDir = SpriteEffects.None;
		if (Direction == -1)
		{
			spDir = SpriteEffects.FlipHorizontally;
		}
		Ins.Batch.Draw(Texture, position, frame, Lighting.GetColor(position.ToTileCoordinates()), 0, origin, 1f, spDir);
	}

	private float oldStepValue = 0;

	public void UpdateSteps()
	{
		OldStepPositions = StepPositions;
		StepPositions = new List<Vector2>();
		StepVelocities = new List<Vector2>();
		float stepValue = (float)(Main.time * MoveingSpeed);
		stepValue %= 16;
		Vector2 piece_drawPos = Position + new Vector2(-76, 62) + new Vector2(Direction, -1) * stepValue;
		for (int i = -3; i < TotalLength + 1; i++)
		{
			Vector2 offset = Vector2.zeroVector;
			if (i == TotalLength)
			{
				offset = new Vector2(0, stepValue);
			}
			if (i <= -1)
			{
				offset = new Vector2(0, stepValue + 16 * i);
			}
			if (i == -3)
			{
				offset += new Vector2(16 - stepValue, 0);
			}
			piece_drawPos += new Vector2(Direction * 16, -16);
			StepPositions.Add(piece_drawPos + offset);
			int oldPosUsedInVelocityCalculateIndex = StepPositions.Count - 1;
			if (oldStepValue > stepValue)
			{
				oldPosUsedInVelocityCalculateIndex = StepPositions.Count - 2;
			}
			if (oldPosUsedInVelocityCalculateIndex > 0 && OldStepPositions.Count > oldPosUsedInVelocityCalculateIndex)
			{
				StepVelocities.Add(piece_drawPos + offset - OldStepPositions[oldPosUsedInVelocityCalculateIndex]);
			}
			else
			{
				StepVelocities.Add(Vector2.zeroVector);
			}
		}
		oldStepValue = stepValue;
	}
}