using Everglow.Commons.Physics;
using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly;

internal class TileSpin
{
	public static Dictionary<(int, int), Vector2> TileRotation = new Dictionary<(int, int), Vector2>();

	/// <summary>
	/// 此项仅用于复杂的组合吊灯
	/// </summary>
	/// <param name="ij"></param>
	/// <param name="strength"></param>
	public static void ActivateTileSpin((int, int) ij, float strength)
	{
		if (!TileRotation.ContainsKey(ij))
			TileRotation.Add(ij, new Vector2(-Math.Clamp(strength, -1, 1) * 0.2f));
		else
		{
			float rot;
			float omega;
			omega = TileRotation[ij].X;
			rot = TileRotation[ij].Y;
			if (CanAddForce(rot, strength) || CanAddForce(omega, strength))
				TileRotation[ij] = new Vector2(omega - Math.Clamp(strength, -1, 1) * 0.2f, rot + omega - Math.Clamp(strength, -1, 1) * 0.2f);
			if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
				TileRotation.Remove(ij);
		}
	}
	public static bool CanAddForce(float value1, float value2)
	{
		return value1 * value2 < 0 || Math.Abs(value1) < Math.Abs(value2);
	}
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
	/// 更新贴图旋转，并抖落蓝色荧光花粉dust
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="k1"></param>
	/// <param name="k2"></param>
	public void UpdateBlackShrub(int i, int j, float k1 = 0.75f, float k2 = 0.13f, Vector2 offset = new Vector2(), float Addx = 0, float Addy = 0, int width = 16, int height = 16)
	{
		if (TileRotation.ContainsKey((i, j)) && !Main.gamePaused)
		{
			float rot;
			float omega;
			omega = TileRotation[(i, j)].X;
			rot = TileRotation[(i, j)].Y;
			omega = omega * 0.75f - rot * 0.13f;
			TileRotation[(i, j)] = new Vector2(omega, rot + omega);
			float Strength = Math.Abs(omega) + Math.Abs(rot);
			if (Main.rand.NextBool(Math.Clamp((int)(100 - Strength * 1200f * k1), 1, 900)))
			{
				var d = Dust.NewDustDirect(new Vector2(i * 16 + Addx, j * 16 + Addy) + offset.RotatedBy(rot), width, height, ModContent.DustType<BlueParticleDark>());
				var d2 = Dust.NewDustDirect(new Vector2(i * 16 + Addx, j * 16 + Addy) + offset.RotatedBy(rot), width, height, ModContent.DustType<BlueParticle>());
				d.alpha = (int)(255 - Strength * 2000f * k1);
				d2.scale = Strength * 10f;
				d.scale *= k1;
				d.velocity = (offset.RotatedBy(rot) - offset.RotatedBy(rot - omega)) * k1 * 1f;
				d2.scale *= k1;
				d2.velocity = (offset.RotatedBy(rot) - offset.RotatedBy(rot - omega)) * k1 * 1f;
			}
			if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
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
	public void DrawRotatedChandelier(SpriteBatch spriteBatch, int i, int j, Texture2D tex, float offsetX = 8, float offsetY = 0, int frameX = -1, int frameY = -1, int width = 54, int height = 48, bool specialColor = false, Color color = new Color())
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
			int frX = tile.TileFrameX - 18;
			if(frameX != -1)
			{
				frX = frameX;
			}
			int frY = 0;
			if(frY != -1)
			{
				frY = frameY;
			}
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(frX, frY, width, height), c, rot, new Vector2(width / 2f, 0), 1f, SpriteEffects.None, 0f);
		}
		else
		{
			int frX = tile.TileFrameX - 18;
			if (frameX != -1)
			{
				frX = frameX;
			}
			int frY = 0;
			if (frY != -1)
			{
				frY = frameY;
			}
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(frX, frY, width, height), c, rot, new Vector2(width / 2f, 0), 1f, SpriteEffects.None, 0f);
		}
	}

	/// <summary>
	/// 专门绘制灯笼的
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="tex"></param>
	/// <param name="offsetX"></param>
	/// <param name="offsetY"></param>
	/// <param name="specialColor"></param>
	/// <param name="color"></param>
	public void DrawRotatedLamp(SpriteBatch spriteBatch, int i, int j, Texture2D tex, float offsetX = 0, float offsetY = 0, int width = 18, int height = 34, bool specialColor = false, Color color = new Color())
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
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX, 0, width, height), c, rot, new Vector2(width / 2f, 0), 1f, SpriteEffects.None, 0f);
		}
		else
		{
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, new Rectangle(tile.TileFrameX, 0, width, height), c, rot, new Vector2(width / 2f, 0), 1f, SpriteEffects.None, 0f);
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
	public void DrawRotatedTile(SpriteBatch spriteBatch, int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, bool specialColor = false, Color color = new Color())
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
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
		}
		else
		{
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot, origin, 1f, SpriteEffects.None, 0f);
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
	public void DrawRotatedTile(SpriteBatch spriteBatch, int i, int j, Texture2D tex, Rectangle sourceRectangle, Vector2 origin, float offsetX = 0, float offsetY = 0, float kRot = 1, bool specialColor = false, Color color = new Color())
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
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot * kRot, origin, 1f, SpriteEffects.None, 0f);
		}
		else
		{
			spriteBatch.Draw(tex, new Vector2(i * 16 + offsetX, j * 16 + offsetY) + zero - Main.screenPosition, sourceRectangle, c, rot * kRot, origin, 1f, SpriteEffects.None, 0f);
		}
	}

	/// <summary>
	/// 芦苇类杆状绘制
	/// </summary>
	public void DrawReed(SpriteBatch spriteBatch, int i, int j, int Length, Texture2D texFlower, Texture2D texReed, Rectangle flowerRectangle, Rectangle reedRectangle, Vector2 flowerOrigin, Vector2 reedOrigin, float offsetX = 0, float offsetY = 0, float kRot = 1, bool specialColor = false, Color color = new Color())
	{
		float rot;
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Color c = Lighting.GetColor(i, j);
		if (specialColor)
			c = color;

		if (TileRotation.ContainsKey((i, j)))//有旋转
		{
			rot = TileRotation[(i, j)].Y;

			if (specialColor)
			{
				float maxC = color.B / 255f;
				maxC = Math.Clamp(maxC, 0, 1);
				c = new Color(maxC * 0, maxC * 0.6f, maxC, 0);
			}
			float Totaldx = 0;
			for (int x = 0; x < Length; x++)
			{
				float value0 = Length - x;
				float LowerRotation = 1 / value0 * rot * kRot;

				float dx = MathUtils.Sin(LowerRotation) * 16;

				if (x == Length - 1)
				{
					texReed = texFlower;
					reedRectangle = flowerRectangle;
					reedOrigin = flowerOrigin;
				}
				spriteBatch.Draw(texReed, new Vector2(i * 16 + offsetX + Totaldx, (j - x) * 16 + offsetY) + zero - Main.screenPosition, reedRectangle, c, LowerRotation, reedOrigin, new Vector2(1f, MathUtils.Sqrt(256 + dx * dx) / 16f + 0.1f), SpriteEffects.None, 0f);
				Totaldx += dx;
			}
		}
		else//无旋转
		{
			rot = 0;
			if (specialColor)
			{
				float maxC = color.B / 255f;
				maxC = Math.Clamp(maxC, 0, 1);
				c = new Color(maxC * 0, maxC * 0.6f, maxC, 0);
			}
			for (int x = 0; x < Length; x++)
			{
				if (x == Length - 1)
				{
					texReed = texFlower;
					reedRectangle = flowerRectangle;
					reedOrigin = flowerOrigin;
				}
				spriteBatch.Draw(texReed, new Vector2(i * 16 + offsetX, (j - x) * 16 + offsetY) + zero - Main.screenPosition, reedRectangle, c, rot * kRot, reedOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	/// <summary>
	/// 绘制连线
	/// </summary>
	public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color c0, float Wid, Texture2D tex)
	{
		Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

		var vertex2Ds = new List<Vertex2D>();

		for (int x = 0; x < 3; x++)
		{
			vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x) - Main.screenPosition, c0, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x) - Main.screenPosition, c0, new Vector3(0, 1, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x) - Main.screenPosition, c0, new Vector3(1, 0, 0)));

			vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x) - Main.screenPosition, c0, new Vector3(0, 1, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x) - Main.screenPosition, c0, new Vector3(1, 1, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x) - Main.screenPosition, c0, new Vector3(1, 0, 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = tex;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
	}
}
