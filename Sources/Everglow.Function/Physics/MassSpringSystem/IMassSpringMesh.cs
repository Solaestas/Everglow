namespace Everglow.Commons.Physics.MassSpringSystem;

public interface IMassSpringMesh
{
	public Mass[] Masses { get; }

	public ElasticConstrain[] ElasticConstrains { get; }
}