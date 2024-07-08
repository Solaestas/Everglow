using Everglow.Commons.TileHelper;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;
using static Everglow.Yggdrasil.YggdrasilTown.Kitchen.KitchenSystem.KitchenSystemUI;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

public class ServingCounter_ChineseStyle : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileNoAttach[Type] = false;
		TileID.Sets.DisableSmartCursor[Type] = true;

		DustType = DustID.DynastyWood;
		AdjTiles = new int[] { TileID.Benches };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = true;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1); // Facing right will use the second texture style
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(124, 13, 0));
	}

	public override bool RightClick(int i, int j)
	{
		FoodRequests.Sort((panel1, panel2) => panel1.TimeLeft.CompareTo(panel2.TimeLeft));

		// Iterate through the sorted list and execute CheckFinish() for each panel
		foreach (var panel in FoodRequests)
		{
			panel.CheckFinishInventory();
		}
		return base.RightClick(i, j);
	}

	public override void MouseOver(int i, int j)
	{
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return base.PreDraw(i, j, spriteBatch);
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		DrawSignBoard(pos, pos.ToWorldCoordinates() - screenPosition, spriteBatch, tileDrawing);
	}

	public void DrawSignBoard(Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		// 回声涂料
		if (!TileDrawing.IsVisible(Main.tile[tilePos]))
		{
			return;
		}

		var tile = Main.tile[tilePos];
		ushort type = tile.TileType;
		int paint = Main.tile[tilePos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.ServingCounter_ChineseStyle_Path, type, 1, paint, tileDrawing);
		tex ??= ModAsset.ServingCounter_ChineseStyle.Value;

		// 支持发光涂料
		Color tileLight;
		tileLight = Lighting.GetColor(tilePos);
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(tilePos.X, tilePos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(tilePos.Y, tilePos.X, tile, type, 0, 0, tileLight);

		var tileSpriteEffect = SpriteEffects.None;

		Rectangle frame = new Rectangle(92, 36, 12, 48);
		spriteBatch.Draw(tex, drawCenterPos + new Vector2(0, -6), frame, tileLight, 0, new Vector2(frame.Width * 0.5f, frame.Height), 1f, tileSpriteEffect, 0f);
		if (tile.TileFrameX <= 54)
		{
			frame = new Rectangle(0, 36, 58, 24);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(-44, -58), frame, tileLight, 0, new Vector2(0, frame.Height * 0.5f), 1f, tileSpriteEffect, 0f);

			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind(tilePos.X - 4, tilePos.Y - 4, 2, 3))
			{
				windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter);
			}
			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(tilePos.X - 4, tilePos.Y - 4, 2, 3, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			float rotation = -windCycle * 0.05f;
			frame = new Rectangle(72, 36, 14, 10);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(-37, -46), frame, tileLight, rotation, new Vector2(frame.Width * 0.5f, 0), 1f, tileSpriteEffect, 0f);
			frame = new Rectangle(74, 48, 10, 10);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(-37, -46) + new Vector2(0, 10).RotatedBy(rotation), frame, tileLight, rotation * 2, new Vector2(frame.Width * 0.5f, 0), 1f, tileSpriteEffect, 0f);
			frame = new Rectangle(76, 60, 6, 8);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(-37, -46) + new Vector2(0, 10).RotatedBy(rotation) + new Vector2(0, 10).RotatedBy(rotation * 2), frame, tileLight, rotation * 4, new Vector2(frame.Width * 0.5f, 0), 1f, tileSpriteEffect, 0f);
		}
		else
		{
			frame = new Rectangle(2, 60, 58, 24);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(-14, -58), frame, tileLight, 0, new Vector2(0, frame.Height * 0.5f), 1f, tileSpriteEffect, 0f);
			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind(tilePos.X - 4, tilePos.Y - 4, 2, 3))
			{
				windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter);
			}
			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(tilePos.X - 4, tilePos.Y - 4, 2, 3, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			float rotation = -windCycle * 0.05f;
			frame = new Rectangle(72, 36, 14, 10);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(37, -46), frame, tileLight, rotation, new Vector2(frame.Width * 0.5f, 0), 1f, tileSpriteEffect, 0f);
			frame = new Rectangle(74, 48, 10, 10);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(37, -46) + new Vector2(0, 10).RotatedBy(rotation), frame, tileLight, rotation * 2, new Vector2(frame.Width * 0.5f, 0), 1f, tileSpriteEffect, 0f);
			frame = new Rectangle(76, 60, 6, 8);
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(37, -46) + new Vector2(0, 10).RotatedBy(rotation) + new Vector2(0, 10).RotatedBy(rotation * 2), frame, tileLight, rotation * 4, new Vector2(frame.Width * 0.5f, 0), 1f, tileSpriteEffect, 0f);
		}
	}
}