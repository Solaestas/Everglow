using static Everglow.Yggdrasil.CorruptWormHive.Projectiles.TrueDeathSickle.TrueDeathSickle_Blade;

namespace Everglow.Yggdrasil.CorruptWormHive.VFXs;

[Pipeline(typeof(DevilFlamePipeline), typeof(BloomPipeline))]
internal class DevilFlame3DSickle_worldCoordDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector3 position3D;
	public Vector3 velocity3D;
	public Vector3 rotateAxis;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public Queue<Vector3> trails = new Queue<Vector3>();
	public int ownerWhoAmI = -1;

	public override void OnSpawn()
	{
		trails.Enqueue(position3D - velocity3D);
		base.OnSpawn();
	}

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		if (ownerWhoAmI == -1)
		{
			Active = false;
			return;
		}
		trails.Enqueue(position3D);
		if (trails.Count > 40)
		{
			trails.Dequeue();
		}
		if(maxTime - timer < 22)
		{
			ai[2] = MathHelper.Lerp(ai[2], 1f, 0.18f);
		}
		position3D += velocity3D;
		velocity3D *= 0.96f;
		velocity3D.Y -= 3 * MathF.Abs(ai[1]);
		velocity3D = RodriguesRotate(velocity3D, rotateAxis, ai[1]);
		float delC = 1f * (float)Math.Sin((maxTime - timer) / 40d * Math.PI);
		Lighting.AddLight(new Vector2(position3D.X, position3D.Y), 0.25f * delC, 0f, 0.95f * delC);
	}

	public override void Draw()
	{
		if (ownerWhoAmI == -1)
		{
			Active = false;
			return;
		}
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 1; i < trails.Count; i++)
		{
			Vector3 pos3D = trails.ToArray()[i];
			Vector3 pos3DOld = trails.ToArray()[i - 1];
			float width = (i - 1) / (float)Math.Max(trails.Count - 2f, 1);
			width = MathF.Sin(width * MathF.PI) * scale;
			if(trails.Count <= 4)
			{
				width = 0f;
			}
			float size;
			Vector2 posOld = Projection2D(pos3DOld, Main.screenPosition, 500, out size);
			Vector2 pos = Projection2D(pos3D, Main.screenPosition, 500, out size);
			Vector2 normal = Utils.SafeNormalize(pos - posOld, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2);
			normal *= width * size;
			float timeValue = -(float)Main.timeForVisualEffects * 0.0015f;
			float fx = timer / maxTime;
			var drawcRope = new Color(fx * fx * fx * 2, 0.5f, 1, Math.Clamp(1 - fx, 0, 0.6f));
			bars.Add(pos - normal, drawcRope, new Vector3(timeValue + i / 60f, ai[0], ai[2]));
			bars.Add(pos + normal, drawcRope, new Vector3(timeValue + i / 60f, ai[0] + 0.3f, ai[2]));
		}
		if (bars.Count <= 2)
		{
			float size;
			Vector2 pos = Projection2D(position3D, Main.screenPosition, 500, out size);
			bars.Add(pos, Color.White, new Vector3(0));
			bars.Add(pos, Color.White, new Vector3(0));
			bars.Add(pos, Color.White, new Vector3(0));
			bars.Add(pos, Color.White, new Vector3(0));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}