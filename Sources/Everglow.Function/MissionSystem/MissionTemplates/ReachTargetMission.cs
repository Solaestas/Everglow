using Everglow.Commons.Utilities;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to arrive at the designated location.
/// </summary>
[Obsolete("This mission template will cause a lot bug", true)]
public abstract class ReachTargetMission : MissionBase
{
	public class CheckPoint
	{
		public CheckPoint(Vector2 position)
		{
			Position = position;
			Checked = false;
		}

		public Vector2 Position { get; set; }

		public bool Checked { get; set; }

		public void Check() => Checked = true;
	}

	private float progress = 0f;

	public override float Progress => progress;

	public override MissionIconGroup Icon => null;

	/// <summary>
	/// Check distance
	/// </summary>
	public int CheckDistance { get; set; } = 100;

	/// <summary>
	/// The collection of check points
	/// </summary>
	public List<CheckPoint> CheckPoints { get; init; } = [];

	/// <summary>
	/// Determine whether the check points should be passed orderly
	/// </summary>
	public bool IsOrderedCheckPoints { get; set; } = true;

	/// <summary>
	/// The index of current check point
	/// </summary>
	private int currentCheckPointIndex = 0;

	/// <summary>
	/// The instance of current check point
	/// </summary>
	private CheckPoint CurrentCheckPoint => CheckPoints[currentCheckPointIndex];

	public override void Update()
	{
		base.Update();

		UpdateProgress();
	}

	public override void UpdateProgress(params object[] objs)
	{
		var playerPos = Main.LocalPlayer.position;

		if (CheckPoints.Count == 0) // The length of CheckPoints should more than 1
		{
			progress = 1f;
			return;
		}

		var pointsToDraw = new List<Vector2>();
		if (IsOrderedCheckPoints)
		{
			if (HasReached(playerPos, CurrentCheckPoint.Position))
			{
				CurrentCheckPoint.Check();
				currentCheckPointIndex++;
			}

			if (currentCheckPointIndex < CheckPoints.Count)
			{
				pointsToDraw.Add(CurrentCheckPoint.Position);
			}
		}
		else
		{
			foreach (var point in CheckPoints)
			{
				if (point.Checked)
				{
					continue;
				}

				if (HasReached(playerPos, point.Position))
				{
					point.Check();
				}
				else
				{
					pointsToDraw.Add(point.Position);
				}
			}
		}

		DrawCheckPoints(pointsToDraw);
		progress = CheckPoints.Where(p => p.Checked).Count() / (float)CheckPoints.Count;
	}

	public bool HasReached(Vector2 pos, Vector2 targetPos)
	{
		return Vector2.Distance(pos, targetPos) <= CheckDistance;
	}

	public static void DrawCheckPoints(List<Vector2> points)
	{
		// TODO: Use VFXManager to draw the check points instead of using spriteBatch directly on purpose to optimize rendering
		var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin();

		// Draw check point
		foreach (var point in points)
		{
			Main.spriteBatch.Draw(
				Commons.ModAsset.Point.Value,
				point - Main.screenPosition,
				null,
				new Color(0f, 1f, 0f, 0f),
				0,
				Vector2.Zero,
				1f,
				SpriteEffects.None,
				0);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}