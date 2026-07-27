namespace Everglow.Myth.LanternMoon.Gores;

[Pipeline(typeof(WCSPipeline))]
public class NormalGore : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Texture2D Texture;
	public Vector2 Position;
	public Vector2 Velocity;
	public float RotateSpeed;
	public float Rotation;
	public float Scale;
	public float Timer;
	public float MaxTime;

	public int SideSize()
	{
		if (Texture is not null)
		{
			return (int)MathF.Sqrt(Texture.Size().X * Texture.Size().Y);
		}
		return 1;
	}

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Velocity *= 0.99f;
		RotateSpeed *= 0.98f;
		Rotation += RotateSpeed;
		int size = SideSize();
		Vector2 checkNextX = Position - new Vector2(size) * 0.5f + new Vector2(Velocity.X, 0);
		Vector2 checkNextY = Position - new Vector2(size) * 0.5f + new Vector2(0, Velocity.Y);
		bool collide = false;
		if (Collision.SolidCollision(checkNextX, size, size))
		{
			collide = true;
			Velocity.X *= -Main.rand.NextFloat(0.2f, 0.99f);
			RotateSpeed += Main.rand.NextFloat(-0.03f, 0.03f) * Velocity.Length();
		}
		if (Collision.SolidCollision(checkNextY, size, size))
		{
			collide = true;
			Velocity.Y *= -Main.rand.NextFloat(0.2f, 0.99f);
			RotateSpeed += Main.rand.NextFloat(-0.03f, 0.03f) * Velocity.Length();
			if (Velocity.Length() < 1f)
			{
				Velocity *= 0;
			}
		}
		if(!collide)
		{
			Velocity += new Vector2(0, 0.25f);
		}
	}

	public override void Draw()
	{
		float fade = 1f;
		float timeValue = MaxTime - Timer;
		if(timeValue < 120)
		{
			fade *= timeValue / 120f;
		}
		Ins.Batch.Draw(Texture, Position, null, Lighting.GetColor(Position.ToTileCoordinates()) *fade, Rotation, Texture.Size() * 0.5f, Scale, SpriteEffects.None);

		// Ins.Batch.Draw(tex, Position, null, DrawColor, Rotation + MathHelper.PiOver2, tex.Size() * 0.5f, new Vector2(Fade, 2) * Scale, SpriteEffects.None);
	}
}