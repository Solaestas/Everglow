using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Steamworks;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.IsleOfBloom;

public class IslePeachTree_small : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileWaterDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 2;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			18,
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = true;
		TileObjectData.addTile(Type);
		DustType = ModContent.DustType<IslePeachTree_Sawdust>();
		AddMapEntry(new Color(137, 99, 99));
		HitSound = SoundID.Dig;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = TileUtils.SafeGetTile(i, j);
		if(tile.TileFrameY == 18)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		DrawPeachTree(pos, pos.ToWorldCoordinates() - screenPosition, spriteBatch, tileDrawing);
	}

	/// <summary>
	/// Draw a piece of peach blossom
	/// </summary>
	private void DrawPeachTree(Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var tile = TileUtils.SafeGetTile(tilePos);
		Texture2D tex = ModAsset.IslePeachTree_small_tree.Value;

		// 回声涂料
		if (!TileDrawing.IsVisible(tile))
		{
			return;
		}

		int paint = Main.tile[tilePos].TileColor;
		tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.IslePeachTree_small_tree_Path, Type, 1, paint, tileDrawing);
		tex ??= ModAsset.IslePeachTree_small_tree.Value;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(tilePos.X, tilePos.Y, 1, 1))
		{
			windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter) * 0.25f;
		}

		int totalPushTime = 140;
		float pushForcePerFrame = 0.96f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(tilePos.X, tilePos.Y, 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex * 0.25f;
		float rotation = windCycle * 0.01f;

		var tileLight = Lighting.GetColor(tilePos);

		// 支持发光涂料
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(tilePos.X, tilePos.Y, tile, Type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(tilePos.X, tilePos.Y, tile, Type, 0, 0, tileLight);

		var frame = new Rectangle(0, 26, 182, 142);
		var origin = new Vector2(88, 140);
		var offset = new Vector2(0, 8);
		if (TileUtils.GetFixedRandomNumber(tile) % 2 == 1)
		{
			frame = new Rectangle(182, 10, 156, 158);
			origin = new Vector2(92, 156);
		}

		var drawPos = drawCenterPos;
		var tileSpriteEffect = SpriteEffects.None;
		if(TileUtils.GetFixedRandomNumber(tile, 300) % 2 == 1)
		{
			//tileSpriteEffect = SpriteEffects.FlipHorizontally;
		}
		spriteBatch.Draw(tex, drawPos + offset, frame, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
	}
}