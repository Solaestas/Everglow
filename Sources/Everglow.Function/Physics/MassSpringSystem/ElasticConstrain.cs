namespace Everglow.Commons.Physics.MassSpringSystem;

public class ElasticConstrain(Mass mass1, Mass mass2, float restLength, float stiffness)
{
	public Mass A = mass1;

	public Mass B = mass2;

	public float RestLength = restLength;

	public float Stiffness = stiffness;

	public float LambdaPrev = 0;
}