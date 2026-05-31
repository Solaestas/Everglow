namespace Everglow.Commons.Physics.MassSpringSystem;

public class GlobalRopeManager : ModSystem
{
	public static EulerSolver EulerSolver = new EulerSolver(8);

	public static PBDSolver PBDSolver = new PBDSolver(8);

	public static MassSpringSystem EularRopeSystem = new MassSpringSystem();

	public static MassSpringSystem PBDRopeSystem = new MassSpringSystem();

	public override void Load()
	{
		base.Load();
	}

	public override void PostUpdateEverything()
	{
		EulerSolver.Step(EularRopeSystem, 1);
		PBDSolver.Step(PBDRopeSystem, 1);
		base.PostUpdateEverything();
	}
}