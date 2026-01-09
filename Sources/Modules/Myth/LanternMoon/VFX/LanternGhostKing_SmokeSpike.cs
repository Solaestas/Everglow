namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class LanternGhostKing_SmokeSpike : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 StartPosition = Vector2.zeroVector;
	public Vector2 Position;
	public Vector2 Velocity;
	public float Timer;
	public float MaxTime;
	public float Scale;

	public override void Update()
	{
		Position += Velocity;
		Velocity *= 0.95f;
		if (StartPosition == Vector2.zeroVector)
		{
			StartPosition = Position;
		}
		Timer++;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.LanternGhostKing_SmokeSpike.Value;
		if (StartPosition == Vector2.zeroVector || StartPosition == Position)
		{
			StartPosition = Position;
			Ins.Batch.Draw(tex, Rectangle.Empty, Color.Transparent);
			return;
		}
		Vector2 dir = (Position - StartPosition).SafeNormalize(Vector2.UnitY);
		dir = dir.RotatedBy(MathHelper.PiOver2) * Scale;
		float fade = 1f;
		float timeValue = MaxTime - Timer;
		if (timeValue < 20)
		{
			fade *= timeValue / 20f;
		}

		var bars = new List<Vertex2D>();
		bars.Add(Position + dir, Lighting.GetColor(Position.ToTileCoordinates()) * fade, new Vector3(0, 0, 0));
		bars.Add(Position - dir, Lighting.GetColor(Position.ToTileCoordinates()) * fade, new Vector3(1, 0, 0));

		bars.Add(StartPosition + dir, Lighting.GetColor(StartPosition.ToTileCoordinates()) * fade * 0.2f, new Vector3(0, 1, 0));
		bars.Add(StartPosition - dir, Lighting.GetColor(StartPosition.ToTileCoordinates()) * fade * 0.2f, new Vector3(1, 1, 0));

		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
	}
}