using System;
using Everglow.Commons.TileHelper;
using Everglow.Myth.TheFirefly.Dusts;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodChandelier : ModTile, ITileFluentlyDrawn
{
	private Asset<Texture2D> flameTexture;

	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<BlueGlow>();
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.addTile(Type);

		if (!Main.dedServ)
			flameTexture = ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Tiles/Furnitures/GlowWoodChandelier_Flame");

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 3, 3);
	}
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
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
		if (tile.TileFrameY == 0 && tile.TileFrameX % 54 == 0)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}
	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var tile = Main.tile[pos];
		Texture2D tex = ModAsset.Tiles_GlowWoodChandelier.Value;
		Texture2D flame = flameTexture.Value;

		int offsetX = 8;
		int offsetY = -2;

		int sizeX = 3;
		int sizeY = 3;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(pos.X, pos.Y, sizeX, sizeY))
			windCycle = tileDrawing.GetWindCycle(pos.X, pos.Y, tileDrawing._sunflowerWindCounter);

		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(pos.X, pos.Y, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;

		short tileFrameX = tile.frameX;
		short tileFrameY = tile.frameY;

		Rectangle rectangle = new Rectangle(tileFrameX, tileFrameY, 54, 48);
		Color tileLight = Lighting.GetColor(pos);
		float rotation = -windCycle * 0.15f;
		var origin = new Vector2(sizeX * 18 / 2f, 0);
		var tileSpriteEffect = SpriteEffects.None;
		spriteBatch.Draw(tex, pos.ToWorldCoordinates(0, 0) + new Vector2(offsetX, offsetY) - screenPosition, rectangle, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
		ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)pos.X << 32 | (uint)pos.Y); // Don't remove any casts.
		var color = new Color(30, 30, 30, 0);
		if (tile.TileFrameX < 54)
		{
			for (int k = 0; k < 7; k++)
			{
				float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
				spriteBatch.Draw(flame, pos.ToWorldCoordinates(0, 0) + new Vector2(offsetX + xx, offsetY + yy * 0.6f) - screenPosition, rectangle, color, rotation, origin, 1f, tileSpriteEffect, 0f);
			}
		}
	}
}