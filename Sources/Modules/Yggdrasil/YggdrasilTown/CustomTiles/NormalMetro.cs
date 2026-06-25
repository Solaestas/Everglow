using Everglow.Commons.CustomTiles.Abstracts;
using Everglow.Commons.CustomTiles.Core;
using Everglow.Yggdrasil.KelpCurtain.CustomTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.CustomTiles;

public class NormalMetro : BoxEntity
{
	public BlackAwningBoat_ControlUI LocalUIHelper;

	public override void SetDefaults()
	{
		Size = new Vector2(1696, 16);
		LocalUIHelper = null;
	}

	public override Color MapColor => new Color(128, 131, 142);

	public int Direction = 1;

	public override void AI()
	{
		Velocity *= 0;
		Position += Velocity;
		Velocity = new Vector2(Direction * 0, 0);
		if(Main.mouseLeft && Main.mouseLeftRelease)
		{
			Direction *= -1;
		}
		if (Main.mouseMiddle && Main.mouseMiddleRelease)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D metro = ModAsset.NormalMetro.Value;
		var frame = new Rectangle(108, 0, 848, 146);
		var glow_frame = new Rectangle(108, 292, 848, 146);
		var pos0 = new Vector2((Box.Center.X + Box.Left) * 0.5f, Box.Bottom);
		Main.spriteBatch.Draw(metro, pos0 - Main.screenPosition, frame, Lighting.GetColor(pos0.ToTileCoordinates()), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos0 - Main.screenPosition, glow_frame,  new Color(1f, 1f, 1f, 0), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);

		var pos1 = new Vector2((Box.Center.X + Box.Right) * 0.5f, Box.Bottom);
		Main.spriteBatch.Draw(metro, pos1 - Main.screenPosition, frame, Lighting.GetColor(pos1.ToTileCoordinates()), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos1 - Main.screenPosition, glow_frame, new Color(1f, 1f, 1f, 0), 0, new Vector2(frame.Width / 2f, frame.Height), 1, SpriteEffects.None, 0);

		var headFrame = new Rectangle(0, 0, 106, 146);
		var tailFrame = new Rectangle(958, 0, 106, 146);

		var pos2 = new Vector2(Box.Left, Box.Bottom - 73);
		var pos3 = new Vector2(Box.Right, Box.Bottom - 73);
		Main.spriteBatch.Draw(metro, pos2 - Main.screenPosition, headFrame, Lighting.GetColor(pos2.ToTileCoordinates()), 0, new Vector2(headFrame.Width, headFrame.Height * 0.5f), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos3 - Main.screenPosition, tailFrame, Lighting.GetColor(pos2.ToTileCoordinates()), 0, new Vector2(0, tailFrame.Height * 0.5f), 1, SpriteEffects.None, 0);

		headFrame.Y += 292;
		tailFrame.Y += 292;
		Main.spriteBatch.Draw(metro, pos2 - Main.screenPosition, headFrame, new Color(1f, 1f, 1f, 0) * 0.36f, 0, new Vector2(headFrame.Width, headFrame.Height * 0.5f), 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(metro, pos3 - Main.screenPosition, tailFrame, new Color(1f, 1f, 1f, 0) * 0.36f, 0, new Vector2(0, tailFrame.Height * 0.5f), 1, SpriteEffects.None, 0);
	}

	public void RightClick()
	{
	}

	public override Vector2 StandAccelerate(IBox obj)
	{
		return base.StandAccelerate(obj);
	}
}