namespace Everglow.Commons.VFX.Pipelines;

/// <summary>
/// 草皮Pipeline,合批到RenderTarget2D上进行脏绘制,技术和算力允许的条件下尝试用势能图使草皮弯曲,会自动剪去Main.screenPosition
/// </summary>
public class Grass_FurPipeline : Pipeline
{
	private RenderTarget2D grass_FurScreen;
	private RenderTarget2D grass_FurScreenSwap;
	public override void BeginRender()
	{
		Ins.Batch.Begin();
		effect.Value.Parameters["uTransform"].SetValue(
			Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.TransformationMatrix *
			Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		effect.Value.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}

	public override void Load()
	{
		effect = VFXManager.DefaultEffect;
	}
}