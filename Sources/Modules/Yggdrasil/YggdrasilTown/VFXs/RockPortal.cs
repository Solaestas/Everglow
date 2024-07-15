namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;
public class RockPortalPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.RockPortal;
		effect.Value.Parameters["uNoise"].SetValue(ModAsset.RockPortal1.Value);
		effect.Value.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_RockPortal.Value);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Point.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(RockPortalPipeline))]
public class RockPortal : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public RockPortal() { }
	public override void Update()
	{
		if (scale < 160)
		{
			scale += 2f;
		}
		timer++;
		if (timer > maxTime)
			Active = false;
	}
	public override void Draw()
	{
		float pocession = timer / maxTime;
		float timeValue = (float)(Main.time * 0.001);
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Vector3 lightValue = Lighting.GetColor(position.ToTileCoordinates()).ToVector3();
		float light = (lightValue.Length());
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner,new Color(0, 0,pocession), new Vector3(0,timeValue,light)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5)*0.5f,new Color(0, 1, pocession), new Vector3(0, timeValue + 0.2f, light)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5)*0.5f,new Color(1, 0 ,pocession), new Vector3(1, timeValue, light)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1),new Color(1, 1, pocession), new Vector3(1, timeValue + 0.2f, light))
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
