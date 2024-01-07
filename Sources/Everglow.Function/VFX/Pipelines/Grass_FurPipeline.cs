using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.VFX.Pipelines;

/// <summary>
/// 草皮Pipeline,合批到RenderTarget2D上进行脏绘制(还没实现),技术和算力允许的条件下尝试用势能图(还没实现)使草皮弯曲,会自动-Main.screenPosition
/// </summary>
public class Grass_FurPipeline : Pipeline
{
	private RenderTarget2D grass_FurScreen;
	private RenderTarget2D grass_FurScreenSwap;
	public Vector2 TotalMovedPosition;//距离上次刷新的屏幕坐标
	public Vector2 LastRenderPosition;//上次刷新的屏幕坐标
	public float RefreshDistance = 200f;//刷新阈值
	public override void Load()
	{
		Ins.MainThread.AddTask(() =>
		{
			AllocateRenderTarget(new Vector2(Main.screenWidth, Main.screenHeight));
		});
		Ins.HookManager.AddHook(CodeLayer.ResolutionChanged, (Vector2 size) =>
		{
			grass_FurScreen?.Dispose();
			grass_FurScreenSwap?.Dispose();
			AllocateRenderTarget(size);
		}, "Realloc RenderTarget");
		effect = VFXManager.DefaultEffect;
	}

	private void AllocateRenderTarget(Vector2 size)
	{
		var gd = Main.instance.GraphicsDevice;
		grass_FurScreen = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
		grass_FurScreenSwap = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
	}
	public override void BeginRender()
	{
		var sb = Main.spriteBatch;
		var gd = Main.instance.GraphicsDevice;
		//保存原画
		sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.Invert(Main.GameViewMatrix.TransformationMatrix));
		var cur = Main.screenTarget;
		gd.SetRenderTarget(grass_FurScreenSwap);
		gd.Clear(Color.Transparent);
		sb.Draw(cur, Vector2.Zero, Color.White);
		sb.End();
		//准备绘制草皮
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		gd.SetRenderTarget(grass_FurScreen);
		gd.Clear(Color.Transparent);
		effect.Value.Parameters["uTransform"].SetValue(
			Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.EffectMatrix *
			Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		effect.Value.CurrentTechnique.Passes[0].Apply();
	}
	public override void Render(IEnumerable<IVisual> visuals)
	{
		if (grass_FurScreen == null)
		{
			return;
		}
		TotalMovedPosition = Main.screenPosition - LastRenderPosition;
		if (TotalMovedPosition.Length() <= RefreshDistance)//未超出刷新阈值,直接画已经处理好的RenderTarget即可结束渲染
		{
			EndRender();
			return;
		}

		BeginRender();

		LastRenderPosition = Main.screenPosition;

		foreach (var visual in visuals)
		{
			visual.Draw();
		}
		EndRender();
	}
	public void ReserRenderTarget()
	{

	}
	public override void EndRender()
	{
		var sb = Main.spriteBatch;
		var gd = Main.instance.GraphicsDevice;
		List<Vertex2D> bars = new List<Vertex2D>();
		//未超出刷新阈值，直接用已经处理好的RenderTarget渲染。
		if (TotalMovedPosition.Length() <= RefreshDistance)
		{
			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (int y = 0; y < 31; y++)
			{
				for (int x = 0; x < 30; x++)
				{
					float posX = -TotalMovedPosition.X + x / 30f * grass_FurScreen.Width;
					float posY = -TotalMovedPosition.Y + y / 30f * grass_FurScreen.Height;
					Color lightColor0 = Lighting.GetColor((int)(posX + LastRenderPosition.X + TotalMovedPosition.X) / 16, (int)(posY + LastRenderPosition.Y + TotalMovedPosition.Y) / 16);
					Color lightColor2 = Lighting.GetColor((int)(posX + LastRenderPosition.X + TotalMovedPosition.X) / 16, (int)(posY + LastRenderPosition.Y + grass_FurScreen.Height / 30f + TotalMovedPosition.Y) / 16);
					if (x == 0)
					{
						bars.Add(new Vector2(posX, posY), Color.Transparent, new Vector3(x / 30f, y / 30f, 0));
						bars.Add(new Vector2(posX, posY + grass_FurScreen.Height / 30f), Color.Transparent, new Vector3(x / 30f, (y + 1) / 30f, 0));
					}
					bars.Add(new Vector2(posX, posY), lightColor0, new Vector3(x / 30f, y / 30f, 0));
					bars.Add(new Vector2(posX, posY + grass_FurScreen.Height / 30f), lightColor2, new Vector3(x / 30f, (y + 1) / 30f, 0));
					if (x == 29)
					{
						bars.Add(new Vector2(posX, posY), Color.Transparent, new Vector3(x / 30f, y / 30f, 0));
						bars.Add(new Vector2(posX, posY + grass_FurScreen.Height / 30f), Color.Transparent, new Vector3(x / 30f, (y + 1) / 30f, 0));
					}
				}
			}
			if (bars.Count > 2)
			{
				Main.graphics.GraphicsDevice.Textures[0] = grass_FurScreen;//TextureAssets.MagicPixel.Value
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			sb.End();
			return;
		}
		//超出刷新阈值的瞬间帧用新处理的RenderTarget。
		Ins.Batch.End();

		gd.SetRenderTarget(Main.screenTarget);
		gd.Clear(Color.Transparent);
		sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		sb.Draw(grass_FurScreenSwap, Vector2.Zero, Color.White);

		for (int y = 0; y < 31; y++)
		{
			for (int x = 0; x < 30; x++)
			{
				float posX = x / 30f * grass_FurScreen.Width;
				float posY = y / 30f * grass_FurScreen.Height;
				Color lightColor0 = Lighting.GetColor((int)(posX + TotalMovedPosition.X) / 16, (int)(posY + TotalMovedPosition.Y) / 16);
				Color lightColor2 = Lighting.GetColor((int)(posX + TotalMovedPosition.X) / 16, (int)(posY + grass_FurScreen.Height / 30f + TotalMovedPosition.Y) / 16);
				if (x == 0)
				{
					bars.Add(new Vector2(posX, posY), Color.Transparent, new Vector3(x / 30f, y / 30f, 0));
					bars.Add(new Vector2(posX, posY + grass_FurScreen.Height / 30f), Color.Transparent, new Vector3(x / 30f, (y + 1) / 30f, 0));
				}
				bars.Add(new Vector2(posX, posY), lightColor0, new Vector3(x / 30f, y / 30f, 0));
				bars.Add(new Vector2(posX, posY + grass_FurScreen.Height / 30f), lightColor2, new Vector3(x / 30f, (y + 1) / 30f, 0));
				if (x == 29)
				{
					bars.Add(new Vector2(posX, posY), Color.Transparent, new Vector3(x / 30f, y / 30f, 0));
					bars.Add(new Vector2(posX, posY + grass_FurScreen.Height / 30f), Color.Transparent, new Vector3(x / 30f, (y + 1) / 30f, 0));
				}
			}
		}
		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = grass_FurScreen;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		sb.End();
	}
}