namespace Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;

[Pipeline(typeof(LanternFlameRingPipeline), typeof(BloomPipeline))]
public class LanternFlameRingDust : Visual
{
	public NPC OwnerLanternKing;
	public float Fade;
	public float MaxFade;
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
			Fade++;
		}
		else
		{
			Fade = 0;
			LanternGhostKing lanternGhostKing = OwnerLanternKing.ModNPC as LanternGhostKing;
			if (lanternGhostKing != null)
			{
				Radius = lanternGhostKing.RingRadius;
				Fade = lanternGhostKing.RingFade;
			}
		}
		if (Fade > MaxFade)
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
		if (Radius < 1000)
		{
			width *= MathF.Max(0, (Radius - 500) / 500f);
		}
		if (Fade > 120)
		{
			mulColor *= (240 - Fade) / 120f;
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
			if (i % 5 == 0)
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