using static Everglow.Commons.VFX.Pipelines.ScreenReflectionPipeline;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.KingJellyBall;

[Pipeline(typeof(ScreenReflectionPipeline))]
public class KingJellyBall_Reflection : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public NPC MyKingJellyBallOwner;

	public Vector2 Position;

	public float Rotation;

	public float Scale;

	public override void OnSpawn()
	{
	}

	public override void Update()
	{
		if (MyKingJellyBallOwner == null || !MyKingJellyBallOwner.active || MyKingJellyBallOwner.type != ModContent.NPCType<KingJellyBall>() || MyKingJellyBallOwner.life <= 0 || Ins.VisualQuality.Low)
		{
			Active = false;
			return;
		}
		KingJellyBall kingJellyBall = MyKingJellyBallOwner.ModNPC as KingJellyBall;
		if (kingJellyBall == null)
		{
			Active = false;
			return;
		}
		Position = MyKingJellyBallOwner.Center;
		Rotation = MyKingJellyBallOwner.rotation;
		Scale = MyKingJellyBallOwner.scale;
		kingJellyBall.NoReflectionTime = 0;
	}

	public override void Draw()
	{
		if (MyKingJellyBallOwner == null || !MyKingJellyBallOwner.active || MyKingJellyBallOwner.type != ModContent.NPCType<KingJellyBall>() || MyKingJellyBallOwner.life <= 0 || Ins.VisualQuality.Low)
		{
			Active = false;
			return;
		}
		KingJellyBall kingJellyBall = MyKingJellyBallOwner.ModNPC as KingJellyBall;
		if (kingJellyBall == null)
		{
			Active = false;
			return;
		}

		var textureOld = Main.graphics.GraphicsDevice.Textures[1];
		Main.graphics.GraphicsDevice.Textures[1] = ModAsset.KingJellyBall_Reflect.Value;
		float timeValue = (float)Main.time * 0.03f;
		List<MirrorFaceVertex> jellyBallBodyInner = new List<MirrorFaceVertex>();

		// adjust center base on the polar funtion graph.
		Vector2 offset = new Vector2(0, -60) + new Vector2(0, -120 * (Scale - 0.3f));
		Vector2 offsetedCenter = Position + offset;
		int step = 150;
		for (int theta = 0; theta <= step; theta++)
		{
			float a = 200;
			float b = 110 + 10 * MathF.Sin(timeValue);
			float angle = theta / (float)step * MathHelper.TwoPi;
			float r = a - b * MathF.Sin(angle);

			// noise wave
			for (int k = 0; k < 6; k++)
			{
				r += MathF.Sin((theta / (float)step * MathHelper.TwoPi + MathF.Sin(timeValue * MathF.Pow(2, k * 0.2f)) * 0.22f) * 8 * MathF.Pow(2, k)) / MathF.Pow(2f, k) * 1;
				r += MathF.Sin((theta / (float)step * MathHelper.TwoPi - timeValue * 0.33f) * 8 * MathF.Pow(2, k)) / MathF.Pow(2f, k) * 2;
				r += MathF.Sin((theta / (float)step * MathHelper.TwoPi + timeValue * 0.65f) * 8 * MathF.Pow(2, k)) / MathF.Pow(2f, k) * 1;
			}
			r *= Scale;
			Vector2 toDistance = new Vector2(-r, 0).RotatedBy(angle);
			toDistance.Y *= 1.3f;
			Vector2 width = Vector2.Normalize(toDistance) * 2f;
			jellyBallBodyInner.Add(new MirrorFaceVertex(new Vector3(offsetedCenter, -150), new Color(0.4f, 0.6f, 1f, 0.5f), new Vector3(0.5f, 0.5f, 0)));
			jellyBallBodyInner.Add(new MirrorFaceVertex(new Vector3(offsetedCenter + toDistance - width, -10), new Color(0.7f, 0.7f, 1f, 1f), new Vector3(new Vector2(0.5f) + new Vector2(-0.27f, 0).RotatedBy(angle), 0)));
		}
		if (jellyBallBodyInner.Count >= 2)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, jellyBallBodyInner.ToArray(), 0, jellyBallBodyInner.Count - 2);
		}
		else
		{
			jellyBallBodyInner.Add(new MirrorFaceVertex(new Vector3(0), new Color(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0)));
			jellyBallBodyInner.Add(new MirrorFaceVertex(new Vector3(0), new Color(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0)));
			jellyBallBodyInner.Add(new MirrorFaceVertex(new Vector3(0), new Color(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0)));
			jellyBallBodyInner.Add(new MirrorFaceVertex(new Vector3(0), new Color(0.5f, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0)));

			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, jellyBallBodyInner.ToArray(), 0, jellyBallBodyInner.Count - 2);
		}
		Main.graphics.GraphicsDevice.Textures[1] = textureOld;
	}
}