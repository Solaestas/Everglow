using ReLogic.Content;

namespace Everglow.Sources.Modules.MythModule.Common.VFXPipelines
{
	internal class ScreenReflectPipeline : Pipeline
	{
		public override void Load()
		{
			effect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ScreenReflect", AssetRequestMode.ImmediateLoad);
		}
		public override void BeginRender()
		{
			var effect = this.effect.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.Parameters["tex1"].SetValue(Main.screenTarget);
			effect.Parameters["uScreenWidth"].SetValue(Main.screenWidth);
			effect.Parameters["uScreenHeight"].SetValue(Main.screenHeight);
			VFXManager.spriteBatch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
			effect.CurrentTechnique.Passes[0].Apply();
		}

		public override void EndRender()
		{
			VFXManager.spriteBatch.End();
		}
	}
}
