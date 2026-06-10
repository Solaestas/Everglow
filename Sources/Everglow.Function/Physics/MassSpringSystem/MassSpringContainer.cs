namespace Everglow.Commons.Physics.MassSpringSystem;

public class MassSpringContainer
{
	public List<Mass> Masses { get; }

	public List<ElasticConstrain> Springs { get; }

	public float Damping { get; set; }

	public MassSpringContainer()
	{
		Masses = [];
		Springs = [];
		Damping = 0.99f;
	}

	public void AddMassSpringMesh(IMassSpringMesh mesh)
	{
		Masses.AddRange(mesh.Masses);
		Springs.AddRange(mesh.ElasticConstrains);
	}
}