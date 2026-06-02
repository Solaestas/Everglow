namespace Everglow.Commons.Physics.MassSpringSystem;

public class GlobalRopeManager : ModSystem
{
	public static EulerSolver EulerSolver = new EulerSolver(8);

	public static PBDSolver PBDSolver = new PBDSolver(8);

	public static List<MassSpringSystem> EularRopeSystems = [];

	public static List<MassSpringSystem> PBDRopeSystems = [];

	public override void Load()
	{
		base.Load();
	}

	public override void PostUpdateEverything()
	{
		foreach(var eularSys in EularRopeSystems)
		{
			EulerSolver.Step(eularSys, 1);
		}
		foreach (var pbdSys in PBDRopeSystems)
		{
			PBDSolver.Step(pbdSys, 1);
		}
		base.PostUpdateEverything();
	}
}