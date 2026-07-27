namespace Everglow.Commons.Physics.MassSpringSystem;

public abstract class Solver
{
	public abstract void Step(MassSpringContainer system, float deltaTime);
}
