using Everglow.Common.VFX;

namespace Everglow.Core.VFX.Pipelines
{
	/// <summary>
	/// 世界坐标系Pipeline，会自动剪去Main.screenPosition
	/// </summary>
	internal class WCSPipeline : Pipeline
	{
		public override void BeginRender()
		{
			VFXManager.spriteBatch.Begin();
			effect.Value.Parameters["uTransform"].SetValue(
				Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
				Main.GameViewMatrix.ZoomMatrix *
				Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1)
				);
			effect.Value.CurrentTechnique.Passes[0].Apply();
		}

		public override void EndRender()
		{
			VFXManager.spriteBatch.End();
		}

		public override void Load()
		{
			effect = VFXManager.DefaultEffect;
		}
	}
}