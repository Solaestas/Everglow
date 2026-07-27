namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class DivineAscendHitStar : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 Position;
	public float[] ai;
	public float Timer;
	public float MaxTime;
	public float Scale;
	public float Rotation;

	public override void Update()
	{
		Timer++;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		float pocession = 1 - Timer / MaxTime;
		var drawColor = new Color(1f, 0.75f, 0.3f, 0);
		Texture2D texture = Commons.ModAsset.StarSlashGray.Value;
		Texture2D spot = Commons.ModAsset.LightPoint2.Value;
		Ins.Batch.Draw(spot, Position, null, drawColor, Rotation, spot.Size() * 0.5f, MathF.Pow(pocession, 2) * Scale * 6f, SpriteEffects.None);
		Ins.Batch.Draw(texture, Position, null, drawColor, Rotation, texture.Size() * 0.5f, new Vector2(pocession * 2, 1) * Scale, SpriteEffects.None);
		Ins.Batch.Draw(texture, Position, null, drawColor, Rotation + 0.6f, texture.Size() * 0.5f, new Vector2(pocession, 0.5f) * Scale, SpriteEffects.None);
		Ins.Batch.Draw(texture, Position, null, drawColor, Rotation - 0.6f, texture.Size() * 0.5f, new Vector2(pocession, 0.5f) * Scale, SpriteEffects.None);
		Lighting.AddLight(Position, new Vector3(1f, 0.75f, 0.3f) * pocession);
	}
}