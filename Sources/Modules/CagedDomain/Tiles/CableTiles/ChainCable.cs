using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Commons.TileHelper;
using Terraria.GameContent.Drawing;

namespace Everglow.CagedDomain.Tiles.CableTiles;

public class ChainCable : CableTile
{
	public override void PostSetDefaults()
	{
		LampDistance = 2;
		RopeUnitMass = 0.8f;
		SingleLampMass = 0.2f;
		MaxWireStyle = 1;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		return base.PreDraw(i, j, spriteBatch);
	}

	public override void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default)
	{
		// 回声涂料
		if (!TileDrawing.IsVisible(Main.tile[pos]))
		{
			return;
		}

		var tile = Main.tile[pos];
		ushort type = tile.TileType;
		int paint = Main.tile[pos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.ChainCable_Path, type, 1, paint, tileDrawing);
		tex ??= ModAsset.ChainCable.Value;
		var tileSpriteEffect = SpriteEffects.None;

		// 获取发绳端物块信息
		var masses = rope.Masses;
		for (int i = 0; i < masses.Length - 1; i++)
		{
			Mass thisMass = masses[i];
			Mass nextMass = masses[i + 1];

			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1))
			{
				windCycle = tileDrawing.GetWindCycle((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), tileDrawing._sunflowerWindCounter);
			}

			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			if (!Main.gamePaused)
			{
				rope.ApplyForceSpecial(i, new Vector2(windCycle / 4.0f, 0.4f * thisMass.Value));
			}

			// 支持发光涂料
			Color tileLight;
			if (color != default(Color))
			{
				tileLight = color;
			}
			else
			{
				tileLight = Lighting.GetColor((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f));
			}
			Vector2 toNextMass = nextMass.Position - thisMass.Position;
			Vector2 drawPos = thisMass.Position - Main.screenPosition;
			if (i % 2 == 0)
			{
				spriteBatch.Draw(tex, drawPos, null, tileLight, toNextMass.ToRotation() + MathHelper.PiOver2, tex.Size() * 0.5f, 1, tileSpriteEffect, 0);
			}
		}
	}
}