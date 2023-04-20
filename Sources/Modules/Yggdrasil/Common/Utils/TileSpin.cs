namespace Everglow.Yggdrasil.Common.Utils;

internal class TileSpin
{
	public static Dictionary<(int, int), Vector2> TileRotation = new Dictionary<(int, int), Vector2>();
	/// <summary>
	/// 更新旋转
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="k1"></param>
	/// <param name="k2"></param>
	public void Update(int i, int j, float k1 = 0.75f, float k2 = 0.13f)
	{
		if (TileRotation.ContainsKey((i, j)) && !Main.gamePaused)
		{
			float rot;
			float Omega;
			Omega = TileRotation[(i, j)].X;
			rot = TileRotation[(i, j)].Y;
			Omega = Omega * k1 - rot * k2;

			if (Main.tile[i, j].WallType == 0)
				Omega = Omega * 0.99f - Math.Clamp(Main.windSpeedCurrent, -1, 1) * (0.3f + MathUtils.Sin(j + i + (float)Main.time / 12f) * 0.1f) * 0.2f;
			TileRotation[(i, j)] = new Vector2(Omega, rot + Omega);

			if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
				TileRotation.Remove((i, j));
		}
	}
	/// <summary>
	/// 专门绘制吊灯的
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="tex"></param>
	/// <param name="offsetX"></param>
	/// <param name="offsetY"></param>
	/// <param name="specialColor"></param>
	/// <param name="color"></param>
	public void DrawRotatedChandelier(int i, int j, Texture2D tex, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
	{
		float rot = 0;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		var tile = Main.tile[i, j];
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Color c = Lighting.GetColor(i, j);
		if (specialColor)
			c = color;
		if (TileRotation.ContainsKey((i, j)))
		{
			rot = TileRotation[(i, j)].Y;
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX - 18, 0, 54, 48), c, rot, new Vector2(27, 0), 1f, SpriteEffects.None, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX - 18, 0, 54, 48), c, rot, new Vector2(27, 0), 1f, SpriteEffects.None, 0f);
		}
	}
	/// <summary>
	/// 专门绘制吊灯的
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="tex"></param>
	/// <param name="offsetX"></param>
	/// <param name="offsetY"></param>
	/// <param name="specialColor"></param>
	/// <param name="color"></param>
	public void DrawRotatedLamp(int i, int j, Texture2D tex, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
	{
		float rot = 0;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		var tile = Main.tile[i, j];
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Color c = Lighting.GetColor(i, j);
		if (specialColor)
			c = color;
		if (TileRotation.ContainsKey((i, j)))
		{
			rot = TileRotation[(i, j)].Y;
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX, 0, 18, 34), c, rot, new Vector2(9, 0), 1f, SpriteEffects.None, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX, 0, 18, 34), c, rot, new Vector2(9, 0), 1f, SpriteEffects.None, 0f);
		}
	}
	/// <summary>
	/// 画旋转物块
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="tex"></param>
	/// <param name="sourceRectangle"></param>
	/// <param name="origin"></param>
	/// <param name="offsetX"></param>
	/// <param name="offsetY"></param>
	/// <param name="specialColor"></param>
	/// <param name="color"></param>
	public void DrawRotatedTile(int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
	{
		float rot = 0;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Color c = Lighting.GetColor(i, j);
		if (specialColor)
			c = color;
		if (TileRotation.ContainsKey((i, j)))
		{
			rot = TileRotation[(i, j)].Y;
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
		}
	}
	/// <summary>
	/// 画旋转物块
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="tex"></param>
	/// <param name="sourceRectangle"></param>
	/// <param name="origin"></param>
	/// <param name="offsetX"></param>
	/// <param name="offsetY"></param>
	/// <param name="kRot"></param>
	/// <param name="specialColor"></param>
	/// <param name="color"></param>
	public void DrawRotatedTilePrecise(int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, float kRot = 1, bool specialColor = false, Color color = new Color())
	{
		float rot = 0;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Color c = Lighting.GetColor(i, j);
		if (specialColor)
			c = color;
		if (TileRotation.ContainsKey((i, j)))
		{
			rot = TileRotation[(i, j)].Y;
			if (specialColor)
			{
				float maxC = Math.Max(color.R / 255f, Math.Abs(rot * 3) + Math.Abs(TileRotation[(i, j)].X * 3));
				maxC = Math.Clamp(maxC, 0, 1);
				c = new Color(maxC, maxC, maxC, 0);
			}
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot * kRot, origin, 1f, SpriteEffects.None, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot * kRot, origin, 1f, SpriteEffects.None, 0f);
		}
	}
	/// <summary>
	/// 画灯笼串
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="tex"></param>
	/// <param name="source1"></param>
	/// <param name="source2"></param>
	/// <param name="source3"></param>
	/// <param name="origin1"></param>
	/// <param name="origin2"></param>
	/// <param name="origin3"></param>
	/// <param name="offsetX"></param>
	/// <param name="offsetY"></param>
	/// <param name="kRot"></param>
	/// <param name="specialColor"></param>
	/// <param name="color"></param>
	public void DrawThreeLanternsString(int i, int j, Texture2D tex, Rectangle source1, Rectangle source2, Rectangle source3, Vector2 origin1, Vector2 origin2, Vector2 origin3, float offsetX = 0, float offsetY = 0, float kRot = 1, bool specialColor = false, Color color = new Color())
	{
		float rot = 0;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Color c = Lighting.GetColor(i, j);
		if (specialColor)
			c = color;
		Vector2 Position1 = Vector2.Zero;
		Vector2 Position2 = Position1 + new Vector2(0, 10);
		Vector2 Position3 = Position2 + new Vector2(0, 9);
		if (TileRotation.ContainsKey((i, j)))
		{
			rot = TileRotation[(i, j)].Y;
			if (specialColor)
			{
				float maxC = Math.Max(color.R / 255f, Math.Abs(rot * 3) + Math.Abs(TileRotation[(i, j)].X * 3));
				maxC = Math.Clamp(maxC, 0, 1);
				c = new Color(maxC, maxC, maxC, 0);
			}
			Position2 = Position1 + new Vector2(0, 10).RotatedBy(rot * kRot * 0.6f);
			Position3 = Position2 + new Vector2(0, 9).RotatedBy(rot * kRot * 1.0f);
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition + Position1, source1, c, rot * kRot * 0.6f, origin1, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition + Position2, source2, c, rot * kRot * 1.0f, origin2, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition + Position3, source3, c, rot * kRot * 1.6f, origin3, 1f, SpriteEffects.None, 0f);
		}
		else
		{
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition + Position1, source1, c, 0, origin1, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition + Position2, source2, c, 0, origin2, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition + Position3, source3, c, 0, origin3, 1f, SpriteEffects.None, 0f);
		}
	}
}
