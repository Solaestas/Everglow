using Spine;

namespace Everglow.Commons.Skeleton2D;

public class Skeleton2D
{
	public static bool ShowSkeleton => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<SkeletonSight>();

	private Atlas atlas;
	private Skeleton skeleton;
	private AnimationState animationState;

	// Mark this to public if it's necessary to access the atlas directly
	private Atlas Atlas => atlas;

	public Skeleton Skeleton => skeleton;

	public AnimationState AnimationState => animationState;

	public Skeleton2D(Skeleton skeleton, Atlas atlas, AnimationState state)
	{
		this.skeleton = skeleton;
		this.atlas = atlas;
		this.animationState = state;
	}

	// public Dictionary<string, Animation> Animations { get; set; }

	/// <summary>
	/// World space position of the skeleton relative to the root bone
	/// </summary>
	public Vector2 Position
	{
		get => new Vector2(skeleton.X, skeleton.Y);

		set
		{
			skeleton.X = value.X;
			skeleton.Y = value.Y;
			skeleton.UpdateWorldTransform();
		}
	}

	/// <summary>
	/// World space rotation in radians
	/// </summary>
	public float Rotation
	{
		get => skeleton.RootBone.Rotation / MathUtils.RadDeg;

		set
		{
			skeleton.RootBone.Rotation = value * MathUtils.RadDeg;
			skeleton.UpdateWorldTransform();
		}
	}

	public void PlayAnimation(int trackIndex, string name, float time)
	{
		animationState.SetAnimation(trackIndex, name, false);
		animationState.Update(time);

		// var animation = Animations[name];
		// foreach (var boneTimeline in animation.BonesTimeline)
		// {
		// 	foreach (var track in boneTimeline.Tracks)
		// 	{
		// 		track.LocateKeyFrames(time, out IKeyFrame cur, out IKeyFrame next);
		// 		cur?.Interpolate(time, next);
		// 	}
		// }
		// foreach (var slotTimeline in animation.SlotsTimeline)
		// {
		// 	foreach (var track in slotTimeline.Tracks)
		// 	{
		// 		track.LocateKeyFrames(time, out IKeyFrame cur, out IKeyFrame next);
		// 		cur?.Interpolate(time, next);
		// 	}
		// }
	}
}