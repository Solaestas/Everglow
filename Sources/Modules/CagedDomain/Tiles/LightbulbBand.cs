using System.Collections.Generic;
using Everglow.Commons.Physics;
using Everglow.Commons.TileHelper;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.Drawing;
using static Everglow.Commons.Physics.Rope;

namespace Everglow.CagedDomain.Tiles;

public class LightbulbBand : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;

		AddMapEntry(new Color(151, 31, 32));
	}
	/// <summary>
	/// 绳头位置 绳子
	/// </summary>
	public Dictionary<Point,Rope> RopesOfAllThisTileInTheWorld = new Dictionary<Point, Rope>();
	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY < 54 && tile.TileFrameY >= 0)
		{
			r = 0.45f;
			g = 0.15f;
			b = 0.0f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
	}
	public override void HitWire(int i, int j)
	{

	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		TileFluentDrawManager.AddFluentPoint(this, i, j);
		return base.PreDraw(i, j, spriteBatch);
	}
	public override bool RightClick(int i, int j)
	{
		AddRope(i, j);
		return base.RightClick(i, j);
	}
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		RopesOfAllThisTileInTheWorld.Remove(new Point(i, j));
		foreach (Rope rope in RopesOfAllThisTileInTheWorld.Values)
		{
			if((rope.GetMassList.Last().Position - new Vector2(i + 0.5f, j + 0.5f) * 16f).Length() < 4)
			{
				foreach(Point point in RopesOfAllThisTileInTheWorld.Keys)
				{
					Rope tryRope;
					RopesOfAllThisTileInTheWorld.TryGetValue(point, out tryRope);
					if (tryRope == rope)
					{
						RopesOfAllThisTileInTheWorld.Remove(point);
						break;
					}
				}
			}
		}
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		base.NearbyEffects(i, j, closer);
	}
	public void AddRope(int i, int j)
	{
		if (RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			return;
		}
		else
		{
			for(int x = -30;x < 31;x++)
			{
				for (int y = -30; y < 31; y++)
				{
					Tile tile = Main.tile[i + x, j + y];
					if (tile.TileType == Type && !(x == 0 && y == 0))
					{
						int counts = (int)new Vector2(x, y).Length();

						Rope rope = new Rope(new Vector2(i, j) * 16 + new Vector2(8), new Vector2(i + x, j + y) * 16 + new Vector2(8), counts, 1, 1, (Vector2) => Vector2.Zero, true);
						RopesOfAllThisTileInTheWorld.Add(new Point(i, j), rope);
						return;
					}
				}
			}
			
		}
	}
	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		Rope rope; 
		RopesOfAllThisTileInTheWorld.TryGetValue(pos, out rope);
		if (rope != null)
		{
			rope.Update(1);
			DrawLanternPiece(rope, pos, spriteBatch, tileDrawing);
		}
	}
	//绘制挂绳
	private void DrawLanternPiece(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = new Color())
	{
		// 回声涂料	
		//if (!TileDrawing.IsVisible(Main.tile[paintPos]))
		//	return;

		var tile = Main.tile[pos];
		ushort type = tile.TileType;
		int paint = Main.tile[pos].TileColor;
		//Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.LightbulbBand_bulbPath, type, 1, paint, tileDrawing);
		Texture2D tex = ModAsset.LightbulbBand_bulb.Value;

		int sizeX = 2;
		int sizeY = 2;


		// 支持发光涂料
		Color tileLight;
		if (color != new Color())
		{
			tileLight = color;
		}
		else
		{
			tileLight = Lighting.GetColor(pos);
		}
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(pos.X, pos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(pos.Y, pos.X, tile, type, 0, 0, tileLight);

		var origin = new Vector2(0, 0);
		var tileSpriteEffect = SpriteEffects.None;
		for(int i = 0;i < rope.GetMassList.Length - 1; i++)
		{
			_Mass thisMass = rope.GetMassList[i];
			_Mass nextMass = rope.GetMassList[i + 1];

			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1))
				windCycle = tileDrawing.GetWindCycle((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), tileDrawing._sunflowerWindCounter);
			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			float rotation = -windCycle * 0.4f;
			rope.ApplyForceSpecial(i, new Vector2(windCycle * 20, 0));

			Vector2 toNextMass = nextMass.Position - thisMass.Position;
			Vector2 drawPos = thisMass.Position - Main.screenPosition;
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, 2, 2, 2), tileLight, toNextMass.ToRotation(), new Vector2(1f), new Vector2(toNextMass.Length() / 2f, 1), tileSpriteEffect, 0);
			if(i % 4 == 2)
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 6, 10, 18), tileLight, rotation, new Vector2(5f, 0), 1f, tileSpriteEffect, 0);
				Lighting.AddLight(thisMass.Position, new Vector3(0.7f, 0.4f, 0.3f));
			}
		}
	}
}
