using Everglow.Commons.Enums;
using Everglow.Commons.Graphics;
using Everglow.Commons.Vertex;
using SteelSeries.GameSense;

namespace Everglow.Commons.VFX.CommonVFXDusts;

//ʹ������
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
		//Main.graphics.graphicsDevice.Textures[0] = ModAsset.fl;
		Ins.Batch.BindTexture(ModAsset.Flare_Tex.Value);
		Main.graphics.graphicsDevice.Textures[1] = ModAsset.Noise_perlin.Value;

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
		Ins.Batch.Draw(drawPos-Main.screenPosition, null, c, rotation, new Vector2(64), scale, 0);
	}
}