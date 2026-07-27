namespace Everglow.Commons.Physics.MassSpringSystem;

public class MassSpringContainer
{
	public Solver Solver { get; }

	public List<Mass> Masses { get; }

	public List<ElasticConstrain> Springs { get; }

	public float Damping { get; set; }

	public MassSpringContainer(Solver solver = null)
	{
		Masses = [];
		Springs = [];
		Damping = 0.99f;
		Solver = solver;
	}

	public void AddMassSpringMesh(IMassSpringMesh mesh)
	{
		Masses.AddRange(mesh.Masses);
		Springs.AddRange(mesh.ElasticConstrains);
	}
}
