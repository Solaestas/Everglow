using Everglow.Commons.Interfaces;

namespace Everglow.Myth.TheFirefly.Gores;

/// <summary>
/// 溶解尸块Pipeline，会自动剪去Main.screenPosition
/// </summary>
public class BluishGiantGentianGorePipeline : Pipeline
{
	public override void BeginRender()
	{
		Ins.Batch.Begin();
		effect.Value.Parameters["uTransform"].SetValue(
			Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
			Main.GameViewMatrix.TransformationMatrix *
			Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		effect.Value.CurrentTechnique.Passes[0].Apply();
		effect.Value.Parameters["uDissolveColor"].SetValue(new Vector4(0.05f, 0.2f, 1f, 0));
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}

	public override void Render(IEnumerable<IVisual> visuals)
	{
		BeginRender();
		foreach (var visual in visuals)
		{
			DissolveGore dissolveGore = visual as DissolveGore;
			if (dissolveGore != null)
			{
				dissolveGore.DrawDissolvePart();
			}
		}
		EndRender();
	}

	public override void Load()
	{
		effect = ModAsset.BluishGiantGentianGore;
	}
}