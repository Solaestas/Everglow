namespace Everglow.Commons.Physics.MassSpringSystem;

public class PBDSolver(int iterations) : Solver
{
	public override void Step(MassSpringSystem system, float deltaTime)
	{
		float dt = deltaTime / iterations;
		for (int k = 0; k < iterations; k++)
		{
			for (int i = 0; i < system.Masses.Count; i++)
			{
				Mass m = system.Masses[i];

				m.DeltaPos = Vector2.Zero;
				m.HessianDiag = 0;
				m.OldPos = m.Position;

				if (!m.IsStatic)
				{
					m.Velocity += dt * m.Force / m.Value;
					m.Position += dt * m.Velocity;
				}
			}

			// Constraint solving
			for (int i = 0; i < system.Springs.Count; i++)
			{
				ElasticConstrain spring = system.Springs[i];
				Mass p1 = spring.A;
				Mass p2 = spring.B;
				var restLength = spring.RestLength;
				var stiffness = spring.Stiffness;

				var d = p1.Position - p2.Position;
				var L = d.Length();
				var C = L - restLength;

				var gradient = d.SafeNormalize(Vector2.One);

				var invMa = p1.IsStatic ? 0 : 1.0f / p1.Value;
				var invMb = p2.IsStatic ? 0 : 1.0f / p2.Value;

				// var W = 1.0f / p1.Value + 1.0f / p2.Value;
				var compliance = 1.0f / (stiffness * dt * dt);
				var lambda = -(C + compliance * spring.LambdaPrev) / (invMa + invMb + compliance);

				p1.DeltaPos += gradient * invMa * lambda;
				p1.HessianDiag += 1;

				// p1.Position += correction * invMa * lambda;
				p2.DeltaPos -= gradient * invMb * lambda;
				p2.HessianDiag += 1;

				spring.LambdaPrev = lambda; // Update lambda_prev
			}

			// Update velocities
			for (int i = 0; i < system.Masses.Count; i++)
			{
				Mass m = system.Masses[i];

				if (!m.IsStatic)
				{
					// if (hasCollision)
					// {
					// m.Position = CheckCollision(i, m.Position, dummyPos[i]);
					// }
					if (m.HessianDiag > 0)
					{
						m.Position += m.DeltaPos / m.HessianDiag;
						m.Velocity = (m.Position - m.OldPos) / dt;
					}
				}
			}
		}

		for (int i = 0; i < system.Masses.Count; i++)
		{
			Mass m = system.Masses[i];
			m.Force = Vector2.Zero;

			m.Velocity *= MathF.Pow(system.Damping, deltaTime);
		}
	}
}