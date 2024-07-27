using Everglow.Commons.Interfaces;
using Everglow.Commons.VFX.Visuals;

namespace Everglow.Commons.VFX.Pipelines;

/// <summary>
/// 世界坐标系Pipeline，会自动剪去Main.screenPosition
/// </summary>
public class DissolveAndNoDissolvePipeline : Pipeline
{
	public void BeginRenderDissolve()
	{
		Ins.Batch.Begin();

		Effect ef = ModAsset.GoreDissolve.Value;
		ef.Parameters["uTransform"].SetValue(
			Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.TransformationMatrix *
			Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		ef.CurrentTechnique.Passes["Test"].Apply();
	}
	public override void BeginRender()
	{
		Ins.Batch.Begin();
		effect.Value.Parameters["uTransform"].SetValue(
			Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.TransformationMatrix *
			Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		effect.Value.CurrentTechnique.Passes[0].Apply();
	}
	public override void Render(IEnumerable<IVisual> visuals)
	{
		BeginRenderDissolve();
		foreach (var visual in visuals)
		{
			DissolveGore dissolveGore = visual as DissolveGore;
			if (dissolveGore != null)
			{
				dissolveGore.DrawDissolvePart();
			}
		}
		EndRender();

		BeginRender();
		foreach (var visual in visuals)
		{
			visual.Draw();
		}
		EndRender();
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