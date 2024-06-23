using Everglow.Commons.TileHelper;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader.IO;

namespace Everglow.Myth.TheFirefly.Tiles;

public class FireflyMoss : ModTile, ITileFluentlyDrawn
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileBlockLight[Type] = false;
		Main.tileLighted[Type] = true;
		Main.tileMoss[Type] = true;
		Main.tileCut[Type] = true;
		AddMapEntry(new Color(51, 107, 204));
		DustType = ModContent.DustType<FluorescentTreeDust>();
		HitSound = SoundID.Grass;
	}

	public override bool CreateDust(int i, int j, ref int type)
	{
		Dust.NewDustDirect(new Vector2(i, j) * 16, 16, 16, DustType, 0, 0, 0, default, Main.rand.NextFloat(0.3f, 0.6f));
		return false;
	}

	public override void MouseOver(int i, int j)
	{
		Player player = Main.LocalPlayer;
		if (player.HeldItem.type == ItemID.PaintScraper || player.HeldItem.type == ItemID.SpectrePaintScraper)
		{
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = player.HeldItem.type;
			if (player.controlUseItem)
			{
				WorldGen.KillTile(i, j);
			}
		}
		base.MouseOver(i, j);
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		Player player = Main.LocalPlayer;
		if (player.HeldItem.type == ItemID.PaintScraper || player.HeldItem.type == ItemID.SpectrePaintScraper)
		{
			if (Main.rand.NextBool(10))
			{
				yield return new Item(ModContent.ItemType<Items.FireflyMoss_Item>());
			}
		}
		else
		{
			yield break;
		}
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		DrawMossPiece(pos, pos.ToWorldCoordinates() - screenPosition, spriteBatch, tileDrawing);
	}

	/// <summary>
	/// Draw a piece of moss
	/// </summary>
	private void DrawMossPiece(Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var tile = Main.tile[tilePos];
		ushort type = tile.TileType;
		var frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 18, 18);

		// 回声涂料
		if (!TileDrawing.IsVisible(tile))
		{
			return;
		}

		int paint = Main.tile[tilePos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.FireflyMoss_Path, type, 1, paint, tileDrawing);
		tex ??= ModAsset.FireflyMoss.Value;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(tilePos.X, tilePos.Y, 1, 1))
		{
			windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter);
		}

		int totalPushTime = 140;
		float pushForcePerFrame = 0.96f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(tilePos.X, tilePos.Y, 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;
		float rotation = windCycle * 0.21f;

		var tileLight = Lighting.GetColor(tilePos);

		// 支持发光涂料
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(tilePos.X, tilePos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(tilePos.X, tilePos.Y, tile, type, 0, 0, tileLight);

		var offset = new Vector2(0);
		var origin = new Vector2(0);
		switch (tile.TileFrameY / 54)
		{
			case 0:
				origin = new Vector2(9, 18);
				offset = new Vector2(0, 14);
				break;
			case 1:
				origin = new Vector2(9, 0);
				offset = new Vector2(0, -10);
				break;
			case 2:
				origin = new Vector2(0, 9);
				offset = new Vector2(-10, 0);
				break;
			case 3:
				origin = new Vector2(18, 9);
				offset = new Vector2(10, 0);
				break;
		}

		var tileSpriteEffect = SpriteEffects.None;
		spriteBatch.Draw(tex, drawCenterPos + offset, frame, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
		frame.X += 20;
		spriteBatch.Draw(tex, drawCenterPos + offset, frame, new Color(0.1f, 0.3f + 0.2f * MathF.Sin(tilePos.X * 2.4f + tilePos.Y), 0.6f + 0.3f * MathF.Sin(tilePos.X - tilePos.Y * 3), 0), rotation, origin, 1f, tileSpriteEffect, 0f);
	}
}