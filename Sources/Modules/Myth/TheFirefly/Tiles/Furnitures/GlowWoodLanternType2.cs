using Everglow.Commons.TileHelper;
using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly;
using Everglow.Myth.TheFirefly.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodLanternType2 : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		AdjTiles = new int[] { TileID.HangingLanterns };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile & AnchorType.SolidBottom & AnchorType.Platform, TileObjectData.newTile.Width, 0);
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(251, 235, 127), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 2);
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 18)
		{
			r = 0.1f;
			g = 0.9f;
			b = 1f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 0)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var tile = Main.tile[pos];
		Texture2D tex = ModAsset.GlowWoodLanternType2Draw.Value;

		int offsetX = 8;
		int offsetY = -2;
		if (WorldGen.IsBelowANonHammeredPlatform(pos.X, pos.Y))
		{
			offsetY -= 8;
		}

		int sizeX = 1;
		int sizeY = 2;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(pos.X, pos.Y, sizeX, sizeY))
			windCycle = tileDrawing.GetWindCycle(pos.X, pos.Y, tileDrawing._sunflowerWindCounter);

		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(pos.X, pos.Y, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;

		short tileFrameX = tile.frameX;
		short tileFrameY = tile.frameY;

		Rectangle rectangle = new Rectangle(tileFrameX, tileFrameY, sizeX * 16, sizeY * 16);
		Color tileLight = Lighting.GetColor(pos);
		float rotation = -windCycle * 0.15f;
		var origin = new Vector2(sizeX * 16 / 2f, 0);
		var tileSpriteEffect = SpriteEffects.None;
		spriteBatch.Draw(tex, pos.ToWorldCoordinates(0, 0) + new Vector2(offsetX, offsetY) - screenPosition, rectangle, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
	}
}