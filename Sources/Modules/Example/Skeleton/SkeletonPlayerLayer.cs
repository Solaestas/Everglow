using Everglow.Commons.Skeleton2D;
using Everglow.Commons.Skeleton2D.Reader;
using Everglow.Commons.Skeleton2D.Renderer.DrawCommands;
using ReLogic.Content;
using Spine;
using Terraria.DataStructures;
using static ReLogic.Peripherals.RGB.Corsair.CorsairDeviceGroup;

namespace Everglow.Example.Skeleton;

public class SkeletonPlayerLayer : PlayerDrawLayer
{
	private Skeleton2D skeleton2D;
	private Everglow.Commons.Skeleton2D.Renderer.SkeletonDebugRenderer debugRenderer;
	private Everglow.Commons.Skeleton2D.Renderer.SkeletonRenderer skeletonRenderer;

	public override void Load()
	{
		var json = Mod.GetFileBytes("Example/Skeleton/Animations/raptor-pro.json");
		var atlas = Mod.GetFileBytes("Example/Skeleton/Animations/raptor-pro.atlas");
		skeleton2D = Skeleton2DReader.ReadSkeleton(atlas, json, ModAsset.raptorqqq.Value);

		skeleton2D.AnimationState.SetAnimation(0, "walk", true);
		skeleton2D.AnimationState.AddAnimation(0, "jump", false, 2);
		skeleton2D.AnimationState.AddAnimation(0, "roar", true, 0);
		Main.QueueMainThreadAction(() =>
		{
			debugRenderer = new Commons.Skeleton2D.Renderer.SkeletonDebugRenderer(Main.graphics.GraphicsDevice);
			skeletonRenderer = new Commons.Skeleton2D.Renderer.SkeletonRenderer(Main.graphics.GraphicsDevice);
		});
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
		if (true)
		{
			skeleton2D.AnimationState.Update(0.02f);
			skeleton2D.AnimationState.Apply(skeleton2D.Skeleton);
			skeleton2D.Skeleton.X = Main.screenWidth / 2;
			skeleton2D.Skeleton.Y = Main.screenHeight;

			skeleton2D.Skeleton.UpdateWorldTransform();

			skeleton2D.Position = player.Center;
			skeleton2D.Rotation = 0f;
			//debugRenderer.DisableAll();
			//debugRenderer.DrawBones = true;

			//debugRenderer.Begin();
			//debugRenderer.Draw(skeleton2D);
			//debugRenderer.End();

			skeletonRenderer.Begin();

			Main.graphics.graphicsDevice.RasterizerState = new RasterizerState
			{
				FillMode = FillMode.Solid,
			};
			var cmdList = skeletonRenderer.Draw(skeleton2D);
			NaiveExecuter executer = new NaiveExecuter();
			executer.Execute(cmdList, Main.graphics.graphicsDevice);
			Main.graphics.graphicsDevice.RasterizerState = new RasterizerState
			{
				FillMode = FillMode.Solid,
			};
		}
	}
}