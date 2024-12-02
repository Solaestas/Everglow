using Everglow.Commons.Enums;
using Everglow.Commons.Graphics;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;
using SteelSeries.GameSense;

namespace Everglow.Commons.VFX.CommonVFXDusts;

//Ê¹ÓÃÀý£º
/*
 * GradientColor flareColor = new GradientColor();
        flareColor.colorList.Add((new Color(1f, 0.5f, 0f), 0f));
        flareColor.colorList.Add((new Color(0.00f, 0.03f, 1f), 0.8f));

        if (time % 2 == 0)
		{
			var flare = new Flare();
            flare.color = flareColor;
			flare.scale = 0.6f;
			flare.gravity = -0.16f;
			flare.velocity = Main.rand.NextVector2Circular(1,1);
			flare.velocity.Y -= 1;
			flare.maxTimeleft = 45f;
			flare.timeleft = 45f;
			flare.Owner = Main.LocalPlayer;
			Ins.VFXManager.Add(flare);
		}
 * */
public class FlarePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.Flare;
		
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		Ins.Batch.Begin(BlendState.Additive, DepthStencilState.None, SamplerState.AnisotropicClamp, RasterizerState.CullNone);
		Main.graphics.graphicsDevice.Textures[1] = ModAsset.Noise_perlin.Value;
        effect.Parameters["uTransform"].SetValue(
            Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) *
            Main.GameViewMatrix.TransformationMatrix *
            Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
        effect.CurrentTechnique.Passes[0].Apply();
    }

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(FlarePipeline))]
public class Flare : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public Vector2 velocity;
	public float gravity = -0.2f;
	public float timeleft;
	public float maxTimeleft;
	public float scale;
	public GradientColor color;
	float rotation;
	public Entity Owner;

	public float speedLimits = 1;
	public Flare() {
        
    }
	public override void OnSpawn()
	{
        rotation = Main.rand.NextFloat(6.28f);
    }
	public override void Update()
	{
		position += velocity;
		velocity.Y += gravity;
		velocity *= speedLimits;
        //scale *= 0.99f;
       
        timeleft--;
		if (timeleft<=0)
			Active = false;
		if (Collision.SolidCollision(position, 10, 10))
		{
			velocity.Y *= 0.6f;
		}
	}

	public override void Draw()
	{
		Color c = color.GetColor(1 - timeleft / maxTimeleft);
		c.A = (byte)((1 - timeleft / maxTimeleft) * 255);
		Vector2 drawPos = position;
		if (Owner != null)
			drawPos += Owner.Center;
		
		Ins.Batch.Draw(ModAsset.Flare_Tex.Value,drawPos, null, c, rotation, new Vector2(64), scale, 0);
	}
}