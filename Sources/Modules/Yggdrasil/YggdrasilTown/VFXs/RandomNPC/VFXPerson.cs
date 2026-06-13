using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs.RandomNPC;

[Pipeline(typeof(WCSPipeline))]
public class VFXPerson : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;

	public Vector2 Velocity;

	public float Timer;

	public float MaxTime;

	public int Direction;

	public int Frame;

	public Color SkinColor;

	public override void Update()
	{
		if (Main.mouseMiddle && Main.mouseMiddleRelease)
		{
			Active = false;
			return;
		}
		if (TileUtils.PlatformCollision(Position + new Vector2(-8, 0), 16, 48))
		{
			Velocity.Y *= 0;
			for (int t = 0; t < 8; t++)
			{
				if (!TileUtils.PlatformCollision(Position + new Vector2(-8, -1), 16, 48))
				{
					break;
				}
				Position.Y -= 1;
			}
		}
		else
		{
			Velocity.Y += 0.25f;
			if (Velocity.Y > 8)
			{
				Velocity.Y = 8;
			}
		}
		Position += Velocity;
		Timer++;
		if (Timer % 6 == 0)
		{
			Frame++;
			if (Frame >= 20)
			{
				Frame = 0;
			}
		}
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		SpriteEffects flip = Direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
		Texture2D head = Terraria.GameContent.TextureAssets.Players[0, 0].Value;
		Rectangle headFrame = new Rectangle(0, Frame * 56, 40, 56);
		Ins.Batch.Draw(head, Position + new Vector2(0, 50), headFrame, Lighting.GetColor(Position.ToTileCoordinates(), SkinColor), 0, new Vector2(headFrame.Width * 0.5f, headFrame.Height), 1f, flip);
		Texture2D foot = Terraria.GameContent.TextureAssets.Players[0, 10].Value;
		Rectangle footFrame = new Rectangle(0, Frame * 56, 40, 56);
		Ins.Batch.Draw(foot, Position + new Vector2(0, -4), footFrame, Lighting.GetColor(Position.ToTileCoordinates(), SkinColor), 0, new Vector2(footFrame.Width * 0.5f, 0), 1f, flip);
	}
}