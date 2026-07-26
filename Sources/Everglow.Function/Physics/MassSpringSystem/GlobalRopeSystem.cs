namespace Everglow.Commons.Physics.MassSpringSystem;

public class GlobalRopeSystem : ModSystem
{
	private static EulerSolver EulerSolver { get; set; }

	private static PBDSolver PBDSolver { get; set; }

	public static List<MassSpringContainer> EulerContainers { get; private set; }

	public static List<MassSpringContainer> PBDContainers { get; private set; }

	public override void Load()
	{
		EulerSolver = new EulerSolver(8);
		PBDSolver = new PBDSolver(8);

		EulerContainers = [];
		PBDContainers = [];
	}

	public override void Unload()
	{
		EulerSolver = null;
		PBDSolver = null;

		EulerContainers.Clear();
		EulerContainers = [];

		PBDContainers.Clear();
		PBDContainers = [];
	}

	public override void PostUpdateEverything()
	{
		foreach (var euler in EulerContainers)
		{
			if (euler.Solver is not null)
			{
				euler.Solver.Step(euler, 1);
			}
			else
			{
				EulerSolver.Step(euler, 1);
			}
		}
		foreach (var pbd in PBDContainers)
		{
			if (pbd.Solver is not null)
			{
				pbd.Solver.Step(pbd, 1);
			}
			else
			{
				PBDSolver.Step(pbd, 1);
			}
		}
	}
}
