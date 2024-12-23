using Everglow.Commons.Physics.MassSpringSystem;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// 缆车节点
/// </summary>
public class CableCarJoint : CableTile
{
	public override void PostSetDefaults()
	{
		LampDistance = 2;
		RopeUnitMass = 0.8f;
		SingleLampMass = 0.8f;
		Elasticity = 180;
		MaxWireStyle = 6;
		MaxCableLength = 2500;
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (!RopesOfAllThisTileInTheWorld.ContainsKey(new Point(i, j)))
		{
			CableEneity cableEneity;
			TryGetCableEntityAs(i, j, out cableEneity);
			if (cableEneity != null)
			{
				AddRope(i, j, i + cableEneity.ToTail.X, j + cableEneity.ToTail.Y);
			}

			// Only use for Cable Car
			FindAndConnectRopeNearBy(i, j, 3);
		}
		base.NearbyEffects(i, j, closer);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Color lightColor = Lighting.GetColor(i, j);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		Tile tile = Main.tile[i, j];
		float rotation = 0;
		if (tile.TileFrameX == 18)
		{
			rotation = MathHelper.PiOver2;
		}
		if (tile.TileFrameX == 36)
		{
			rotation = MathHelper.Pi;
		}
		if (tile.TileFrameX == 54)
		{
			rotation = MathHelper.PiOver2 * 3;
		}
		spriteBatch.Draw(ModAsset.CableCarJoint.Value, new Point(i, j).ToWorldCoordinates() - Main.screenPosition + zero, new Rectangle(0, 0, 88, 56), lightColor, rotation, new Vector2(44, 6), 1, SpriteEffects.None, 0);

		TileFluentDrawManager.AddFluentPoint(this, i, j);
		foreach (Point point in RopeHeadAndTail.Keys)
		{
			if (RopeHeadAndTail[point] == new Point(i, j))
			{
				TileFluentDrawManager.AddFluentPoint(this, point.X, point.Y);
			}
		}
		return false;
	}

	public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
	{
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
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.CableCarJoint_Path, type, 1, paint, tileDrawing);
		tex ??= ModAsset.CableCarJoint.Value;
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
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, 56, 8, 8), tileLight, toNextMass.ToRotation(), new Vector2(4), new Vector2(toNextMass.Length() / 8f, 0.5f), tileSpriteEffect, 0);
		}
	}
}