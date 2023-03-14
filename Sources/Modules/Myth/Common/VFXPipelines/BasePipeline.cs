namespace Everglow.Myth.Common.VFXPipelines;

internal class BasePipeline : Pipeline
{
	public override void BeginRender()
	{
		Ins.Batch.Begin();
		effect.Value.Parameters["uTransform"].SetValue(Main.Transform *
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
