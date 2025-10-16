using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class AgedGardenMagicArray : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		DustType = ModContent.DustType<WaterErodedGreenBrickDust>();
		MinPick = int.MaxValue;

		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileSolid[Type] = false;
		Main.tileSolidTop[Type] = true;
		Main.tileWaterDeath[Type] = false;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Width = 14;
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.CoordinatePadding = 2;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new(7, 1);
		TileObjectData.newTile.AnchorTop = new AnchorData(0, 0, 0);
		TileObjectData.newTile.AnchorBottom = new AnchorData(0, 0, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(84, 111, 124));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}

	public override bool CanKillTile(int i, int j, ref bool blockDamaged)
	{
		return false;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameY == 0)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return base.PreDraw(i, j, spriteBatch);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		base.PostDraw(i, j, spriteBatch);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		base.NearbyEffects(i, j, closer);
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		DrawMagicStones(pos, pos.ToWorldCoordinates() - screenPosition, spriteBatch, tileDrawing);
	}

	/// <summary>
	/// Draw a piece of lotus
	/// </summary>
	private void DrawMagicStones(Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var tile = Main.tile[tilePos];
		if (!(tile.TileType == Type && tile.HasTile))
		{
			return;
		}
		ushort type = tile.TileType;

		// 回声涂料
		if (!TileDrawing.IsVisible(tile))
		{
			return;
		}

		int paint = Main.tile[tilePos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.AgedGardenMagicArray_Path, type, 1, paint, tileDrawing);
		tex ??= ModAsset.AgedGardenMagicArray.Value;
		for (int j = 0; j < 7; j++)
		{
			var frame = new Rectangle(tile.TileFrameX / 18 * 16, (6 - j) * 16 + 36, 16, 16);
			var tileLight = Lighting.GetColor(tilePos + new Point(0, -j));

			// 支持发光涂料
			tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(tilePos.X, tilePos.Y - j, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
			tileLight = tileDrawing.DrawTiles_GetLightOverride(tilePos.X, tilePos.Y - j, tile, type, 0, 0, tileLight);

			var origin = new Vector2(8, 8);

			var tileSpriteEffect = SpriteEffects.None;
			spriteBatch.Draw(tex, drawCenterPos + new Vector2(0, -j * 16 - 16), frame, tileLight, 0, origin, 1f, tileSpriteEffect, 0f);
		}
	}
}