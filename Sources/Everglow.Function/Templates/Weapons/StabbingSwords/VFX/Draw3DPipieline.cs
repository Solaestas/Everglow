using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;

public class Draw3DPipieline : Pipeline
{
	public override void BeginRender()
	{
		Ins.Batch.Begin(BlendState.AlphaBlend);
	}

	public override void EndRender()
	{
		var camPos = new Vector3(Main.screenWidth / 2f + Main.screenPosition.X, Main.screenHeight / 2f + Main.screenPosition.Y, 0);
		var matrix = Matrix.CreateLookAt(camPos, new Vector3(camPos.X, camPos.Y, 1), Vector3.Down);

		var projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
			MathHelper.Pi / 2f,
			Main.graphics.GraphicsDevice.Viewport.AspectRatio,
			1,
			2000);
		projectionMatrix.M22 *= Math.Sign(Main.GameViewMatrix.TransformationMatrix.M22);

		matrix *= projectionMatrix;

		effect.Value.Parameters["uTransform"].SetValue(matrix);
		effect.Value.CurrentTechnique.Passes[0].Apply();

		Ins.Batch.End();
	}

	public override void Load()
	{
		effect = ModAsset.DrawPrim3D;
		Ins.Batch.RegisterVertex<Vertex3D_2>();
	}
}