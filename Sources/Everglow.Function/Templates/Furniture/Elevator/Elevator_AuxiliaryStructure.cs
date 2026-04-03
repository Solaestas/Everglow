using Everglow.Commons.CustomTiles.Core;

namespace Everglow.Commons.Templates.Furniture.Elevator;

public abstract class Elevator_AuxiliaryStructure : BoxEntity
{
	public override Color MapColor => new Color(122, 91, 79);

	public CustomElevator ParentElavator;

	public Vector2 RelativePosition;

	public override void SetDefaults()
	{
		Size = new Vector2(96, 16);
		RelativePosition = new Vector2(0, 0);
	}

	public override void AI()
	{
		if (ParentElavator is null || !ParentElavator.Active)
		{
			Active = false;
			return;
		}

		Vector2 toRelative = ParentElavator.Box.Center + ParentElavator.Velocity + RelativePosition - Box.Center;
		if (toRelative.Length() > 0.05f)
		{
			Velocity = toRelative;
		}
		float maxSpeed = ParentElavator.Velocity.Length() * 2f + Math.Max(ParentElavator.StopTimer - ParentElavator.StopTimeMax + 5, 0);
		if (Velocity.Length() > maxSpeed)
		{
			Velocity = Velocity.SafeNormalize(Vector2.Zero) * maxSpeed;
		}
	}

	public override void Draw()
	{
		if (PreDraw())
		{
			base.Draw();
		}
	}

	public virtual bool PreDraw()
	{
		return true;
	}

	public virtual void PostDraw()
	{
	}
}