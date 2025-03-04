using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionBackground : UIBlock
{
	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		sb.Draw(ModAsset.Mission_MarbleBoard.Value, HitBox, Color.White);
		sb.Draw(ModAsset.Mission_MarbleBoard_background.Value, HitBox, Color.White);

		var scale = MissionContainer.Scale;
		var basePos = HitBox.TopLeft();

		// Laser Prism
		var laserPrism = ModAsset.LaserPrism.Value;
		var laserPrismPos = basePos + scale * (new Vector2(45, 203) + laserPrism.Size() / 2);
		sb.Draw(laserPrism, laserPrismPos, null, Color.White, 0, laserPrism.Size() / 2, scale, SpriteEffects.None, 0);

		// Crystal
		var crystal = ModAsset.TetragonalCrystal.Value;
		var crystalPos1 = basePos + scale * new Vector2(239, 413);
		sb.Draw(crystal, crystalPos1, null, Color.White, 0, crystal.Size() / 2, scale, SpriteEffects.None, 0);
		var crystalPos2 = basePos + scale * new Vector2(618, 52);
		sb.Draw(crystal, crystalPos2, null, Color.White, 0, crystal.Size() / 2, scale, SpriteEffects.None, 0);

		// Glass Brick
		var glassBrick = ModAsset.GlassBrick.Value;
		var glassBrickPos = basePos + scale * (new Vector2(235, 203) + glassBrick.Size() / 2);
		sb.Draw(glassBrick, glassBrickPos, null, Color.White, 0, glassBrick.Size() / 2, scale, SpriteEffects.None, 0);

		// Reflect Chain
		var chain = ModAsset.MirrorChain.Value;
		var chainCenter = basePos + scale * new Vector2(270, 210);
		var chainRotation = MathHelper.PiOver4 * 3 / 2;
		for (int i = -2; i <= 2; i++)
		{
			var chainPos = chainCenter + i * new Vector2(chain.Height * scale, 0).RotatedBy(MathHelper.PiOver2 + chainRotation);
			sb.Draw(chain, chainPos, null, Color.White, chainRotation, chain.Size() / 2, scale, SpriteEffects.None, 0);
		}
	}
}