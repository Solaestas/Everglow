namespace Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;

internal class LanternFlameRingPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.LanternFlameRing;
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_rgb.Value);
		effect.Parameters["uTime"].SetValue((float)(-Main.timeForVisualEffects * 0.01f));
		Texture2D LanternFlameRingColor = Commons.ModAsset.Trail.Value;
		Ins.Batch.BindTexture<Vertex2D>(LanternFlameRingColor);
		Main.graphics.GraphicsDevice.Textures[1] = ModAsset.HeatMap_flameRing_lantern.Value;
		Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.AnisotropicClamp;
		Main.graphics.GraphicsDevice.Textures[2] = Commons.ModAsset.Noise_cell.Value;
		Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(LanternFlameRingPipeline), typeof(BloomPipeline))]
internal class LanternFlameRingDust : Visual
{
	public NPC OwnerLanternKing;
	public float timer;
	public float maxTime;
	public float Radius;
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public override void OnSpawn()
	{
		if (OwnerLanternKing == null)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc != null && npc.active)
				{
					if (npc.type == ModContent.NPCType<LanternGhostKing>())
					{
						OwnerLanternKing = npc;
					}
				}
			}
		}
	}
	public override void Update()
	{
		if (OwnerLanternKing == null || !OwnerLanternKing.active)
		{
			timer++;
		}
		else
		{
			timer = 0;
			LanternGhostKing lanternGhostKing = OwnerLanternKing.ModNPC as LanternGhostKing;
			if (lanternGhostKing != null)
			{
				Radius = lanternGhostKing.RingRadius;
			}
		}
		if (timer >= maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		if (OwnerLanternKing == null)
		{
			return;
		}
		Vector2 center = OwnerLanternKing.Center;
		LanternGhostKing lanternGhostKing = OwnerLanternKing.ModNPC as LanternGhostKing;
		if (lanternGhostKing != null)
		{
			center = lanternGhostKing.RingCenter;
		}
		float width = 150;
		var color = new Color(1f, 1f, 1f, 0);
		float mulColor = 1f;
		if(Radius < 1000)
		{
			width *= MathF.Max(0, (Radius - 500) / 500f);
		}
		if(timer > 120)
		{
			mulColor *= (240 - timer) / 120f;
		}
		color *= mulColor;
		float timeValue = (float)(-Main.timeForVisualEffects * 0.001f);
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i <= 500; i++)
		{
			Vector2 pos = center + new Vector2(Radius, 0).RotatedBy(i / 500f * MathHelper.TwoPi);
			Vector2 outerPos = center + new Vector2(Radius + width, 0).RotatedBy(i / 500f * MathHelper.TwoPi);
			bars.Add(pos, color, new Vector3(i / 100f, 0.5f, 0 + timeValue));
			bars.Add(outerPos, new Color(0f, 0f, 0f, 0), new Vector3(i / 100f, 1f, 0.05f + timeValue));
			if(i % 5 == 0)
			{
				Lighting.AddLight(pos, new Vector3(1f, 0.5f, 0) * width / 200f * mulColor);
			}
		}
		for (int i = 0; i <= 500; i++)
		{
			Vector2 pos = center + new Vector2(Radius, 0).RotatedBy(i / 500f * MathHelper.TwoPi);
			Vector2 innerrPos = center + new Vector2(Radius - width * 0.3f, 0).RotatedBy(i / 500f * MathHelper.TwoPi);
			bars.Add(pos, color, new Vector3(i / 100f + 0.3f + timeValue * 2, 0.5f, 0 + timeValue));
			bars.Add(innerrPos, new Color(0f, 0f, 0f, 0), new Vector3(i / 100f + 0.36f + timeValue * 2, 1f, 0.05f + timeValue));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}