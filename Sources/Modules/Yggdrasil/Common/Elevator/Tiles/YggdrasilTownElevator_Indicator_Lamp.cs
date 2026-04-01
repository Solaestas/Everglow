using Everglow.Commons.CustomTiles;
using Everglow.Commons.Templates.Furniture.Elevator;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class YggdrasilTownElevator_Indicator_Lamp : ModTile, IFloorIndicatorTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18,
		};
		TileObjectData.newTile.CoordinateWidth = 32;
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
		TileObjectData.newTile.Origin = new Point16(0, 2);

		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.Origin = new Point16(0, 2);
		TileObjectData.addTile(Type);
		DustType = DustID.Iron;
		AddMapEntry(new Color(191, 142, 111));
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		var tile = Main.tile[i, j];
		int frameY = 0;
		foreach (var entity in ColliderManager.Instance.OfType<YggdrasilElevator>())
		{
			Vector2 center = entity.Box.Center;
			if (Math.Abs(center.Y / 16f - j) < 4 && tile.TileFrameY % 54 == 0 && Math.Abs(center.X / 16f - i) < entity.Size.X / 32f + 5)
			{
				frameY = 54;
				break;
			}
		}
		if (frameY == 54)
		{
			if (tile.TileFrameY == 0)
			{
				// FurnitureUtils.LightHitwireStyleVertical(i, j, Type, 1, 3, 32);
				for (int y = 0; y < 3; y++)
				{
					var tile2 = TileUtils.SafeGetTile(i, j + y);
					tile2.TileFrameY = (short)(54 + y * 18);
				}
			}
		}
		else
		{
			if (tile.TileFrameY == 54)
			{
				for (int y = 0; y < 3; y++)
				{
					var tile2 = TileUtils.SafeGetTile(i, j + y);
					tile2.TileFrameY = (short)(y * 18);
				}
			}
		}
		if (tile.TileFrameY is >= 54 and <= 72)
		{
			Lighting.AddLight(new Vector2(i * 16, j * 16), new Vector3(1f, 0.8f, 0.3f));
		}
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D glow = ModAsset.YggdrasilTownElevator_Indicator_Lamp_glow.Value;
		var zero = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		var tile = Main.tile[i, j];
		Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 32, 32);
		spriteBatch.Draw(glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-8, 0), frame, new Color(1f, 1f, 1f, 0), 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
	}
}