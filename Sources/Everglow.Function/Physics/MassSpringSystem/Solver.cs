namespace Everglow.Commons.Physics.MassSpringSystem;

public abstract class Solver
{
	public abstract void Step(MassSpringSystem system, float deltaTime);
}