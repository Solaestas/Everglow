namespace Everglow.ZY.Commons.Core;

public interface IUpdateable
{
	void Update();
}

public interface IDrawable
{
	void Draw();
}

public interface IActive
{
	bool Active { get; set; }

	void Kill()
	{
		Active = false;
	}
}

public interface IMoveable
{
	Vector2 Position { get; set; }

	Vector2 Velocity { get; set; }

	void Move();
}
