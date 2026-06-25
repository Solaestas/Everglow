using Everglow.Commons.CustomTiles.Abstracts;
using Everglow.Commons.CustomTiles.Core;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.CustomTiles;

public class YggdrasilTown_NormalMetro : BoxEntity
{
	public int StopTimer = 0;

	public int Direction = -1;

	public int StopTimeMax = 600;

	public bool Moving = false;

	public float TrackY = 0;

	public float StationX = 0;

	public float Speed = 0;

	public override void SetDefaults()
	{
		Size = new Vector2(1696, 16);
	}

	public override Color MapColor => new Color(128, 131, 142);

	public override void AI()
	{
		SetTrackY();
		Position = new Vector2(Position.X, TrackY);
		if (StopTimer == 0 && !Moving)
		{
			if (!SearchStation())
			{
				Direction *= -1;
				SearchStation();
			}
		}
		if (StationX != 0 && Math.Abs(Box.Center.X - StationX) > 16)
		{
			Moving = true;
		}
		if (Moving)
		{
			Accelerate();
		}
		Velocity = new Vector2(Direction * Speed, 0);
		Position += Velocity;
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			Direction *= -1;
		}
		if (Main.mouseMiddle && Main.mouseMiddleRelease)
		{
			Active = false;
		}
	}

	public void Accelerate()
	{
		if (MathF.Abs(Position.X + Size.X * 0.5f * Direction - StationX) > 800)
		{
			if (Speed < 30)
			{
				Speed += 0.2f;
			}
		}
		else
		{
			if (Speed > 0)
			{
				Speed -= 0.2f;
			}
		}
	}

	public void SetTrackY()
	{
		TrackY = Position.Y - Position.Y % 16;
	}

	public bool SearchStation()
	{
		int y = (int)(TrackY / 16f - 3);
		for (int dx = 10; dx < 2000; dx += 5)
		{
			int x = (int)(Box.Center.X / 16) + dx * Direction;
			var tile = TileUtils.SafeGetTile(x, y);
			if (tile.TileType == ModContent.TileType<MetroStationSign_YggdrasilTown>() || tile.TileType == ModContent.TileType<MetroStationSign_PylonSquare>())
			{
				StationX = x - tile.TileFrameX / 18 + 2;
				return true;
			}
		}
		return false;
	}

	public override void Draw()
	{
		Texture2D metro = ModAsset.YggdrasilTown_NormalMetro.Value;
		var frame = new Rectangle(108, 0, 848, 146);
		var glow_frame = new Rectangle(108, 292, 848, 146);
		var pos0 = new Vector2((Box.Center.X + Box.Left) * 0.5f, Box.Bottom);
		Main.spriteBatch.Draw(metro, pos0 - Main.screenPosition, frame, Lighting.GetColor(pos0.ToTileCoordinates()), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos0 - Main.screenPosition, glow_frame, new Color(1f, 1f, 1f, 0), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);

		var pos1 = new Vector2((Box.Center.X + Box.Right) * 0.5f, Box.Bottom);
		Main.spriteBatch.Draw(metro, pos1 - Main.screenPosition, frame, Lighting.GetColor(pos1.ToTileCoordinates()), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos1 - Main.screenPosition, glow_frame, new Color(1f, 1f, 1f, 0), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);

		var headFrame = new Rectangle(0, 0, 106, 146);
		var tailFrame = new Rectangle(958, 0, 106, 146);

		var pos2 = new Vector2(Box.Left, Box.Bottom - 73);
		var pos3 = new Vector2(Box.Right, Box.Bottom - 73);
		Main.spriteBatch.Draw(metro, pos2 - Main.screenPosition, headFrame, Lighting.GetColor(pos2.ToTileCoordinates()), 0, new Vector2(headFrame.Width, headFrame.Height * 0.5f), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos3 - Main.screenPosition, tailFrame, Lighting.GetColor(pos2.ToTileCoordinates()), 0, new Vector2(0, tailFrame.Height * 0.5f), 1, SpriteEffects.None, 0);

		headFrame.Y += 292;
		tailFrame.Y += 292;
		Main.spriteBatch.Draw(metro, pos2 - Main.screenPosition, headFrame, new Color(1f, 1f, 1f, 0) * 0.36f, 0, new Vector2(headFrame.Width, headFrame.Height * 0.5f), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos3 - Main.screenPosition, tailFrame, new Color(1f, 1f, 1f, 0) * 0.36f, 0, new Vector2(0, tailFrame.Height * 0.5f), 1, SpriteEffects.None, 0);
	}

	public void RightClick()
	{
	}

	public override Vector2 StandAccelerate(IBox obj)
	{
		return base.StandAccelerate(obj);
	}
}