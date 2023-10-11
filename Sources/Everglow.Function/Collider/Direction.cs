namespace Everglow.Commons.Collider;

[Flags]
public enum Direction : byte
{
	None = 0,
	Up = 1,
	Left = 2,
	Right = 4,
	Down = 8,
	In = 16,
	UpLeft = Up | Left,
	UpRight = Up | Right,
	DownLeft = Down | Left,
	DownRight = Down | Right,
}
