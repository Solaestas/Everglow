using ReLogic.Content;

namespace Everglow.Myth.Bosses.Acytaea;


internal class NPPipeline : Pipeline
{
	public override void BeginRender()
	{
		VFXManager.spriteBatch.Begin(BlendState.Additive);
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
		effect = ModContent.Request<Effect>("Everglow/Sources/Commons/Core/VFX/Effect/Shader2D");
	}
}

internal class AcytaeaPipeline : PostPipeline
{
	private Asset<Texture2D> texture;
	public override void Render(RenderTarget2D rt2D)
	{
		VFXManager.spriteBatch.Begin();
		var effect = this.effect.Value;
		Main.instance.GraphicsDevice.Textures[1] = texture.Value;
		//TODO 常量待优化，目前测试用
		effect.Parameters["m"].SetValue(0.62f);
		effect.Parameters["n"].SetValue(0.01f);
		effect.CurrentTechnique.Passes[0].Apply();
		VFXManager.spriteBatch.Draw(rt2D, Vector2.Zero, Color.White);
		VFXManager.spriteBatch.End();
	}

	public override void Load()
	{
		texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Cosmic");
		effect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/BigTentacle");

	}
}