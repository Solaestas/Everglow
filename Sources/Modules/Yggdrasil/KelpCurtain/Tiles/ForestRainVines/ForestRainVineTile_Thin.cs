using Everglow.Commons.DataStructures;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Commons.TileHelper;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.ForestRainVines;

public class ForestRainVineTile_Thin : HangingTile
{
	public override void InitHanging()
	{
		MaxWireStyle = 1;
		CanGrasp = true;

		RopeUnitMass = 3f;
		SingleLampMass = 250f;
		Elasticity = 200f;
		UnitLength = 6f;
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		if(j >= 20)
		{
			Tile tileUp = Main.tile[i, j - 1];
			if(tileUp.Slope != SlopeType.Solid)
			{
				tileUp.Slope = SlopeType.Solid;
			}
		}
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public override void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default)
	{
		base.DrawCable(rope, pos, spriteBatch, tileDrawing, color);
	}

	public override void DrawWinch(int i, int j, SpriteBatch spriteBatch)
	{
		RopeUnitMass = 3f;
		SingleLampMass = 250f;
		Elasticity = 200f;
		UnitLength = 6f;

		Color lightColor = Lighting.GetColor(i, j);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		Texture2D tex = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Rectangle frame = new Rectangle(0, 0, 16, 16);
		spriteBatch.Draw(tex, new Vector2(i, j) * 16 - Main.screenPosition + zero, frame, lightColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}

	public override void DrawRopeUnit(SpriteBatch spriteBatch, Texture2D texture, Vector2 drawPos, Point tilePos, Rope rope, int index, float rotation, Color tileLight)
	{
		var masses = rope.Masses;
		Rectangle frame;
		Vector2 offset = new Vector2(0, 0);
		drawPos += offset;
		int randomByPos = tilePos.X;
		if (index <= masses.Length - 10)
		{
			frame = new Rectangle(0, 8 + ((index + randomByPos) % 18) * 8, 16, 8);
			spriteBatch.Draw(texture, drawPos, frame, tileLight, rotation, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
		else
		{
			int contraryIndex = masses.Length - index;
			frame = new Rectangle(0, 152 + (10 - contraryIndex) * 8, 16, 8);
			spriteBatch.Draw(texture, drawPos, frame, tileLight, rotation, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
	}
}