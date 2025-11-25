namespace Everglow.Commons.Utilities;

public static partial class MathUtils
{
	/// <summary>
	/// Calculate displacement of uniformly accelerated rectilinear motion.
	/// </summary>
	/// <param name="initialVelocity"></param>
	/// <param name="constantAcceleration"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	public static float UARNDisplacement(float initialVelocity, float constantAcceleration, float time)
	{
		return initialVelocity * time + 0.5f * constantAcceleration * time * time;
	}
}