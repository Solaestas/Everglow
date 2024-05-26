using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics.MassSpringSystem;
public class PBDSolver : Solver
{
	private readonly int iterations;
	public PBDSolver(int iterations)
	{
		this.iterations = iterations;
	}
	public override void Step(MassSpringSystem system, float deltaTime)
	{
		float alpha = 1e-6f;

		for (int iter = 0; iter < iterations; iter++)
		{
			// Time integration
			foreach (var p in system.Masses)
			{
				p.PrevPosition = p.Position;
				p.Position += deltaTime * p.Velocity;
			}

			// Constraint solving
			foreach (var spring in system.Springs)
			{
				var p1 = spring.A;
				var p2 = spring.B;
				var restLength = spring.RestLength;
				var stiffness = spring.Stiffness;
				var d = p1.Position - p2.Position;
				var L = d.Length();
				var C = L - restLength;
				if (L > 1e-6f) // To avoid division by zero
				{
					var gradient = d / L;
					var W = 1.0f / p1.Mass + 1.0f / p2.Mass;
					var compliance = 1.0f / (stiffness * deltaTime * deltaTime);
					var lambda = -(C + compliance * spring.LambdaPrev) / (W + compliance);
					var correction = lambda * gradient;
					p1.Position += correction / p1.Mass;
					p2.Position -= correction / p2.Mass;
					spring.LambdaPrev = lambda; // Update lambda_prev
				}
			}

			// Update velocities
			foreach (var p in system.Masses)
			{
				p.Velocity = (p.Position - p.PrevPosition) / deltaTime;
			}
		}
	}
}
