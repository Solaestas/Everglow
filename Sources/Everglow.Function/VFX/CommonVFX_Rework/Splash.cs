﻿using Everglow.Commons.Enums;
using Everglow.Commons.Graphics;
using Everglow.Commons.Interfaces;
using Everglow.Commons.VFX.Pipelines;

namespace Everglow.Commons.VFX.CommonVFXDusts;

public class SplashPipeline : Pipeline
{
	public override void BeginRender()
	{

	}
	public override void EndRender()
	{


	}

	public override void Load()
	{
		effect = ModAsset.CommonDissolve;

	}
	public override void Render(IEnumerable<IVisual> visuals)
	{
		foreach (Visual v in visuals)
		{
			var effect = this.effect.Value;
			Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicClamp, RasterizerState.CullNone);
			Main.graphics.graphicsDevice.Textures[1] = ModAsset.Noise_cell.Value;

			effect.Parameters["uvMulti"].SetValue(0.2f);

			effect.Parameters["uTransform"].SetValue(
				Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
				Main.GameViewMatrix.TransformationMatrix *
				Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));

			if (v is Splash s)
			{
				effect.Parameters["uDissolve"].SetValue((float)Math.Pow(1 - s.timeleft / s.maxTimeleft, 2f));
			}

			effect.CurrentTechnique.Passes[0].Apply();

			v.Draw();

			Ins.Batch.End();
		}
	}

}
[Pipeline(typeof(SplashPipeline),typeof(BloomPipeline))]
public class Splash : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float gravity = -0.2f;
	public float timeleft;
	public float maxTimeleft;
	public float scale = 1;
	public GradientColor color;
	float rotation;
	public Entity Owner;

	float maxScale;
	public float speedLimits = 1;
	public Splash()
	{

	}
	public override void OnSpawn()
	{
		rotation = Main.rand.NextFloat(6.28f);
		texType = Main.rand.Next(3);
	}
	public override void Update()
	{
		position += velocity;
		velocity.Y += gravity;
		velocity *= speedLimits;
		scale *= 0.97f;

		timeleft--;
		if (timeleft <= 0)
			Active = false;
		if (Collision.SolidCollision(position, 10, 10))
		{
			velocity.Y *= 0.6f;
		}
	}

	int texType = 0;
	public override void Draw()
	{
		Color c = color.GetColor(1 - timeleft / maxTimeleft);
		c.A = 0;
		Vector2 drawPos = position;
		if (Owner != null)
			drawPos += Owner.Center;
		Texture2D tex = texType switch
		{
			0 => ModAsset.Splash_0.Value,
			1 => ModAsset.Splash_1.Value,
			2 => ModAsset.Splash_2.Value,
			_ => ModAsset.Splash_0.Value,
		};

		Ins.Batch.Draw(tex, drawPos, null, c, rotation, tex.Size() / 2, scale, 0);
	}
}