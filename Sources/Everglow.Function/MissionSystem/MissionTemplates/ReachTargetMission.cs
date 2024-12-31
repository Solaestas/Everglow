using Everglow.Commons.Utilities;

namespace Everglow.Commons.MissionSystem.MissionTemplates;

/// <summary>
/// Represents a mission where the player needs to arrive at the designated location.
/// </summary>
public class ReachTargetMission : MissionBase
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

	private string _name = string.Empty;
	private string _displayName = string.Empty;
	private string _description = string.Empty;
	private float _progress = 0f;
	private long _timeMax = -1;

	public override string Name => _name;

	public override string DisplayName => _displayName;

	public override string Description => _description;

	public override float Progress => _progress;

	public override long TimeMax => _timeMax;

	public override Texture2D Icon => null;

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

	/// <summary>
	/// Sets the basic information for the mission.
	/// </summary>
	/// <param name="name">The unique name of the mission.</param>
	/// <param name="displayName">The display name of the mission.</param>
	/// <param name="description">A brief description of the mission.</param>
	/// <param name="timeMax">The maximum time allowed to complete the mission, in ticks. Use -1 for no time limit.</param>
	/// <exception cref="ArgumentNullException">Thrown if any of the string parameters are null.</exception>
	public void SetInfo(string name, string displayName, string description, long timeMax = -1)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException(nameof(name), "Mission name cannot be null or empty.");
		}

		if (string.IsNullOrWhiteSpace(displayName))
		{
			throw new ArgumentNullException(nameof(displayName), "Mission display name cannot be null or empty.");
		}

		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentNullException(nameof(description), "Mission description cannot be null or empty.");
		}

		_name = name;
		_displayName = displayName;
		_description = description;
		_timeMax = timeMax;
	}

	public override void Update()
	{
		base.Update();

		UpdateProgress(Main.LocalPlayer.position);
	}

	public override void UpdateProgress(params object[] objs)
	{
		if (PoolType != MissionManager.PoolType.Accepted || objs[0] is not Vector2 playerPos)
		{
			return;
		}

		if (CheckPoints.Count == 0) // The length of CheckPoints should more than 1
		{
			_progress = 1f;
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
		_progress = CheckPoints.Where(p => p.Checked).Count() / (float)CheckPoints.Count;
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