using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics.MassSpringSystem;
public class Rope : IMassSpringMesh
{
	private List<_Mass> masses;
	private List<ElasticConstrain> springs;
	/// <summary>
	/// 自动生成一串由位置决定的绳子链
	/// </summary>
	/// <param name="positions"></param>
	public Rope(List<Vector2> positions, float elasticity, float mass, RenderingTransformFunction renderingTransform, bool hasCollision = false)
	{
		for (int i = 0; i < positions.Count; i++)
		{
			masses.Add(new _Mass(mass, positions[i], i == positions.Count - 1));
		}

		for (int i = 0; i < positions.Count - 1; i++)
		{
			springs.Add(new ElasticConstrain(masses[i], masses[i + 1], (positions[i] - positions[i + 1]).Length(), elasticity));
		}
	}

	public void ApplyForce()
	{
		float gravity = 9;
		for (int i = 0; i < masses.Count; i++)
		{
			_Mass m = masses[i];
			m.Force += new Vector2(2 * (MathF.Sin((float)Main.timeForVisualEffects / 72f + m.Position.X / 13f + m.Position.Y / 4f) + 0.9f), 0)
				* Main.windSpeedCurrent
				+ new Vector2(0, gravity * m.Mass);
		}
	}

	public void ApplyForceSpecial(int index, Vector2 force)
	{
		masses[index].Force += force;
	}

	public List<ElasticConstrain> GetElasticConstrains()
	{
		return springs;
	}

	public List<_Mass> GetMasses()
	{
		return masses;
	}
}
