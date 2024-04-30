using Everglow.Commons.Physics;
using Everglow.Commons.TileHelper;
using Terraria.GameContent.Drawing;
using static Everglow.Commons.Physics.Rope;

namespace Everglow.CagedDomain.Tiles;

public class LightbulbBand : CableTile
{
	public override void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default)
	{
		// 回声涂料	
		if (!TileDrawing.IsVisible(Main.tile[pos]))
			return;

		var tile = Main.tile[pos];
		ushort type = tile.TileType;
		int paint = Main.tile[pos].TileColor;
		Texture2D tex = ModAsset.LightbulbBand_bulb.Value;
		var origin = new Vector2(0, 0);
		var tileSpriteEffect = SpriteEffects.None;
		for (int i = 0; i < rope.GetMassList.Length - 1; i++)
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
			if (!Main.gamePaused)
			{
				rope.ApplyForceSpecial(i, new Vector2(windCycle * 1, 4 * thisMass.Mass));
			}
			// 支持发光涂料
			Color tileLight;
			if (color != new Color())
			{
				tileLight = color;
			}
			else
			{
				tileLight = Lighting.GetColor((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f)) * 10;
			}
			tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(pos.X, pos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
			tileLight = tileDrawing.DrawTiles_GetLightOverride(pos.X, pos.Y, tile, type, 0, 0, tileLight);
			Vector2 toNextMass = nextMass.Position - thisMass.Position;
			Vector2 drawPos = thisMass.Position - Main.screenPosition;
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, 2, 2, 2), tileLight, toNextMass.ToRotation(), new Vector2(1f), new Vector2(toNextMass.Length() / 2f, 1), tileSpriteEffect, 0);
			if (thisMass.Mass == 2)
			{
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 6, 10, 18), tileLight, rotation, new Vector2(5f, 0), 1f, tileSpriteEffect, 0);
				Lighting.AddLight(thisMass.Position, new Vector3(0.7f, 0.4f, 0.3f));
			}
		}
	}
}
