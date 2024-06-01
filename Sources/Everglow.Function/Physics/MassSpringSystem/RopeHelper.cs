namespace Everglow.Commons.Physics.MassSpringSystem;

public class RopeHelper
{
	/// <summary>
	/// Two points are given to connect the rope and both ends are fixed.
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <param name="count"></param>
	/// <param name="elasticity"></param>
	/// <param name="mass"></param>
	/// <param name="knotDistance"></param>
	/// <param name="knotMass"></param>
	/// <returns></returns>
	public static Rope CreateConnectRope2Points(Vector2 start, Vector2 end, int count, float elasticity, float mass, int knotDistance = 0, float knotMass = 1)
	{
		List<Vector2> positions = new List<Vector2>();
		for (int t = 0; t <= count; t++)
		{
			positions.Add(Vector2.Lerp(start, end, t / (float)count));
		}
		Rope rope = new Rope(positions, elasticity, mass);
		List<_Mass> massList = rope.GetMasses();
		for (int i = 0; i < positions.Count; i++)
		{
			float specialMass = mass;
			if (knotDistance > 0)
			{
				if (i % knotDistance == knotDistance / 2)
				{
					specialMass = knotMass;
				}
			}
			if ((i == positions.Count - 1) || (i == 0))
			{
				massList[i].IsStatic = true;
			}
			massList[i].Mass = specialMass;
			massList[i].Position = positions[i]; // = new _Mass(specialMass, positions[i], (i == positions.Count - 1) || (i == 0));
		}
		return rope;
	}
}