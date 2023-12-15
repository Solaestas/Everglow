using Everglow.Commons.Skeleton2D;
using Everglow.Commons.Skeleton2D.Reader;
using Terraria.DataStructures;

namespace Everglow.Example.Skeleton;

public class SkeletonPlayerLayer : PlayerDrawLayer
{
	private Skeleton2D skeleton2D;
	public override void Load()
	{
		var data = Mod.GetFileBytes("Example/Skeleton/Animations/owl-pro.json");
		skeleton2D = Skeleton2DReader.ReadSkeleton(data, $"Everglow/Example/Skeleton/Animations/");
	}
	public override Position GetDefaultPosition()
	{
		return new BeforeParent(PlayerDrawLayers.ProjectileOverArm);
	}

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		return true;
	}

	public override void Draw(ref PlayerDrawSet drawInfo)
	{
		var player = drawInfo.drawPlayer;
		if (player.HeldItem == null || player.HeldItem.type == ItemID.None)
		{
			skeleton2D.Position = player.Center;
			skeleton2D.Rotation = 0f;
			// skeleton2D.InverseKinematics(Main.MouseWorld);
			skeleton2D.PlayAnimation("idle", (float)Main.time % 300 / 60f);
			skeleton2D.DrawDebugView(Main.spriteBatch);
		}
	}
}
