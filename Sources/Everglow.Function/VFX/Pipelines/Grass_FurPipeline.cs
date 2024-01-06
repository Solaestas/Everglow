using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;

namespace Everglow.Commons.VFX.Pipelines;

/// <summary>
/// 草皮Pipeline,合批到RenderTarget2D上进行脏绘制,技术和算力允许的条件下尝试用势能图使草皮弯曲,会自动剪去Main.screenPosition
/// </summary>
public class Grass_FurPipeline : Pipeline
{
	private RenderTarget2D grass_FurScreen;
	private RenderTarget2D grass_FurScreenSwap;
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
		var sb = Ins.Batch;
		var gd = Main.instance.GraphicsDevice;

		sb.Begin(BlendState.AlphaBlend, DepthStencilState.Default, SamplerState.PointClamp, RasterizerState.CullNone);


		gd.SetRenderTarget(grass_FurScreen);
		Ins.Batch.Begin();
		effect.Value.Parameters["uTransform"].SetValue(
			Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.TransformationMatrix *
			Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		effect.Value.CurrentTechnique.Passes[0].Apply();
	}
	public override void Render(IEnumerable<IVisual> visuals)
	{
		BeginRender();
		foreach (var visual in visuals)
		{
			visual.Draw();
		}
		EndRender();
	}
	public override void EndRender()
	{
		var sb = Ins.Batch;
		var gd = Main.instance.GraphicsDevice;
		sb.End();

		var cur = Ins.VFXManager.CurrentRenderTarget;
		Ins.VFXManager.SwapRenderTarget();
		gd.SetRenderTarget(Ins.VFXManager.CurrentRenderTarget);
		sb.Begin(BlendState.AlphaBlend, DepthStencilState.Default, SamplerState.PointClamp, RasterizerState.CullNone);
		sb.Draw(cur, Vector2.Zero, Color.White);

		gd.BlendState = BlendState.AlphaBlend;
		sb.Draw(grass_FurScreen, Vector2.Zero, new Color(255, 255, 255, 0));
		sb.End();
	}
}