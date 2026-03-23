using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;

namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(LanternGhostKing_Matrix_DissolvePipeline), typeof(BloomPipeline))]
public class LanternGhostKing_Wheel3Mark : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawBG;

	public NPC OwnerLanternGhostKing;
	public Vector2 Position;
	public float Timer;
	public float MaxTime;
	public float Scale;
	public float Rotation;
	public int ExtraUpdate;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		if (OwnerLanternGhostKing is not null)
		{
			Position = OwnerLanternGhostKing.Center;
			if (OwnerLanternGhostKing.type == ModContent.NPCType<LanternGhostKing>())
			{
				LanternGhostKing lGK = OwnerLanternGhostKing.ModNPC as LanternGhostKing;
				Rotation = lGK.RushDirectionPridiction - MathHelper.PiOver2;
			}
		}
		else
		{
			Rotation = (Position - Main.LocalPlayer.Center).ToRotation() + MathHelper.PiOver2;
		}
		Timer += ExtraUpdate;
		if (Timer > MaxTime)
		{
			Active = false;
		}
		float growThrethod = 60;
		if (Timer < growThrethod)
		{
			Scale += MathF.Cos(Timer / 90f * MathHelper.Pi) * 0.05f * ExtraUpdate / 6f;
		}
		else
		{
			Scale += MathF.Cos(Timer / 300f * MathHelper.Pi) * 0.001f;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.LanternGhostKing_Wheel3Mark_1.Value;
		float fade = 1f;
		float timeValue = MaxTime - Timer;
		float fadeThrethod = 60;
		if (timeValue < fadeThrethod)
		{
			fade *= timeValue / fadeThrethod;
		}
		if (Timer < fadeThrethod)
		{
			fade *= Timer / fadeThrethod;
		}

		Color powerColor = new Color(0.5f, 0.05f, 0.1f, 0f);
		Color ringColor = new Color(1f, 0f, 0f, 0f);
		int count = 80;
		float matrixRot = 0f;
		float zCoord = 1 - fade;
		if (Timer <= 60)
		{
			zCoord = 0;
		}
		var bars = new List<Vertex2D>();
		for (int i = 0; i <= count; i++)
		{
			float rot = i / (float)count * MathHelper.TwoPi + matrixRot;
			Vector2 drawPos0 = Position + new Vector2(0, 300).RotatedBy(rot) * Scale;
			Vector2 drawPos1 = Position + new Vector2(0, 100).RotatedBy(rot) * Scale;
			Vector2 drawPos2 = Position + new Vector2(0, 230).RotatedBy(rot) * Scale;

			bars.Add(drawPos0, powerColor * fade, new Vector3(i * 8f / count, 0, zCoord));
			bars.Add(drawPos1, ringColor * fade, new Vector3(i * 8f / count, 1, zCoord));
			Lighting.AddLight(drawPos2, new Vector3(1f, 0.01f, 0.05f) * fade * 0.5f);
		}
		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);

		ringColor *= 0.6f;
		powerColor *= 0.6f;
		tex = ModAsset.LanternGhostKing_Wheel3Mark_0.Value;
		bars = new List<Vertex2D>();
		for (int i = 0; i <= count; i++)
		{
			float rot = i / (float)count * MathHelper.TwoPi + matrixRot + (float)Main.timeForVisualEffects * 0.003f;
			Vector2 drawPos0 = Position + new Vector2(0, 200).RotatedBy(rot) * Scale;
			Vector2 drawPos1 = Position + new Vector2(0, 80).RotatedBy(rot) * Scale;
			bars.Add(drawPos0, powerColor * fade, new Vector3(i * 4f / count, 0, zCoord));
			bars.Add(drawPos1, ringColor * fade, new Vector3(i * 4f / count, 1, zCoord));
		}
		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);

		bars = new List<Vertex2D>();
		for (int i = 0; i <= count; i++)
		{
			float rot = i / (float)count * MathHelper.TwoPi + matrixRot - (float)Main.timeForVisualEffects * 0.003f;
			Vector2 drawPos0 = Position + new Vector2(0, 180).RotatedBy(rot) * Scale;
			Vector2 drawPos1 = Position + new Vector2(0, 60).RotatedBy(rot) * Scale;
			bars.Add(drawPos0, powerColor * fade * 0.8f, new Vector3(i * 4f / count, 0, zCoord));
			bars.Add(drawPos1, ringColor * fade * 0.8f, new Vector3(i * 4f / count, 1, zCoord));
		}
		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);

		Texture2D star = Commons.ModAsset.StarSlashGray.Value;
		float starScale = fade * Scale * 3;
		Ins.Batch.Draw(star, Position, null, powerColor * fade, 0, star.Size() * 0.5f, starScale, SpriteEffects.None);
		Ins.Batch.Draw(star, Position, null, powerColor * fade, MathHelper.PiOver2, star.Size() * 0.5f, starScale, SpriteEffects.None);

		Ins.Batch.Draw(star, Position, null, powerColor * fade, -MathHelper.PiOver4, star.Size() * 0.5f, starScale * 0.66f, SpriteEffects.None);
		Ins.Batch.Draw(star, Position, null, powerColor * fade, MathHelper.PiOver4, star.Size() * 0.5f, starScale * 0.66f, SpriteEffects.None);

		Texture2D spot = Commons.ModAsset.LightPoint2.Value;
		Ins.Batch.Draw(spot, Position, null, new Color(1f, 1f, 1f, 0), 0, spot.Size() * 0.5f, starScale * 0.75f, SpriteEffects.None);
	}
}