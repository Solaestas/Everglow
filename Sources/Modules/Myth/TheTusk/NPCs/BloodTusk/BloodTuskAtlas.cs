using Terraria;

namespace Everglow.Myth.TheTusk.NPCs.BloodTusk;

public class BloodTuskAtlas
{
	public struct DrawPiece
	{
		public Rectangle DrawRectangle;
		public Vector2 Offset0;
		public Vector2 Offset1;

		public DrawPiece(Rectangle rectangle, Vector2 offset0 = default, Vector2 offset1 = default)
		{
			DrawRectangle = rectangle;
			this.Offset0 = offset0;
			this.Offset1 = offset1 == default ? offset0 : offset1;
		}
	}

	public static DrawPiece Phase2 = new DrawPiece(new Rectangle(0, 0, 220, 216), new Vector2(-1, 28));
	public static DrawPiece Phase1 = new DrawPiece(new Rectangle(226, 0, 220, 216), new Vector2(-1, 28));

	public static DrawPiece SubTusk0 = new DrawPiece(new Rectangle(220, 294, 22, 32), new Vector2(-76, 66), new Vector2(-66, 86));
	public static DrawPiece SubTusk1 = new DrawPiece(new Rectangle(254, 280, 22, 48), new Vector2(-50, 54), new Vector2(-34, 104));
	public static DrawPiece SubTusk2 = new DrawPiece(new Rectangle(280, 300, 20, 24), new Vector2(-23, 84), new Vector2(-8, 100));
	public static DrawPiece SubTusk3 = new DrawPiece(new Rectangle(308, 288, 12, 38), new Vector2(-24, 45), new Vector2(-24, 90));
	public static DrawPiece SubTusk4 = new DrawPiece(new Rectangle(328, 298, 16, 24), new Vector2(9, 86), new Vector2(0, 100));
	public static DrawPiece SubTusk5 = new DrawPiece(new Rectangle(354, 296, 12, 30), new Vector2(28, 61), new Vector2(28, 100));
	public static DrawPiece SubTusk6 = new DrawPiece(new Rectangle(376, 290, 20, 34), new Vector2(41, 69), new Vector2(30, 90));
	public static DrawPiece SubTusk7 = new DrawPiece(new Rectangle(404, 292, 30, 30), new Vector2(68, 79), new Vector2(40, 96));

	public static DrawPiece SubTusk0_2 = new DrawPiece(new Rectangle(220, 242, 22, 32), new Vector2(-76, 66), new Vector2(-66, 86));
	public static DrawPiece SubTusk1_2 = new DrawPiece(new Rectangle(254, 228, 22, 48), new Vector2(-50, 54), new Vector2(-34, 104));
	public static DrawPiece SubTusk2_2 = new DrawPiece(new Rectangle(280, 248, 20, 24), new Vector2(-23, 84), new Vector2(-8, 100));
	public static DrawPiece SubTusk3_2 = new DrawPiece(new Rectangle(308, 236, 12, 38), new Vector2(-24, 45), new Vector2(-24, 90));
	public static DrawPiece SubTusk4_2 = new DrawPiece(new Rectangle(328, 246, 16, 24), new Vector2(9, 86), new Vector2(0, 100));
	public static DrawPiece SubTusk5_2 = new DrawPiece(new Rectangle(354, 244, 12, 30), new Vector2(28, 61), new Vector2(28, 100));
	public static DrawPiece SubTusk6_2 = new DrawPiece(new Rectangle(376, 238, 20, 34), new Vector2(41, 69), new Vector2(30, 90));
	public static DrawPiece SubTusk7_2 = new DrawPiece(new Rectangle(404, 240, 30, 30), new Vector2(68, 79), new Vector2(40, 96));

	public static DrawPiece Gum_Surface = new DrawPiece(new Rectangle(26, 252, 174, 68), new Vector2(2, 94));
	public static DrawPiece Gum_Surface_Center = new DrawPiece(new Rectangle(92, 270, 30, 50), new Vector2(-4, 103));
	public static DrawPiece Gum_Middle = new DrawPiece(new Rectangle(26, 328, 174, 60), new Vector2(2, 88));
	public static DrawPiece Gum_Bottom = new DrawPiece(new Rectangle(0, 390, 220, 52), new Vector2(-1, 110));

	public static DrawPiece Tusk0 = new DrawPiece(new Rectangle(366, 346, 50, 160), Vector2.zeroVector, new Vector2(0, 150));
	public static DrawPiece Tusk_Black = new DrawPiece(new Rectangle(300, 346, 50, 160), Vector2.zeroVector, new Vector2(0, 150));
	public static DrawPiece Tusk1 = new DrawPiece(new Rectangle(232, 346, 50, 160), Vector2.zeroVector, new Vector2(0, 150));

	public static DrawPiece StickyBlood = new DrawPiece(new Rectangle(450, 0, 152, 450));
	public static DrawPiece BloodDrop0 = new DrawPiece(new Rectangle(620, 0, 30, 192));
	public static DrawPiece BloodDrop1 = new DrawPiece(new Rectangle(668, 4, 30, 150));
	public static DrawPiece BloodDrop2 = new DrawPiece(new Rectangle(710, 4, 12, 138));
	public static DrawPiece BloodDrop3 = new DrawPiece(new Rectangle(732, 4, 14, 98));
}

public static class DrawPieceExtensions
{
	public static void Draw(this BloodTuskAtlas.DrawPiece drawPiece, NPC tusk, List<Vertex2D> bars, float specialCower = -1)
	{
		Texture2D texture = ModAsset.BloodTusk_Atlas.Value;
		float cower = (tusk.ModNPC as BloodTusk)?.CowerValue ?? 0;
		if(specialCower is >= 0 and <= 1)
		{
			cower = specialCower;
		}
		Vector2 drawPos = tusk.Center + Vector2.Lerp(drawPiece.Offset0, drawPiece.Offset1, cower) + new Vector2(0, -20);
		Rectangle rectangle = drawPiece.DrawRectangle;
		if (rectangle.Height / 2 + drawPos.Y > tusk.Bottom.Y + 64)
		{
			rectangle.Height -= (int)(rectangle.Height / 2f + drawPos.Y - tusk.Bottom.Y - 64) * 2;
		}
		rectangle.Height = Math.Max(rectangle.Height, 0);
		float alpha = (255 - tusk.alpha) / 255f;
		float rotation = tusk.rotation;
		Vector2 topLeft = rectangle.TopLeft() / texture.Size();
		Vector2 topRight = rectangle.TopRight() / texture.Size();
		Vector2 bottomLeft = rectangle.BottomLeft() / texture.Size();
		Vector2 bottomRight = rectangle.BottomRight() / texture.Size();
		AddVertex(bars, drawPos + new Vector2(-rectangle.Width, -rectangle.Height).RotatedBy(rotation) * 0.5f, new Vector3(topLeft, 0), alpha);
		AddVertex(bars, drawPos + new Vector2(rectangle.Width, -rectangle.Height).RotatedBy(rotation) * 0.5f, new Vector3(topRight, 0), alpha);
		AddVertex(bars, drawPos + new Vector2(-rectangle.Width, rectangle.Height).RotatedBy(rotation) * 0.5f, new Vector3(bottomLeft, 0), alpha);

		AddVertex(bars, drawPos + new Vector2(rectangle.Width, -rectangle.Height).RotatedBy(rotation) * 0.5f, new Vector3(topRight, 0), alpha);
		AddVertex(bars, drawPos + new Vector2(rectangle.Width, rectangle.Height).RotatedBy(rotation) * 0.5f, new Vector3(bottomRight, 0), alpha);
		AddVertex(bars, drawPos + new Vector2(-rectangle.Width, rectangle.Height).RotatedBy(rotation) * 0.5f, new Vector3(bottomLeft, 0), alpha);
	}

	private static void AddVertex(List<Vertex2D> bars, Vector2 position, Vector3 coord, float alpha)
	{
		bars.Add(position, Lighting.GetColor(position.ToTileCoordinates()) * alpha, coord);
	}

	public static Vector2 GetOrigin(this BloodTuskAtlas.DrawPiece drawPiece)
	{
		return drawPiece.DrawRectangle.Size() * 0.5f;
	}
}