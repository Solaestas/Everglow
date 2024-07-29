using Everglow.Commons.Utilities;

namespace Everglow.Commons.Physics.MassSpringSystem;

public class EulerSolver(int iterations) : Solver
{
	private static Vector2 G_prime(Mass A, Mass B, float elasticity, float restLength)
	{
		var offset = A.Position - B.Position;
		var length = offset.Length();
		var unit = offset.NormalizeSafe();

		return -elasticity * (length - restLength) * unit;
	}

	public override void Step(MassSpringSystem system, float deltaTime)
	{
		for (int i = 0; i < system.Masses.Count; i++)
		{
			Mass m = system.Masses[i];

			// Detect Nans
			Debug.Assert(!m.Position.HasNaNs());

			m.Velocity *= MathF.Pow(system.Damping, deltaTime);
			if (m.IsStatic)
			{
				// m.Position = m.Position;
				m.OldPos = m.Position;
			}
			else
			{
				// m.Velocity += m.Force / m.Mass * deltaTime;
				m.OldPos = m.Position + m.Velocity * deltaTime;
				m.Position = m.OldPos;
			}
		}

		for (int k = 0; k < iterations; k++)
		{
			for (int i = 0; i < system.Masses.Count; i++)
			{
				Mass m = system.Masses[i];
				m.Gradient = m.Value / (deltaTime * deltaTime) * (m.Position - m.OldPos);
				m.Gradient -= m.Force;
				m.HessianDiag = 0;
			}

			for (int i = 0; i < system.Springs.Count; i++)
			{
				ElasticConstrain spr = system.Springs[i];
				Vector2 v = G_prime(spr.A, spr.B, spr.Stiffness, spr.RestLength);
				spr.A.Gradient -= v;
				spr.B.Gradient -= -v;

				spr.A.HessianDiag += spr.Stiffness;
				spr.B.HessianDiag += spr.Stiffness;
			}

			for (int i = 0; i < system.Masses.Count; i++)
			{
				Mass m = system.Masses[i];
				if (m.IsStatic)
				{
					continue;
				}
				float alpha = 1f / (m.Value / (deltaTime * deltaTime) + m.HessianDiag);
				var dx = alpha * m.Gradient;
				m.Position -= dx;
			}
		}

		for (int i = 0; i < system.Masses.Count; i++)
		{
			Mass m = system.Masses[i];
			m.Force = Vector2.Zero;
			if (m.IsStatic)
			{
				continue;
			}
			m.Velocity += (m.Position - m.OldPos) / deltaTime;
		}
	}
}