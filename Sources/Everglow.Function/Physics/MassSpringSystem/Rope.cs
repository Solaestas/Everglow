namespace Everglow.Commons.Physics.MassSpringSystem;

public class Rope : IMassSpringMesh
{
	public const float Gravity = 9f;

	private Mass[] _masses;

	private ElasticConstrain[] _springs;

	/// <summary>
	/// 自动生成一串由位置决定的绳子链
	/// </summary>
	/// <param name="positions"> </param>
	private Rope(int count)
	{
		_masses = new Mass[count];
		_springs = new ElasticConstrain[count - 1];
	}

	public ElasticConstrain[] ElasticConstrains => _springs;

	public Mass[] Masses => _masses;

	/// <summary>
	/// Two points are given to connect the rope and both ends are fixed.
	/// </summary>
	/// <param name="start"> </param>
	/// <param name="end"> </param>
	/// <param name="count"> </param>
	/// <param name="elasticity"> </param>
	/// <param name="mass"> </param>
	/// <param name="knotDistance"> </param>
	/// <param name="knotMass"> </param>
	/// <returns> </returns>
	public static Rope Create(Vector2 start, Vector2 end, int count, float elasticity, float mass, int knotDistance = 0, float knotMass = 1)
	{
		Rope rope = new Rope(count);
		for (int i = 0; i < count; i++)
		{
			var position = Vector2.Lerp(start, end, i / (count - 1f));
			float specialMass = mass;
			if (knotDistance > 0)
			{
				if (i % knotDistance == knotDistance / 2)
				{
					specialMass = knotMass;
				}
			}
			var m = rope._masses[i] = new Mass(specialMass, position, i == 0 || i == count - 1);

			// give a tiny force to break the initial balance.
			m.Velocity = new Vector2(0, 0.1f).RotatedBy(i);
			if (i != 0)
			{
				var prev = rope._masses[i - 1];
				rope._springs[i - 1] = new ElasticConstrain(prev, rope._masses[i],
					(prev.Position - m.Position).Length(), elasticity);
			}
		}
		return rope;
	}

	/// <summary>
	/// Create a rope at <paramref name="start" /> extending to the positive y-axis.
	/// <br /><paramref name="start" /> is fixed.
	/// </summary>
	/// <param name="start"> </param>
	/// <param name="count"> </param>
	/// <param name="elasticity"> </param>
	/// <param name="mass"> </param>
	/// <returns> </returns>
	public static Rope Create(Vector2 start, int count, float elasticity, float mass)
	{
		Rope rope = new Rope(count);
		for (int i = 0; i < count; i++)
		{
			var position = start + new Vector2(0, mass * 10 * i);
			var m = rope._masses[i] = new Mass(mass, position, i == 0);
			if (i != 0)
			{
				var prev = rope._masses[i - 1];
				rope._springs[i - 1] = new ElasticConstrain(prev, rope._masses[i],
					(prev.Position - m.Position).Length(), elasticity);
			}
		}
		return rope;
	}

	/// <summary>
	/// Create a rope at <paramref name="start" /> extending to the positive y-axis.
	/// <br /><paramref name="start" /> is fixed.
	/// </summary>
	/// <param name="start"> </param>
	/// <param name="count"> </param>
	/// <param name="elasticity"> </param>
	/// <param name="mass"> </param>
	/// <returns> </returns>
	public static Rope CreateWithHangHead(Vector2 start, int count, float elasticity, float mass, float headMass, int disableCount = 0, float restJointDistance = 6)
	{
		Rope rope = new Rope(count);
		for (int i = 0; i < count; i++)
		{
			int phasePos = i - disableCount;
			if (phasePos < 0)
			{
				phasePos = 0;
			}
			var position = start + new Vector2(0, restJointDistance * phasePos);
			var m = rope._masses[i] = new Mass(mass, position, i == 0);
			if (i == count - 1)
			{
				m.Value = headMass;
			}
			if (i != 0)
			{
				var prev = rope._masses[i - 1];
				rope._springs[i - 1] = new ElasticConstrain(prev, rope._masses[i],
					restJointDistance, elasticity);
			}
		}
		return rope;
	}

	public void ApplyForce()
	{
		for (int i = 0; i < _masses.Length; i++)
		{
			Mass m = _masses[i];
			m.Force += new Vector2(2 * (MathF.Sin((float)Main.timeForVisualEffects / 72f + m.Position.X / 13f + m.Position.Y / 4f) + 0.9f), 0)
				* Main.windSpeedCurrent
				+ new Vector2(0, Gravity * m.Value);
		}
	}

	public void ApplyForceSpecial(int index, Vector2 force)
	{
		_masses[index].Force += force;
	}
}