namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class RockSmog_ConePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.RockSmog_Cone;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_perlin.Value);
		effect.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_rockSmog.Value);
		Texture2D halo = Commons.ModAsset.Point.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}

[Pipeline(typeof(RockSmog_ConePipeline))]
public class RockSmog_ConeDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public Queue<Vector2> oldPos = new Queue<Vector2>();

	public override void Update()
	{
		oldPos.Enqueue(position);
		if (oldPos.Count > 60)
		{
			oldPos.Dequeue();
		}
		position += velocity;
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		velocity *= 0.97f;
		velocity.Y += 0.3f;
		if (scale < 60)
		{
			scale += 0.4f;
		}
		timer++;
		if (Collision.SolidCollision(position, 0, 0))
		{
			timer += 5;
		}
		if (timer > maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		float pocession = timer / maxTime;
		float timeValue = (float)(Main.time * 0.002);
		Vector3 lightValue = Lighting.GetColor(position.ToTileCoordinates()).ToVector3();
		float light = lightValue.Length();
		List<Vertex2D> bars = new List<Vertex2D>();
		Vector2[] oldPoses = oldPos.ToArray();

		if(oldPoses.Length <= 2)
		{
			bars.Add(position, new Color(0, light, pocession), new Vector3(ai[0], timeValue, light));
			bars.Add(position, new Color(0, light, pocession), new Vector3(ai[0], timeValue, light));
			bars.Add(position, new Color(0, light, pocession), new Vector3(ai[0], timeValue, light));
			bars.Add(position, new Color(0, light, pocession), new Vector3(ai[0], timeValue, light));
		}
		else
		{
			for (int i = 1; i < oldPos.Count; i++)
			{
				Vector2 normal = oldPoses[i] - oldPoses[i - 1];
				normal = Vector2.Normalize(normal).RotatedBy(MathHelper.PiOver2) * scale;
				float width = 1 - i / (float)(oldPos.Count - 1);
				bars.Add(oldPoses[i] + normal, new Color(0, light, pocession), new Vector3(ai[0] + i / 10f, 0, width));
				bars.Add(oldPoses[i] - normal, new Color(0, light, pocession), new Vector3(ai[0] + i / 10f, 0.8f, width));
			}
		}
		if(bars.Count > 0)
		{
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		}
	}
}