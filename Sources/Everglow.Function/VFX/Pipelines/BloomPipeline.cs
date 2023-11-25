using Everglow.Commons.Enums;

namespace Everglow.Commons.VFX.Pipelines;

public class BloomPipeline : PostPipeline
{
	private RenderTarget2D[] blurScreens;
	private RenderTarget2D blurScreenSwap;
	private const int MAX_BLUR_LEVELS = 4;

	public override void Load()
	{
		blurScreens = new RenderTarget2D[MAX_BLUR_LEVELS];
		Ins.MainThread.AddTask(() =>
		{
			AllocateRenderTarget(new Vector2(Main.screenWidth, Main.screenHeight));
		});
		Ins.HookManager.AddHook(CodeLayer.ResolutionChanged, (Vector2 size) =>
		{
			for (int i = 0; i < blurScreens.Length; i++)
			{
				blurScreens[i]?.Dispose();
			}
			blurScreenSwap?.Dispose();
			AllocateRenderTarget(size);
		}, "Realloc RenderTarget");
		effect = ModAsset.Bloom;
	}

	private void AllocateRenderTarget(Vector2 blurSize)
	{
		var gd = Main.instance.GraphicsDevice;
		for (int i = 0; i < MAX_BLUR_LEVELS; i++)
		{
			blurScreens[i] = new RenderTarget2D(gd,
				(int)blurSize.X >> i, (int)blurSize.Y >> i, false,
				SurfaceFormat.Color, DepthFormat.None);
		}
		blurScreenSwap = new RenderTarget2D(gd, (int)blurSize.X >> MAX_BLUR_LEVELS, (int)blurSize.Y >> MAX_BLUR_LEVELS,
			false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
	}

	public override void Render(RenderTarget2D rt2D)
	{
		var sb = Main.spriteBatch;
		var gd = Main.instance.GraphicsDevice;
		var effect = this.effect.Value;
		Rectangle rectangle = new(0, 0, 1, 1);

		sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);

		effect.Parameters["uTransform"].SetValue(
			Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0, 1));
		effect.Parameters["uLimit"].SetValue(0.5f);
		effect.CurrentTechnique.Passes["GetLight"].Apply();

		gd.SetRenderTarget(blurScreens[0]);
		sb.Draw(rt2D, rectangle, Color.White);

		effect.Parameters["uDelta"].SetValue(1);

		// Downsampling
		for (int i = 1; i < MAX_BLUR_LEVELS; i++)
		{
			gd.SetRenderTarget(blurScreens[i]);
			effect.Parameters["uSize"].SetValue(blurScreens[i].Size());
			effect.CurrentTechnique.Passes["Blur"].Apply();
			sb.Draw(blurScreens[i - 1], rectangle, Color.White);
		}

		// GaussianBlur
		effect.Parameters["uIntensity"].SetValue(1);

		gd.SetRenderTarget(blurScreenSwap);
		gd.Clear(Color.Transparent);
		effect.Parameters["uSize"].SetValue(blurScreenSwap.Size());
		effect.CurrentTechnique.Passes["BloomH"].Apply();
		sb.Draw(blurScreens[^1], rectangle, Color.White);

		gd.SetRenderTarget(blurScreens[^1]);
		gd.Clear(Color.Transparent);
		effect.Parameters["uSize"].SetValue(blurScreens[^1].Size());
		effect.CurrentTechnique.Passes["BloomV"].Apply();
		sb.Draw(blurScreenSwap, rectangle, Color.White);

		// Upsampling
		for (int i = MAX_BLUR_LEVELS - 1; i > 0; i--)
		{
			gd.SetRenderTarget(blurScreens[i - 1]);
			effect.Parameters["uSize"].SetValue(blurScreens[i - 1].Size());
			effect.CurrentTechnique.Passes["Blur"].Apply();
			sb.Draw(blurScreens[i], rectangle, Color.White);
		}

		sb.End();

		var cur = Ins.VFXManager.CurrentRenderTarget;
		Ins.VFXManager.SwapRenderTarget();
		gd.SetRenderTarget(Ins.VFXManager.CurrentRenderTarget);
		sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
		sb.Draw(cur, Vector2.Zero, Color.White);

		gd.BlendState = BlendState.AlphaBlend;
		sb.Draw(rt2D, Vector2.Zero, Color.White);

		gd.BlendState = BlendState.Additive;
		sb.Draw(blurScreens[0], cur.Bounds, Color.White);
		sb.End();
	}
}