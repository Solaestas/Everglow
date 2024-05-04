using Everglow.Commons.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Everglow.Commons.Physics;

public delegate Vector2 RenderingTransformFunction(Vector2 pos);

public class Rope
{
	public struct _Mass
	{
		public bool IsStatic;
		public float Mass;
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector2 Force;

		public _Mass()
		{
			this.Mass = 1f;
			this.Velocity = Vector2.Zero;
			this.Force = Vector2.Zero;
			this.Position = Vector2.Zero;
			this.IsStatic = false;
		}
	}

	private static Vector2 RotateBy(Vector2 vector, float radians)
	{
		float cos = MathF.Cos(radians);
		float sin = MathF.Sin(radians);
		return new Vector2(
			vector.X * cos - vector.Y * sin,
			vector.X * sin + vector.Y * cos);
	}

	public struct Spring
	{
		internal float Elasticity;
		internal float RestLength;
		internal int A;
		internal int B;

		public Spring()
		{
			this.Elasticity = 1f;
			this.RestLength = 0f;
			this.A = 0;
			this.B = 0;
		}
	}

	private _Mass[] masses;
	private Spring[] springs;
	private Vector2[] dummyPos;
	private Vector2[] gradiants;
	private RenderingTransformFunction renderingTransform;

	private float damping;
	private float elasticity;
	private bool hasCollision;

	public _Mass[] GetMassList
	{
		get
		{
			return masses;
		}
	}

	public Spring[] GetSpringList
	{
		get
		{
			return springs;
		}
	}

	public RenderingTransformFunction RenderingTransform
	{
		get
		{
			return renderingTransform;
		}
	}

	private float CrossProduct2D(Vector2 a, Vector2 b)
	{
		return a.X * b.Y - a.Y * b.X;
	}

	public Rope Clone(Vector2 deltaPosition)
	{
		Rope clone = new Rope();
		clone.elasticity = elasticity;
		clone.damping = damping;
		clone.renderingTransform = renderingTransform;
		clone.masses = new _Mass[masses.Length];
		clone.springs = new Spring[springs.Length];
		clone.gradiants = new Vector2[gradiants.Length];
		clone.dummyPos = new Vector2[dummyPos.Length];
		masses.CopyTo(clone.masses, 0);
		springs.CopyTo(clone.springs, 0);
		gradiants.CopyTo(clone.gradiants, 0);
		dummyPos.CopyTo(clone.dummyPos, 0);
		for (int i = 0; i < clone.masses.Length; i++)
		{
			clone.masses[i].Position += deltaPosition;
		}
		return clone;
	}

	private Rope()
	{
		damping = 0.99f;
		hasCollision = false;
	}

	private Vector2 G_prime(int A, int B, float elasticity, float restLength)
	{
		var offset = dummyPos[A] - dummyPos[B];
		var length = offset.Length();
		var unit = offset.NormalizeSafe();

		return -elasticity * (length - restLength) * unit;
	}

	private Matrix<float> G_Hessian(float dt, int A, int B, float elasticity, float restLength)
	{
		ref _Mass mA = ref masses[A];
		ref _Mass mB = ref masses[B];

		var offset = dummyPos[A] - dummyPos[B];
		var length = (float)offset.Length();
		var length2 = Vector2.Dot(offset, offset);

		var span = offset.ToMathNetVector().OuterProduct(offset.ToMathNetVector());
		var term1 = span * elasticity / length2;
		var term2 = (Matrix<float>.Build.DenseIdentity(2) - span / length2) * elasticity * (1 - restLength / length);
		return Matrix<float>.Build.DenseIdentity(2) * mA.Mass / (dt * dt) + term1 + term2;
	}

	private Vector2 CheckCollision(int i, Vector2 oldPos, Vector2 newPos)
	{
		ref _Mass m = ref masses[i];
		Vector3 sdf = SDFUtils.CalculateTileSDF(m.Position);
		float EPS = 1e-2f;
		if (sdf.X < EPS)
		{
			newPos -= 1f * (EPS - sdf.X) * new Vector2(sdf.Y, sdf.Z);
		}
		return newPos;
	}

	public void ApplyForce()
	{
		float gravity = 9;
		for (int i = 0; i < masses.Length; i++)
		{
			ref _Mass m = ref masses[i];
			m.Force += new Vector2(2 * (MathF.Sin((float)Main.timeForVisualEffects / 72f + m.Position.X / 13f + m.Position.Y / 4f) + 0.9f), 0)
				* Main.windSpeedCurrent
				+ new Vector2(0, gravity * m.Mass);
		}
	}

	public void ApplyForceSpecial(int index, Vector2 force)
	{
		ref _Mass m = ref masses[index];
		m.Force += force;
	}

	public void ClearForce()
	{
		// for (int i = 0; i < m_masses.Length; i++)
		// {
		//    ref _Mass m = ref m_masses[i];

		// }
	}

	public void Update(float deltaTime)
	{
		for (int i = 0; i < masses.Length; i++)
		{
			ref _Mass m = ref masses[i];
			if (float.IsNaN(m.Position.X) && float.IsNaN(m.Position.Y))
			{
				Terraria.Main.NewText("!!");
			}

			m.Velocity *= (float)Math.Pow(damping, deltaTime);
			dummyPos[i] = m.Position + m.Velocity * deltaTime;

			if (m.IsStatic)
			{
				dummyPos[i] = m.Position;
			}
		}

		for (int k = 0; k < 16; k++)
		{
			for (int i = 0; i < masses.Length; i++)
			{
				ref _Mass m = ref masses[i];
				Vector2 x_hat = m.Position + deltaTime * m.Velocity;
				gradiants[i] = m.Mass / (deltaTime * deltaTime) * (dummyPos[i] - x_hat);
				gradiants[i] -= m.Force;
			}

			for (int i = 0; i < springs.Length; i++)
			{
				ref Spring spr = ref springs[i];
				Vector2 v = G_prime(spr.A, spr.B, spr.Elasticity, spr.RestLength);
				gradiants[spr.A] -= v;
				gradiants[spr.B] -= -v;

				// var He = G_Hessian(deltaTime, spr.A, spr.B, spr.Elasticity, spr.RestLength);
				// m_springH[spr.A, spr.A] = He;
				// m_springH[spr.B, spr.B] = He;
				// m_springH[spr.A, spr.B] = -He;
				// m_springH[spr.B, spr.A] = -He;
			}

			for (int i = 0; i < masses.Length; i++)
			{
				ref _Mass m = ref masses[i];
				m.Force = Vector2.Zero;
				if (m.IsStatic)
				{
					continue;
				}
				float alpha = 1f / (m.Mass / (deltaTime * deltaTime) + 4 * elasticity);
				var dx = alpha * gradiants[i];
				dummyPos[i] -= dx;
			}
		}

		if (hasCollision)
		{
			float tTest;
			bool res = CollisionUtils.CCD_SegmentPoint(new Vector2(0, 2), new Vector2(1, -2), new Vector2(0, 0), new Vector2(0, -1), new Vector2(0.5f, 0.5f), Vector2.Zero, out tTest);
			for (int i = 0; i < springs.Length; i++)
			{
				ref Spring spr = ref springs[i];
				ref _Mass A = ref masses[spr.A];
				ref _Mass B = ref masses[spr.B];

				Vector2 Av = dummyPos[spr.A] - A.Position;
				Vector2 Bv = dummyPos[spr.B] - B.Position;

				Vector2 center = (dummyPos[spr.A] + dummyPos[spr.B]) / 2f;
				int tileX = (int)(center.X / 16);
				int tileY = (int)(center.Y / 16);

				Vector2 baseOffset = new Vector2(tileX * 16 - 16, tileY * 16 - 16);

				float minTimeToCollision = 1f;
				Vector2 targetPos = Vector2.Zero;
				Vector2 normal = Vector2.Zero;
				for (int a = -3; a <= 3; a++)
				{
					for (int b = -3; b <= 3; b++)
					{
						if (tileX + b < 0 || tileX + b >= Terraria.Main.maxTilesX || tileY + a < 0 || tileY + a >= Terraria.Main.maxTilesY)
						{
							continue;
						}
						var tile = Terraria.Main.tile[tileX + b, tileY + a];
						ICollider2D collider = SDFUtils.ExtractColliderFromTile(tile, tileX + b, tileY + a, false);

						if (collider != null)
						{
							IPolygonalCollider2D collider2d = collider as IPolygonalCollider2D;
							foreach (var point in collider2d.GetPolygon().Points)
							{
								float t;
								if (CollisionUtils.CCD_SegmentPoint(A.Position - baseOffset, Av, B.Position - baseOffset, Bv, point - baseOffset, Vector2.Zero, out t))
								{
									if (t < 1 && t < minTimeToCollision)
									{
										minTimeToCollision = t;

										var proj = A.Position + Vector2.Dot(B.Position - A.Position, point - A.Position) * Vector2.Normalize(B.Position - A.Position);
										normal = Vector2.Normalize(point - proj);
										targetPos = point;
									}
								}
							}
						}
					}
				}

				if (minTimeToCollision < 1)
				{
					// Terraria.Main.NewText(minTimeToCollision);
					minTimeToCollision *= 0.9f;

					Vector2 contVA = Av * (1 - minTimeToCollision);
					Vector2 contVB = Bv * (1 - minTimeToCollision);

					Vector2 dummyA = A.Position + Av * minTimeToCollision;
					Vector2 dummyB = B.Position + Bv * minTimeToCollision;

					var torqueA = CrossProduct2D(dummyA - targetPos, contVA);
					var torqueB = CrossProduct2D(dummyB - targetPos, contVB);

					var MoI = (dummyA - targetPos).LengthSquared() * A.Mass + (dummyB - targetPos).LengthSquared() * B.Mass;

					var rot = -(torqueA + torqueB) / MoI * 10;
					var rr = RotateBy(dummyA - targetPos, rot);
					dummyPos[spr.A] = targetPos + RotateBy(dummyA - targetPos, rot);
					dummyPos[spr.B] = targetPos + RotateBy(dummyB - targetPos, -rot);
				}
			}
		}

		for (int i = 0; i < masses.Length; i++)
		{
			ref _Mass m = ref masses[i];
			if (m.IsStatic)
			{
				continue;
			}
			Vector2 x_hat = m.Position + deltaTime * m.Velocity;

			if (hasCollision)
			{
				m.Position = CheckCollision(i, m.Position, dummyPos[i]);
			}
			else
			{
				m.Position = dummyPos[i];
			}

			m.Velocity += (m.Position - x_hat) / deltaTime;
		}
	}

	private void InitRopes(int count, float elasticity, RenderingTransformFunction renderingTransform, bool hasCollision)
	{
		masses = new _Mass[count];
		dummyPos = new Vector2[count];
		gradiants = new Vector2[count];
		springs = new Spring[count - 1];
		damping = 0.99f;

		this.elasticity = elasticity;
		this.renderingTransform = renderingTransform;
		this.hasCollision = hasCollision;
	}

	/// <summary>
	/// 自动生成一串由位置决定的绳子链
	/// </summary>
	/// <param name="positions"></param>
	public Rope(List<Vector2> positions, float elasticity, float mass, RenderingTransformFunction renderingTransform, bool hasCollision = false)
	{
		InitRopes(positions.Count, elasticity, renderingTransform, hasCollision);

		for (int i = 0; i < positions.Count; i++)
		{
			masses[i] = new _Mass
			{
				Mass = mass,
				Position = positions[i],
				IsStatic = i == positions.Count - 1,
			};
		}

		for (int i = 0; i < positions.Count - 1; i++)
		{
			springs[i] = new Spring
			{
				Elasticity = elasticity,
				RestLength = (positions[i] - positions[i + 1]).Length(),
				A = i,
				B = i + 1,
			};
		}
	}

	/// <summary>
	/// 自动生成一串由两端位置决定的绳子链,且两端固定
	/// </summary>
	/// <param name="positions"></param>
	public Rope(Vector2 start, Vector2 end, int count, float elasticity, float mass, RenderingTransformFunction renderingTransform, bool hasCollision = false, int knotDistance = 0, float knotMass = 1)
	{
		List<Vector2> positions = new List<Vector2>();
		for (int t = 0; t <= count; t++)
		{
			positions.Add(Vector2.Lerp(start, end, t / (float)count));
		}
		InitRopes(positions.Count, elasticity, renderingTransform, hasCollision);

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
			masses[i] = new _Mass
			{
				Mass = specialMass,
				Position = positions[i],
				IsStatic = (i == positions.Count - 1) || (i == 0),
			};
		}

		for (int i = 0; i < positions.Count - 1; i++)
		{
			springs[i] = new Spring
			{
				Elasticity = elasticity,
				RestLength = (positions[i] - positions[i + 1]).Length(),
				A = i,
				B = i + 1,
			};
		}
	}

	/// <summary>
	/// 自动生成一串给定长度的绳子系统，质量和大小随机
	/// </summary>
	/// <param name="position"></param>
	/// <param name="elasticity"></param>
	/// <param name="scale"></param>
	/// <param name="count"></param>
	public Rope(Vector2 position, float elasticity, float scale, int count, RenderingTransformFunction renderingTransform, bool hasCollision = false)
	{
		InitRopes(count, elasticity, renderingTransform, hasCollision);
		Random rand = Random.Shared;

		masses[0] = new _Mass
		{
			Mass = scale * ((float)rand.NextDouble() * 0.68f + 1),
			Position = position,
			IsStatic = true,
		};

		masses[^1] = new _Mass
		{
			Mass = scale * ((float)rand.NextDouble() * 0.68f + 1) * 1.3f,
			Position = position + new Vector2(0, 6 * count - 6),
			IsStatic = false,
		};
		for (int i = 1; i < count - 1; i++)
		{
			masses[i] = new _Mass
			{
				Mass = scale * ((float)rand.NextDouble() * 0.68f + 1),
				Position = position + new Vector2(0, 6 * i),
				IsStatic = false,
			};
		}

		for (int i = 0; i < count - 1; i++)
		{
			springs[i] = new Spring
			{
				Elasticity = elasticity,
				RestLength = 20f,
				A = i,
				B = i + 1,
			};
		}
	}
}