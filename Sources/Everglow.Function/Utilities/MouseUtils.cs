namespace Everglow.Commons.Utilities;

public interface IMouseState
{
	/// <summary>
	/// Indicates whether the mouse button was just clicked (pressed down).
	/// </summary>
	public bool IsClicked { get; }

	/// <summary>
	/// Indicates whether the mouse button was just released.
	/// </summary>
	public bool IsReleased { get; }

	/// <summary>
	/// Indicates whether the mouse button is currently being held down.
	/// </summary>
	public bool IsHeld { get; }

	/// <summary>
	/// Indicates whether the mouse button is in a released state (not pressed).
	/// </summary>
	public bool IsUp { get; }
}

public static class MouseUtils
{
	public static readonly IMouseState MouseLeft = new MouseLeftState();

	public static readonly IMouseState MouseMiddle = new MouseMiddleState();

	public static readonly IMouseState MouseRight = new MouseRightState();

	private class MouseLeftState : IMouseState
	{
		public bool IsClicked => Main.mouseLeft && Main.mouseLeftRelease;

		public bool IsReleased => !Main.mouseLeft && !Main.mouseLeftRelease;

		public bool IsHeld => Main.mouseLeft && !Main.mouseLeftRelease;

		public bool IsUp => !Main.mouseLeft && Main.mouseLeftRelease;
	}

	private class MouseMiddleState : IMouseState
	{
		public bool IsClicked => Main.mouseMiddle && Main.mouseMiddleRelease;

		public bool IsReleased => !Main.mouseMiddle && !Main.mouseMiddleRelease;

		public bool IsHeld => Main.mouseMiddle && !Main.mouseMiddleRelease;

		public bool IsUp => !Main.mouseMiddle && Main.mouseMiddleRelease;
	}

	private class MouseRightState : IMouseState
	{
		public bool IsClicked => Main.mouseRight && Main.mouseRightRelease;

		public bool IsReleased => !Main.mouseRight && !Main.mouseRightRelease;

		public bool IsHeld => Main.mouseRight && !Main.mouseRightRelease;

		public bool IsUp => !Main.mouseRight && Main.mouseRightRelease;
	}
}