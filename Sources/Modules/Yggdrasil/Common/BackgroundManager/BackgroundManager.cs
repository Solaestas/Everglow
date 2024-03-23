using Everglow.Commons.DataStructures;

namespace Everglow.Yggdrasil.Common.BackgroundManager;

public class BackgroundManager
{
	public class WorldBackground
	{
		public float Layer = 2;//绘制层,景深
		public int Priority = 0;//同层下绘制优先级
		public Vector2 Center;//中心坐标
		public Color Color;//颜色
		public Rectangle DrawRectangle;//绘制域
		public bool Active;//是否绘制，优先级最高
		public Texture2D Texture;//图片
		public float scale = 1;//大小
	}
	public class BoardBackground : WorldBackground
	{
		public bool XClamp = false;
		public bool YClamp = true;
	}
	public class PointBackground : WorldBackground
	{
		public float Rotation = 0;
		public Vector2 Velocity;
		public float[] ai = new float[8];
		public void Update()
		{

		}
		public void SpecialDraw(SpriteBatch spriteBatch)
		{

		}
	}

	public static void QuickDrawBG(Texture2D tex, Rectangle drawArea, Color baseColor, int Ymin, int Ymax, bool Xclamp = false, bool Yclamp = true)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Effect bgW = ModAsset.BackgroundXWarp.Value;
		if (Xclamp && Yclamp)
			bgW = ModAsset.BackgroundXYClamp.Value;
		if (Xclamp && !Yclamp)
			bgW = ModAsset.BackgroundYWarp.Value;
		if (!Xclamp && !Yclamp)
			bgW = ModAsset.BackgroundXYWarp.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		if (Main.LocalPlayer.gravDir == -1)
			projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, 0, Main.screenHeight, 0, 1);
		bgW.Parameters["uTransform"].SetValue(projection);
		bgW.Parameters["uTime"].SetValue(0.34f);
		bgW.CurrentTechnique.Passes[0].Apply();

		//处理掉超出地图界限的部分
		int DrawMaxY = Main.screenHeight;
		int DrawMinY = 0;
		float YSqueezeValueUp = 0f;
		float YSqueezeValueDown = 1f;
		if (Main.screenPosition.Y + Main.screenHeight > Ymax)
		{
			DrawMaxY = Ymax - (int)Main.screenPosition.Y;
			YSqueezeValueDown = (float)DrawMaxY / Main.screenHeight;
		}
		if (Main.screenPosition.Y < Ymin)
		{
			DrawMinY = Ymin - (int)Main.screenPosition.Y;
			YSqueezeValueUp = DrawMinY / (float)Main.screenHeight;
		}

		var CloseII = new List<Vertex2D>
		{
			new Vertex2D(new Vector2(0, DrawMinY), baseColor, new Vector3(drawArea.X / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueUp) / tex.Height, 0)),
			new Vertex2D(new Vector2(Main.screenWidth, DrawMinY), baseColor, new Vector3((drawArea.X + drawArea.Width) / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueUp)/ tex.Height, 0)),
			new Vertex2D(new Vector2(0, DrawMaxY), baseColor, new Vector3(drawArea.X / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueDown) / tex.Height, 0)),

			new Vertex2D(new Vector2(0, DrawMaxY), baseColor, new Vector3(drawArea.X / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueDown) / tex.Height, 0)),
			new Vertex2D(new Vector2(Main.screenWidth, DrawMinY), baseColor, new Vector3((drawArea.X + drawArea.Width) / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueUp) / tex.Height, 0)),
			new Vertex2D(new Vector2(Main.screenWidth, DrawMaxY), baseColor, new Vector3((drawArea.X + drawArea.Width) / (float)tex.Width, (drawArea.Y + drawArea.Height * YSqueezeValueDown) / tex.Height, 0))
		};
		if (CloseII.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = tex;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, CloseII.ToArray(), 0, 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public static void DrawWaterfallInBackground(Vector2 biomeCenter, float moveStep, Vector2 positionToTextureCenter, float Width, float Height, Color baseColor, int Ymin, int Ymax, Vector2 textureSize = new Vector2(), bool Xclamp = false, bool Yclamp = true)
	{
		Vector2 HalfScreenSize = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
		Vector2 ScreenCenter = Main.screenPosition + HalfScreenSize;
		Vector2 deltaPos = ScreenCenter - biomeCenter;
		deltaPos *= moveStep;
		Vector2 DrawCenter = HalfScreenSize - deltaPos + positionToTextureCenter;
		if (Xclamp)
		{
			if (Yclamp)
				DrawWaterfall(DrawCenter, Width, Height, baseColor, Ymin, Ymax);
			else
			{
				Vector2 dCenter = DrawCenter;
				while (dCenter.Y > Main.screenHeight + 160)
				{
					dCenter.Y -= textureSize.Y;
				}
				while (dCenter.Y >= -Height && dCenter.Y <= Main.screenHeight + 160)
				{
					DrawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
					dCenter.Y -= textureSize.Y;
				}

				dCenter = DrawCenter;
				if (dCenter.Y >= -Height && dCenter.Y <= Main.screenHeight + 160)
					dCenter.Y += textureSize.Y;
				while (dCenter.Y < -Height)
				{
					dCenter.Y += textureSize.Y;
				}
				while (dCenter.Y >= -Height && dCenter.Y <= Main.screenHeight + 160)
				{
					DrawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
					dCenter.Y += textureSize.Y;
				}
			}
		}
		else if (Yclamp)
		{
			Vector2 dCenter = DrawCenter;
			while (dCenter.X > Main.screenWidth + Width)
			{
				dCenter.X -= textureSize.X;
			}
			while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
			{
				DrawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
				dCenter.X -= textureSize.X;

			}
			dCenter = DrawCenter;
			if (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
				dCenter.X += textureSize.X;
			while (dCenter.X < -Width)
			{
				dCenter.X += textureSize.X;
			}
			while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
			{
				DrawWaterfall(dCenter, Width, Height, baseColor, Ymin, Ymax);
				dCenter.X += textureSize.X;
			}
		}
		else
		{
			Vector2 dCenter = DrawCenter;
			while (dCenter.X > Main.screenWidth + Width)
			{
				dCenter.X -= textureSize.X;
			}
			while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
			{
				Vector2 ddCenter = dCenter;
				while (ddCenter.Y > Main.screenHeight + 160)
				{
					ddCenter.Y -= textureSize.Y;
				}
				while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
				{
					DrawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
					ddCenter.Y -= textureSize.Y;
				}
				ddCenter = dCenter;
				if (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
					ddCenter.Y += textureSize.Y;
				while (ddCenter.Y < -Height)
				{
					ddCenter.Y += textureSize.Y;
				}
				while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
				{
					ddCenter.Y += textureSize.Y;
					DrawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
				}
				dCenter.X -= textureSize.X;
			}
			dCenter = DrawCenter;
			if (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
				dCenter.X += textureSize.X;
			while (dCenter.X < -Width)
			{
				dCenter.X += textureSize.X;
			}
			while (dCenter.X >= -Width && dCenter.X <= Main.screenWidth + Width)
			{
				Vector2 ddCenter = dCenter;
				while (ddCenter.Y > Main.screenHeight + 160)
				{
					ddCenter.Y -= textureSize.Y;
				}
				while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
				{
					DrawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
					ddCenter.Y -= textureSize.Y;
				}
				ddCenter = dCenter;
				if (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
					ddCenter.Y += textureSize.Y;
				while (ddCenter.Y < -Height)
				{
					ddCenter.Y += textureSize.Y;
				}
				while (ddCenter.Y >= -Height && ddCenter.Y <= Main.screenHeight + 160)
				{
					ddCenter.Y += textureSize.Y;
					DrawWaterfall(ddCenter, Width, Height, baseColor, Ymin, Ymax);
				}
				dCenter.X += textureSize.X;
			}
		}
	}
	public static void DrawWaterfall(Vector2 drawCenter, float width, float height, Color baseColor, int Ymin, int Ymax)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		if (Main.LocalPlayer.gravDir == -1)
			projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, 0, Main.screenHeight, 0, 1);
		Effect bgW = ModAsset.BackgroundYWarp.Value;
		bgW.Parameters["uTransform"].SetValue(projection);
		bgW.Parameters["uTime"].SetValue(0.34f);
		bgW.CurrentTechnique.Passes[0].Apply();

		float Time = (float)-Main.timeForVisualEffects / 40f;
		var WaterFallVertex = new List<Vertex2D>();
		for (float y = 0; y < height; y += 10f)
		{
			float ColorAlpha = Math.Min(y / 100f, 1);
			if (height - y < 100)
				ColorAlpha = Math.Max((height - y) / 100f, 0);
			float WorldYCoordinate = drawCenter.Y + y + Main.screenPosition.Y;
			if (WorldYCoordinate > Ymax || WorldYCoordinate < Ymin)
				ColorAlpha *= 0;
			WaterFallVertex.Add(new Vertex2D(new Vector2(drawCenter.X + width / 2f, drawCenter.Y + y), baseColor * ColorAlpha, new Vector3(0, (float)Math.Pow(y, 0.6) / 10f + Time, 0)));
			WaterFallVertex.Add(new Vertex2D(new Vector2(drawCenter.X - width / 2f, drawCenter.Y + y), baseColor * ColorAlpha, new Vector3(1, (float)Math.Pow(y, 0.6) / 10f + Time, 0)));
		}
		if (WaterFallVertex.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.WaterFall.Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, WaterFallVertex.ToArray(), 0, WaterFallVertex.Count - 2);
		}

		var WaterFallVertexII = new List<Vertex2D>();
		for (float y = 0; y < height; y += 10f)
		{
			float ColorAlpha = Math.Min(y / 100f, 1);
			if (height - y < 100)
				ColorAlpha = Math.Max((height - y) / 100f, 0);
			float WorldYCoordinate = drawCenter.Y + y + Main.screenPosition.Y;
			if (WorldYCoordinate > Ymax || WorldYCoordinate < Ymin)
				ColorAlpha *= 0;
			WaterFallVertexII.Add(new Vertex2D(new Vector2(drawCenter.X + width / 2f, drawCenter.Y + y), baseColor * ColorAlpha * 1.63f, new Vector3(0, (float)Math.Pow(y, 0.4) / 10f + Time, 0)));
			WaterFallVertexII.Add(new Vertex2D(new Vector2(drawCenter.X - width / 2f, drawCenter.Y + y), baseColor * ColorAlpha * 1.63f, new Vector3(1, (float)Math.Pow(y, 0.4) / 10f + Time, 0)));
		}
		if (WaterFallVertexII.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = ModAsset.WaterFall.Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, WaterFallVertexII.ToArray(), 0, WaterFallVertexII.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}