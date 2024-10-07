using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;
using Everglow.Commons.Vertex;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Commons.VFX.Pipelines;

public class WarpPipeline : Pipeline
{
	private RenderTarget2D warpScreen;
	private RenderTarget2D warpScreenSwap;

	public override void Load()
	{
		Ins.MainThread.AddTask(() =>
		{
			AllocateRenderTarget(new Vector2(Main.screenWidth, Main.screenHeight));
		});
		Ins.HookManager.AddHook(CodeLayer.ResolutionChanged, (Vector2 size) =>
		{
			warpScreen?.Dispose();
			warpScreenSwap?.Dispose();
			AllocateRenderTarget(size);
		}, "Realloc RenderTarget");
		effect = ModAsset.ScreenVFXWarp;
	}

	private void AllocateRenderTarget(Vector2 size)
	{
		var gd = Main.instance.GraphicsDevice;
		warpScreen = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
		warpScreenSwap = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
	}

	public override void BeginRender()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		graphicsDevice.SetRenderTarget(warpScreenSwap);
		//保存原屏幕
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default,
			RasterizerState.CullNone);
		spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
		spriteBatch.End();
		graphicsDevice.SetRenderTarget(warpScreen);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone);

		Ins.Batch.BindTexture<Vertex2D>(ModAsset.Noise_rgb.Value);
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		Effect effect0 = VFXManager.DefaultEffect.Value;

		effect0.Parameters["uTransform"].SetValue(
	Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
	Main.GameViewMatrix.TransformationMatrix *
	Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		effect0.CurrentTechnique.Passes[0].Apply();
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
		var spriteBatch = Main.spriteBatch;
		spriteBatch.End();
		Ins.Batch.End();
		var graphicsDevice = Main.graphics.GraphicsDevice;
		graphicsDevice.SetRenderTarget(Main.screenTarget);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone);
		Effect ef = effect.Value;
		ef.Parameters["strength"].SetValue(0.02f);
		ef.CurrentTechnique.Passes["warp"].Apply();
		Main.graphics.GraphicsDevice.Textures[0] = warpScreenSwap;
		Main.graphics.GraphicsDevice.Textures[1] = warpScreen;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
		Color c0 = Color.White;

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(Vector2.zeroVector, c0, new Vector3(0, 0, 0)),
			new Vertex2D(new Vector2(0, Main.screenHeight), c0, new Vector3(0, 1, 0)),

			new Vertex2D(new Vector2(Main.screenWidth, 0), c0, new Vector3(1, 0, 0)),
			new Vertex2D(new Vector2(Main.screenWidth, Main.screenHeight), c0, new Vector3(1, 1, 0))
		};
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		spriteBatch.End();
	}
}